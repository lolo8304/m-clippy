using System.Collections.Concurrent;
using m_clippy.Models;
using Microsoft.Extensions.Logging;

namespace m_clippy.Services
{
    public class ClippyStorage
    {
        // Caching because of bad endpoint performances, better would be client side cacing or precalculation in backend
        private readonly ConcurrentDictionary<string, User> User = new ConcurrentDictionary<string, User>();

        // Caching because of bad endpoint performances, better would be client side cacing or precalculation in backend
        private readonly ConcurrentDictionary<string, ClippyProductsDetails> ClippyProductsDetails = new ConcurrentDictionary<string, ClippyProductsDetails>();

        private string Allergies;

        public ClippyStorage(ILogger<ClippyStorage> logger)
        {
        }

        public User GetUser(string userId)
        {
            return (User.TryGetValue(userId, out var value)) ? value : null;
        }

        public User PutUser(string key, User value)
        {
            User.AddOrUpdate(key, value, (key, old) => { return value; });
            return User.AddOrUpdate(key, value, (key, old) => value);
        }

        public ClippyProductsDetails GetClippyProductDetails(string userId)
        {
            return (ClippyProductsDetails.TryGetValue(userId, out var value)) ? value : null;
        }

        public ClippyProductsDetails PutClippyProductDetails(string key, ClippyProductsDetails value)
        {
            ClippyProductsDetails.AddOrUpdate(key, value, (key, old) => { return value; });
            return ClippyProductsDetails.AddOrUpdate(key, value, (key, old) => value);
        }

        public string GetAllergies()
        {
            return Allergies;
        }

        public void PutAllergies(string allergies)
        {
            Allergies = allergies;
        }

    }
}
