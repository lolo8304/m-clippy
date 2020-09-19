using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Allergens;
using m_clippy.Models.Cart;
using m_clippy.Models.Migros;
using m_clippy.Models.ProductDetails;
using m_clippy.Models.Purchases;
using Microsoft.Extensions.Configuration;

namespace m_clippy.Services
{
    public class MigrosService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;

        public MigrosService(ClippyStorage clippyStorage, IConfiguration configuration)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
        }

        public async Task<ClippyProductsDetails> getPurchases(string userId)
        {
            var user = _clippyStorage.GetUser(userId);
            string clientId = user.ClientId;

            var u = _configuration["MigrosApiUsername"];
            var p = _configuration["MigrosApiPassword"];

            var purchases = new Purchases()
            {
                purchases = await $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}"
                .WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<List<Purchase>>()
            };

            // we map to our own structure
            var clippyProductsDetails = new ClippyProductsDetails();

            // TODO filter by date
            foreach (Purchase purchase in purchases.purchases)
            {
                var einkaufID = purchase.EinkaufID;

                var cart = new Cart()
                {
                    cartItems = await $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}/{einkaufID}/articles"
                        .WithHeader("Api-Version", "7")
                        .WithHeader("accept-language", "de")
                        .WithBasicAuth(u, p)
                        .GetJsonAsync<List<CartItem>>()
                };

                var productIdList = new List<string>();
                foreach (CartItem cartItem in cart.cartItems.GetRange(0, 1)) // hack only 1 shoppingcart
                {
                    productIdList.Add(cartItem.ArtikelID.ToString());
                }
                var productIds = String.Join(",", productIdList.GetRange(0,1)); // hack only 3 products, need to filter scart by date

                var productDetails = await $"https://hackzurich-api.migros.ch/products/{productIds}"
                                .WithHeader("Api-Version", "7")
                                .WithHeader("accept-language", "de")
                                .WithBasicAuth(u, p)
                                .GetJsonAsync<ProductDetails>();

                foreach (ProductDetail product in productDetails.Products)
                {
                    var clippyProductDetail = new ClippyProductDetail();

                    clippyProductDetail.Thumbnail = product.ImageTransparent.Stack.ToString().Replace("{stack}", "small");
                    clippyProductDetail.Image = product.ImageTransparent.Stack.ToString().Replace("{stack}", "medium");

                    // TODO should be historical to shoppingcart date :-)
                    clippyProductDetail.Price = product.Price?.Base?.Price.ToString();
                    clippyProductDetail.Quantity = product?.Price?.Base?.Quantity.ToString() + product?.Price?.Base?.Unit.ToString();

                    // if it match user settings
                    var allAllergens = product?.Features.FindAll(s => s.LabelCode.Equals("MAPI_ALLERGENES"));
                    foreach (Models.ProductDetails.Feature allergen in allAllergens)
                    {
                        foreach (Value value in allergen.Values) {
                            if (user.Allergies.Matching.Contains(value.ValueCode.ToString())) {
                                clippyProductDetail.AllergyAlert = true;

                                clippyProductsDetails.AllergyCounter++;
                            }
                        }
                    }
                    
                    // not always defined nor available
                    clippyProductDetail.LocationAlert = true;


                    clippyProductDetail.HabitsAlert = true;


                    clippyProductsDetails.list.Add(clippyProductDetail);
                }
            }

            return clippyProductsDetails;
        }

        public async Task<AllergenList> GetAllergiesAsync()
        {
            var u = _configuration["MigrosApiUsername"];
            var p = _configuration["MigrosApiPassword"];
            var productList = await "https://hackzurich-api.migros.ch/products?feature_facets%5B%5D=MAPI_ALLERGENES&facet_size=0"
                .WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<ProductList>();

            // we map to our own structure
            var allergenList = new AllergenList();
            var terms = productList.Facets.MAPIALLERGENES.Terms;
            foreach (Term6 term in terms) {
                var allergenEntry = new AllergenEntry
                {
                    Code = term.Term,
                    Text = term.Name
                };

                allergenList.list.Add(allergenEntry);
            }

            return allergenList;
        }


        public async Task<Product> GetProductByIdAsync(string productId)
        {

            return await GetAsync(productId);
        }

        public async Task<Product> GetAsync(string productId)
        {
            var u = _configuration["MigrosApiUsername"];
            var p = _configuration["MigrosApiPassword"];
            return await $"https://hackzurich-api.migros.ch/products/{productId}.json"
                .WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(u, p)
                .GetJsonAsync<Product>();
        }



    }
}
