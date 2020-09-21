﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Cart;
using m_clippy.Models.ProductsDetails;
using m_clippy.Models.Purchase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace m_clippy.Services
{
    public class ReportingService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        private readonly MigrosService _migrosService;
        private ILogger<ReportingService> _logger;

        public static async Task<T> GetJsonAsync<T>(IConfiguration configuration, string url)
        {
            var u = configuration["MigrosApiUsername"];
            var p = configuration["MigrosApiPassword"];

            var result =
                await url
                    .WithHeader("Api-Version", "7")
                    .WithHeader("accept-language", "de")
                    .WithBasicAuth(u, p)
                    .GetJsonAsync<T>();

            return result;
        }

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

        public async Task<ClippyProductsDetails> GetPurchases(string userId)
        {
            var user = _clippyStorage.GetUser(userId);
            if (user == null)
            {
                user = new Mocks().User1();
                user = _clippyStorage.PutUser(userId, user);
            }

            var clientId = user.ClientId;

            // HACK due to bad performance environment 
            const int limitPurchase = 8;
            const int limitCartItem = 4; // never go below 2 or the api call below products?ids=a,b,c,d wont work

            var purchases = new Purchases()
            {
                purchases = await GetJsonAsync<List<Purchase>>(_configuration,
                    $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}")
            };

            // we map to our own structure
            var clippyProductsDetails = new ClippyProductsDetails();
            var countriesSet = new HashSet<string>();
            
            var r = new Random();
            // TODO filter by date
            foreach (Purchase purchase in purchases.purchases.GetRange(0, limitPurchase))
            {
                var purchaseId = purchase.EinkaufID;
                var cart = new Cart()
                {
                    cartItems = await GetJsonAsync<List<CartItem>>(_configuration,
                        $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}/{purchaseId}/articles")
                };
                
                var productIdList = cart.cartItems.Select(cartItem => cartItem.ArtikelID.ToString()).ToList();
                var productIds = string.Join(",", productIdList);
                var productDetails =
                    await GetJsonAsync<ProductDetails>(_configuration,
                        $"https://hackzurich-api.migros.ch/products.json?ids={productIds}&verbosity=detail");

                foreach (var productDetail in productDetails.Products)
                {
                    clippyProductsDetails.ProductsAnalyzed++;

                    var clippyProductDetail = new ClippyProductDetail
                    {
                        Thumbnail = productDetail?.Image?.Stack.ToString().Replace("{stack}", "small"),
                        Image = productDetail?.Image?.Stack.ToString().Replace("{stack}", "medium"),
                        Original = productDetail?.Image?.Stack.ToString().Replace("{stack}", "original"),

                        // TODO should be historical to shopping-cart date :-) i dont see this in their api
                        Quantity = productDetail?.Price?.Base?.Quantity.ToString() +
                                   productDetail?.Price?.Base?.Unit.ToString(),

                        ArticleID = productDetail?.Id.ToString(),

                        Name = productDetail?.Name.ToString()
                    };

                    if (productDetail.Price != null)
                    {
                        if (productDetail.Price.Base != null)
                        {
                            clippyProductDetail.Price = productDetail.Price.Base.Price;
                        }
                    }
                    else
                    {
                        clippyProductDetail.Price = 0.0;
                    }

                    //In some product we have no quantities
                    if (clippyProductDetail.Quantity.Equals(""))
                    {
                        clippyProductDetail.Quantity = "1";
                    }

                    if (productDetail.Features != null)
                    {
                        var productAllergens =
                            productDetail?.Features.FindAll(s => s.LabelCode.Equals("MAPI_ALLERGENES"));
                        foreach (Feature productAllergen in productAllergens)
                        {
                            foreach (Value value in productAllergen.Values)
                            {
                                var allergen = value.ValueCode.ToString();

                                if (user.Allergies.Matching.Contains(allergen))
                                {
                                    clippyProductDetail.AllergyAlert = true;

                                    clippyProductsDetails.AllergyCounter++;
                                }

                                // keep track of all allergens
                                clippyProductsDetails.allergens.AddOrUpdate(allergen, 1,
                                    (allergen, count) => count + 1);

                                clippyProductsDetails.AllergensCounter++;
                            }
                        }

                        if (productAllergens.Count == 0)
                        {
                            clippyProductsDetails.NoAllergensCounter++;
                        }
                    }

                    var sumAdded = false;
                    // user value first produced in CH
                    if (user.Locations.National == 1)
                    {
                        if (productDetail.Origins?.ProducingCountry != null)
                        {
                            var inCh = productDetail.Origins.ProducingCountry.Equals("Hergestellt in der Schweiz");
                            if (!inCh)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                        }

                        if (productDetail.Labels != null)
                        {
                            var hasChLabel = productDetail?.Labels.FindAll(s => s.Name.Equals("Swissness"));
                            if (hasChLabel.Count == 0)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                            else
                            {
                                // HACK data not good enough

                                const int range = 100;
                                var rDouble = r.NextDouble() * range;
                                clippyProductsDetails.NationalSum +=
                                    rDouble + Convert.ToDouble(clippyProductDetail.Price);
                                sumAdded = true;
                            }
                        }
                    }

                    if (user.Locations.Regional == 1)
                    {
                        if (productDetail.Labels != null)
                        {
                            var regional = productDetail?.Labels.FindAll(s => s.Name.Equals("Aus der Region"));
                            if (regional.Count == 0)
                            {
                                clippyProductDetail.LocationAlert = true;
                                clippyProductsDetails.LocationCounter++;
                            }
                            else
                            {
                                // HACK data not good enough
                                const int range = 100;
                                var rDouble = r.NextDouble() * range;
                                clippyProductsDetails.RegionalSum +=
                                    rDouble + Convert.ToDouble(clippyProductDetail.Price);
                                sumAdded = true;
                            }
                        }
                    }

                    if (productDetail.Origins != null)
                    {
                        if (productDetail.Origins.ProducingCountry != null)
                        {
                            string country = productDetail.Origins.ProducingCountry.ToString().ToLower();
                            countriesSet.Add(country);

                            //fixing temporary mess of data 
                            if (country.Contains("in der schweiz")
                                || country.Contains("suisse")
                                || country.Contains("schweizer produkt")
                            )
                            {
                                country = "schweiz";
                            }

                            // keep track of all ProducingCountry
                            clippyProductsDetails.ProducingCountries.AddOrUpdate(country, 1,
                                (allergen, count) => count + 1);
                        }
                    }

                    if (!sumAdded)
                    {
                        // HACK data not good enough
                        const int range = 100;
                        var rDouble = r.NextDouble() * range;
                        clippyProductsDetails.OutsideSum += rDouble + Convert.ToDouble(clippyProductDetail.Price);
                        sumAdded = true;
                    }

                    if (productDetail.Labels != null)
                    {
                        var isVegan = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegan"));
                        var isVeganB = productDetail?.Labels.FindAll(s => s.Name.Equals("Veganblume"));
                        if (isVegan.Count == 0 || isVeganB.Count == 0)
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
                        var isVegetarian = productDetail?.Labels.FindAll(s => s.Name.Equals("V-vegetarisch"));
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
                        var isBio = productDetail?.Labels.FindAll(s => s.Name.Equals("Migros Bio"));
                        if (isBio.Count == 0)
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
            clippyProductsDetails.CountriesCounter = countriesSet.Count;


            // VISION but not time to build geolocation distances nor CO2 calculations
            clippyProductsDetails.CarKm = r.Next(5, 100) + " km";
            clippyProductsDetails.PlanesKm = r.Next(10, 500) + " km";

            // Naive counter impl
            var score = 100;
            if (user.Habits.Bio)
            {
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

            return clippyProductsDetails;
        }
    }
}