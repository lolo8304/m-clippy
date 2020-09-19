using System;
using System.Collections.Concurrent;
using m_clippy.Models;
using Microsoft.Extensions.Logging;

namespace m_clippy.Services
{
    public class ClippyStorage
    {
        public readonly ConcurrentDictionary<string, Habits> Habits = new ConcurrentDictionary<string, Habits>();

        public ClippyStorage(ILogger<ClippyStorage> logger)
        {
        }

        public Habits GetHabit(string userId)
        {
            return (Habits.TryGetValue(userId, out var value)) ? value : null;
        }

        public Habits PutHabits(string key, Habits value)
        {
            Habits.AddOrUpdate(key, value, (key, old) => { return value; });
            return Habits.AddOrUpdate(key, value, (key, old) => value);
        }

    }
}
