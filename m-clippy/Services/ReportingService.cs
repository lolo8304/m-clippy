using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Flurl.Http;
using m_clippy.Models;
using m_clippy.Models.Cart;
using m_clippy.Models.ProductsDetails;
using m_clippy.Models.Purchase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Scotch;

namespace m_clippy.Services
{
    public class ReportingService
    {
        private readonly IConfiguration _configuration;
        private readonly ClippyStorage _clippyStorage;
        
        public static async Task<T> GetJsonAsync<T>(IConfiguration configuration, string url, string fileNameCache)
        {
            var migrosUserName = configuration["MigrosApiUsername"];
            var migrosPassword = configuration["MigrosApiPassword"];

            // https://github.com/mleech/scotch
            var httpClient = HttpClients.NewHttpClient($"./cassette/{fileNameCache}.json", Startup.GetScotchMode());
            return await GetResultFromHttpClientAsync<T>(url, migrosUserName, migrosPassword, httpClient);
        }

        private static async Task<T> GetResultFromHttpClientAsync<T>(string url, string migrosUserName, string migrosPassword, HttpClient httpClient)
        {
            var cli = new FlurlClient(httpClient);
            var result = await cli.Request(url).WithHeader("Api-Version", "7")
                .WithHeader("accept-language", "de")
                .WithBasicAuth(migrosUserName, migrosPassword)
                .GetJsonAsync<T>();
            return result;
        }

        public ReportingService(ClippyStorage clippyStorage,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _clippyStorage = clippyStorage;
        }

        public async Task<ClippyProductsDetails> GetPurchases(string userId)
        {
            var user = _clippyStorage.GetUser(userId);
            if (user == null)
            {
                user = new Mocks().GetMockUserById(userId);
                user = _clippyStorage.PutUser(userId, user);
            }

            var clientId = user.ClientId;

            // HACK due to bad performance environment 
            const int limitPurchase = 8;

            var purchases = new Purchases()
            {
                purchases = await GetJsonAsync<List<Purchase>>(_configuration,
                    $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}", "purchases")
            };

            // we map to our own structure
            var clippyProductsDetails = new ClippyProductsDetails();
            var countriesSet = new HashSet<string>();
            
            // TODO filter by date
            foreach (Purchase purchase in purchases.purchases.GetRange(0, limitPurchase))
            {
                var purchaseId = purchase.EinkaufID;
                var cart = new Cart()
                {
                    cartItems = await GetJsonAsync<List<CartItem>>(_configuration,
                        $"https://hackzurich-api.migros.ch/hack/purchase/{clientId}/{purchaseId}/articles", "articles")
                };
                
                var productIdList = cart.cartItems.Select(cartItem => cartItem.ArtikelID.ToString()).ToList();
                var productIds = string.Join(",", productIdList);
                var productDetails =
                    await GetJsonAsync<ProductDetails>(_configuration,
                        $"https://hackzurich-api.migros.ch/products.json?ids={productIds}&verbosity=detail", "products");

                var productDetailsProducts = productDetails.Products;
                foreach (var productDetail in productDetailsProducts)
                {
                    clippyProductsDetails.ProductsAnalyzed++;

                    analyseOneProduct(productDetail, user, clippyProductsDetails, countriesSet);
                }
            }

            // we just count the unique number of different countries from all articles bought by customer
            clippyProductsDetails.CountriesCounter = countriesSet.Count;


            // VISION but not time to build geolocation distances nor CO2 calculations
            var r = new Random();
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

        public static void analyseOneProduct(ProductDetail productDetail, User user,
            ClippyProductsDetails clippyProductsDetails, HashSet<string> countriesSet)
        {
            var r = new Random();
            var clippyProductDetail = new ClippyProductDetail
            {
                Thumbnail = productDetail?.Image?.Stack.Replace("{stack}", "small"),
                Image = productDetail?.Image?.Stack.Replace("{stack}", "medium"),
                Original = productDetail?.Image?.Stack.Replace("{stack}", "original"),

                // TODO should be historical to shopping-cart date :-) i dont see this in their api
                Quantity = productDetail?.Price?.Base?.Quantity +
                           productDetail?.Price?.Base?.Unit,

                ArticleID = productDetail?.Id,

                Name = productDetail?.Name
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
                        var allergen = value.ValueCode;

                        if (user.Allergies.Matching.Contains(allergen))
                        {
                            clippyProductDetail.AllergyAlert = true;

                            clippyProductsDetails.AllergyCounter++;
                        }

                        // keep track of all allergens
                        clippyProductsDetails.allergens.AddOrUpdate(allergen, 1,
                            (anAllergen, count) => count + 1);

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
                    string country = productDetail.Origins.ProducingCountry.ToLower();
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
}