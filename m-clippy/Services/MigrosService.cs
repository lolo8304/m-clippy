﻿using System;
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
using Microsoft.Extensions.Logging;
using Polly;

namespace m_clippy.Services
{
    public class MigrosService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        private ILogger<MigrosService> _logger;

        public MigrosService(ClippyStorage clippyStorage, IConfiguration configuration, ILogger<MigrosService> logger)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
            _logger = logger;
        }

        public async Task<ClippyProductsDetails> getPurchases(string userId)
        {
            var user = _clippyStorage.GetUser(userId);
            string clientId = user.ClientId;


            // HACK due to bad performance environment 
            int limitPurchase = 4;
            int limitCartItem = 3; // never go below 2 or the api call below products?ids=a,b,c,d wont work


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

            _logger.LogDebug("Found " + purchases.purchases.Count + " purchases");

            var CountriesSet = new HashSet<string>();

            // TODO filter by date
            foreach (Purchase purchase in purchases.purchases.GetRange(0, limitPurchase))
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
                _logger.LogDebug("Found " + cart.cartItems.Count + " cart items in purchase " + purchase.EinkaufID);


                var productIdList = new List<string>();
                foreach (CartItem cartItem in cart.cartItems) 
                {
                    productIdList.Add(cartItem.ArtikelID.ToString());
                }
                var productIds = String.Join(",", productIdList);

                var productDetails = await $"https://hackzurich-api.migros.ch/products.json?ids={productIds}&verbosity=detail"
                        .WithHeader("Api-Version", "7")
                        .WithHeader("accept-language", "de")
                        .WithBasicAuth(u, p)
                        .GetJsonAsync<ProductDetails>();

                _logger.LogDebug("Found " + cart.cartItems.Count + " cart items in purchase " + purchase.EinkaufID + " with " + productDetails.Products.Count);

                foreach (ProductDetail productDetail in productDetails.Products.GetRange(0, 1))
                {
                    var clippyProductDetail = new ClippyProductDetail
                    {
                        Thumbnail = productDetail.ImageTransparent.Stack.ToString().Replace("{stack}", "small"),
                        Image = productDetail.ImageTransparent.Stack.ToString().Replace("{stack}", "medium"),

                        // TODO should be historical to shoppingcart date :-)
                        Price = productDetail.Price?.Base?.Price.ToString(),
                        Quantity = productDetail?.Price?.Base?.Quantity.ToString() + productDetail?.Price?.Base?.Unit.ToString(),

                        ArticleID = productDetail?.Id.ToString()
                    };

                    // if it match user settings
                    var productAllergens = productDetail?.Features.FindAll(s => s.LabelCode.Equals("MAPI_ALLERGENES"));
                    foreach (Models.ProductDetails.Feature productAllergen in productAllergens)
                    {
                        foreach (Value value in productAllergen.Values)
                        {
                            if (user.Allergies.Matching.Contains(value.ValueCode.ToString()))
                            {
                                clippyProductDetail.AllergyAlert = true;

                                clippyProductsDetails.AllergyCounter++;
                            }
                        }
                    }

                    // user value first produced in CH
                    if (user.Locations.National == 1) {
                        bool inCH = productDetail.Origins.ProducingCountry.Equals("Hergestellt in der Schweiz");
                        if (!inCH) {
                            clippyProductDetail.LocationAlert = true;
                            clippyProductsDetails.LocationCounter++;
                        }

                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> HasCHLabel = productDetail?.Labels.FindAll(s => s.Name.Equals("Swissness"));
                            if (HasCHLabel.Count == 0)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                        }
                    }

                    if (user.Locations.Regional == 1)
                    {
                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> Regional = productDetail?.Labels.FindAll(s => s.Name.Equals("Aus der Region"));
                            if (Regional.Count == 0)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                        }

                    }

                    if (productDetail.Origins != null)
                    {
                        if (productDetail.Origins.ProducingCountry != null)
                        {
                            CountriesSet.Add(productDetail.Origins.ProducingCountry.ToString().ToLower());
                        }
                    }

                    if (user.Habits.Vegan)
                    {
                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> isVegan = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegan"));
                            if (isVegan.Count == 0)
                            {
                                clippyProductDetail.HabitsAlert = true;
                                clippyProductsDetails.HabitsCounter++;
                            }

                            List<Models.ProductDetails.Label2> isVeganBlume = productDetail?.Labels.FindAll(s => s.Name.Equals("Veganblume"));
                            if (isVeganBlume.Count == 0)
                            {
                                clippyProductDetail.HabitsAlert = true;
                                clippyProductsDetails.HabitsCounter++;
                            }
                        }
                    }

                    if (user.Habits.Vegetarian)
                    {
                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> isVegetarian = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegetarisch"));
                            if (isVegetarian.Count == 0)
                            {
                                clippyProductDetail.HabitsAlert = true;
                                clippyProductsDetails.HabitsCounter++;
                            }
                        }
                    }

                    if (user.Habits.Bio)
                    {
                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> isBIO = productDetail?.Labels.FindAll(s => s.Name.Equals("Migros Bio"));
                            if (isBIO.Count == 0)
                            {
                                clippyProductDetail.HabitsAlert = true;
                                clippyProductsDetails.HabitsCounter++;
                            }
                        }
                    }

                    clippyProductsDetails.list.Add(clippyProductDetail);
                }
            }

            // we just count the unique number of different countries from all articles bought by customer
            clippyProductsDetails.CountriesCounter = CountriesSet.Count;


            // Naive counter impl
            clippyProductsDetails.Score = 100 - (clippyProductsDetails.AllergyCounter + clippyProductsDetails.LocationCounter +
                clippyProductsDetails.HabitsCounter);
            if (clippyProductsDetails.Score < 0) {
                clippyProductsDetails.Score = 0;
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
