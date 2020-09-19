using System;
using System.Collections.Concurrent;
using m_clippy.Models;
using Microsoft.Extensions.Logging;

namespace m_clippy.Services
{
    public class ClippyStorage
    {
        public readonly ConcurrentDictionary<string, User> User = new ConcurrentDictionary<string, User>();
        public string Allergies;

        public ClippyStorage(ILogger<ClippyStorage> logger)
        {
        }

        public User GetUser(string userId)
        {
            return (User.TryGetValue(userId, out var value)) ? value : null;
        }

        public string GetAllergies()
        {
            return Allergies;
        }

        public void PutAllergies(string allergies)
        {
            Allergies = allergies;
        }

        public User PutUser(string key, User value)
        {
            User.AddOrUpdate(key, value, (key, old) => { return value; });
            return User.AddOrUpdate(key, value, (key, old) => value);
        }

    }
}
