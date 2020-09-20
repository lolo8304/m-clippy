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
using Microsoft.Extensions.Logging;
using Polly;

namespace m_clippy.Services
{
    public class ReportingService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        private readonly MigrosService _migrosService;

        private ILogger<ReportingService> _logger;

        public ReportingService(ClippyStorage clippyStorage,
            MigrosService migrosService,
            IConfiguration configuration,
            ILogger<ReportingService> logger)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
            _migrosService = migrosService;
            _logger = logger;
        }

        public async Task<ClippyProductsDetails> getPurchases(string userId)
        {
            var user = _clippyStorage.GetUser(userId);
            if (user == null)
            {
                user = new Mocks().User1();
                user = _clippyStorage.PutUser(userId, user);
            }

            string clientId = user.ClientId;


            // HACK due to bad performance environment 
            int limitPurchase = 8;
            int limitCartItem = 4; // never go below 2 or the api call below products?ids=a,b,c,d wont work


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
            Random r = new Random();
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

                foreach (ProductDetail productDetail in productDetails.Products)
                {
                    clippyProductsDetails.ProductsAnalyzed++;

                    var clippyProductDetail = new ClippyProductDetail
                    {
                        Thumbnail = productDetail?.ImageTransparent?.Stack.ToString().Replace("{stack}", "small"),
                        Image = productDetail?.ImageTransparent?.Stack.ToString().Replace("{stack}", "medium"),

                        // TODO should be historical to shoppingcart date :-)
           
                        Quantity = productDetail?.Price?.Base?.Quantity.ToString() + productDetail?.Price?.Base?.Unit.ToString(),

                        ArticleID = productDetail?.Id.ToString()
                    };

                    if (productDetail.Price != null)
                    {
                        if (productDetail.Price.Base != null)
                        {
                           clippyProductDetail.Price = productDetail.Price.Base.Price;
                        }
                    }
                    else {
                        clippyProductDetail.Price = 0.0;
                    }

                    //n some product have no quantities
                    if (clippyProductDetail.Quantity.Equals(""))
                    {
                        clippyProductDetail.Quantity = "1";
                    }

                    if (productDetail.Features != null)
                    {
                        var productAllergens = productDetail?.Features.FindAll(s => s.LabelCode.Equals("MAPI_ALLERGENES"));
                        foreach (Models.ProductDetails.Feature productAllergen in productAllergens)
                        {
                            foreach (Value value in productAllergen.Values)
                            {
                                string allergen = value.ValueCode.ToString();

                                if (user.Allergies.Matching.Contains(allergen))
                                {
                                    clippyProductDetail.AllergyAlert = true;

                                    clippyProductsDetails.AllergyCounter++;
                                }

                                // keep track of all allergens
                                clippyProductsDetails.allergens.AddOrUpdate(allergen, 1, (allergen, count) => count + 1);

                                clippyProductsDetails.AllergensCounter++;
                            }
                        }

                        if (productAllergens.Count == 0)
                        {
                            clippyProductsDetails.NoAllergensCounter++;
                        }
                    }

                    bool sumAdded = false;
                    // user value first produced in CH
                    if (user.Locations.National == 1)
                    {
                        if (productDetail.Origins != null) {
                            if (productDetail.Origins.ProducingCountry != null)
                            {
                                bool inCH = productDetail.Origins.ProducingCountry.Equals("Hergestellt in der Schweiz");
                                if (!inCH)
                                {
                                    clippyProductDetail.LocationAlert = true;
                                    clippyProductsDetails.LocationCounter++;
                                }
                            }
                        }

                        if (productDetail.Labels != null)
                        {
                            List<Models.ProductDetails.Label2> HasCHLabel = productDetail?.Labels.FindAll(s => s.Name.Equals("Swissness"));
                            if (HasCHLabel.Count == 0)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                            else
                            {
                                // HACK data not good enough
                    
                                int range = 100;
                                double rDouble = r.NextDouble() * range;
                                clippyProductsDetails.NationalSum += rDouble + Convert.ToDouble(clippyProductDetail.Price);
                                sumAdded = true;
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
                            else
                            {
                                // HACK data not good enough
                                int range = 100;
                                double rDouble = r.NextDouble() * range;
                                clippyProductsDetails.RegionalSum += rDouble + Convert.ToDouble(clippyProductDetail.Price);
                                sumAdded = true;
                            }
                        }

                    }

                    if (productDetail.Origins != null)
                    {
                        if (productDetail.Origins.ProducingCountry != null)
                        {
                            string country = productDetail.Origins.ProducingCountry.ToString().ToLower();
                            CountriesSet.Add(country);

                            //fixing temporary mess of data 
                            if (country.Contains("in der schweiz")
                                || country.Contains("suisse")
                                || country.Contains("schweizer produkt")
                                ) {
                                country = "schweiz";
                            }

                            // keep track of all ProducingCountry
                            clippyProductsDetails.ProducingCountries.AddOrUpdate(country, 1, (allergen, count) => count + 1);
                        }
                    }

                    if (!sumAdded)
                    {
                        // HACK data not good enough
                        int range = 100;
                        double rDouble = r.NextDouble() * range;
                        clippyProductsDetails.OutsideSum += rDouble + Convert.ToDouble(clippyProductDetail.Price);
                        sumAdded = true;
                    }

              
                    if (productDetail.Labels != null)
                    {
                        List<Models.ProductDetails.Label2> isVegan = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegan"));
                        if (isVegan.Count == 0)
                        {
                            clippyProductDetail.HabitsAlert = true;
                            clippyProductsDetails.HabitsCounter++;

                            clippyProductsDetails.NotVeganCounter++;
                        }
                        else
                        {
                            clippyProductsDetails.VeganCounter++;
                        }

                        List<Models.ProductDetails.Label2> isVeganBlume = productDetail?.Labels.FindAll(s => s.Name.Equals("Veganblume"));
                        if (isVeganBlume.Count == 0)
                        {
                            clippyProductDetail.HabitsAlert = true;
                            clippyProductsDetails.HabitsCounter++;

                            clippyProductsDetails.NotVeganCounter++;
                        }
                        else
                        {
                            clippyProductsDetails.VeganCounter++;
                        }
                    }
                    else
                    {
                        clippyProductsDetails.NotVeganCounter++;
                    }


                    if (productDetail.Labels != null)
                    {
                        List<Models.ProductDetails.Label2> isVegetarian = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegetarisch"));
                        if (isVegetarian.Count == 0)
                        {
                            clippyProductDetail.HabitsAlert = true;
                            clippyProductsDetails.HabitsCounter++;

                            clippyProductsDetails.NotVegetarianCounter++;
                        }
                        else
                        {
                            clippyProductsDetails.VegetarianCounter++;
                        }
                    }
                    else
                    {
                        clippyProductsDetails.NotVegetarianCounter++;
                    }


                    if (productDetail.Labels != null)
                    {
                        List<Models.ProductDetails.Label2> isBIO = productDetail?.Labels.FindAll(s => s.Name.Equals("Migros Bio"));
                        if (isBIO.Count == 0)
                        {
                            clippyProductDetail.HabitsAlert = true;
                            clippyProductsDetails.HabitsCounter++;

                            clippyProductsDetails.NotBioCounter++;
                        }
                        else
                        {
                            clippyProductsDetails.BioCounter++;
                        }
                    }
                    else
                    {
                        clippyProductsDetails.NotBioCounter++;
                    }


                    clippyProductsDetails.list.Add(clippyProductDetail);
                }
            }

            // we just count the unique number of different countries from all articles bought by customer
            clippyProductsDetails.CountriesCounter = CountriesSet.Count;


            // VISION but not time to build geolocation distances nor CO2 calculations
            clippyProductsDetails.CarKm = r.Next(5, 100) + " km";
            clippyProductsDetails.PlanesKm = r.Next(10, 500) + " km";


            // Naive counter impl
            int score = 100;
            if (user.Habits.Bio) {
                score = score - clippyProductsDetails.NotBioCounter;
            }
            if (user.Habits.Vegan)
            {
                score = score - clippyProductsDetails.NotVeganCounter;
            }
            if (user.Habits.Vegetarian)
            {
                score = score - clippyProductsDetails.NotVegetarianCounter;
            }
            if (clippyProductsDetails.Score < 0)
            {
                clippyProductsDetails.Score = 0;
            }


            clippyProductsDetails.ProducingCountries2 = new SortedDictionary<string, int>(clippyProductsDetails.ProducingCountries);

            return clippyProductsDetails;
        }

    }
}
