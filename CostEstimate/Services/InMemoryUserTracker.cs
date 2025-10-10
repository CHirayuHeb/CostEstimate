using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System;
using System.Collections.Concurrent;

namespace CostEstimate.Services
{
    public class InMemoryUserTracker : IUserTracker
    {
        private readonly ConcurrentDictionary<string, DateTime> _map
            = new ConcurrentDictionary<string, DateTime>();

        public void UserLoggedIn(string username)
        {
            _map[username] = DateTime.UtcNow;
        }

        public void UserLoggedOut(string username)
        {
            _map.TryRemove(username, out _);
        }

        //public List<string> GetOnlineUsers()
        //{
        //    return _map.Keys.ToList();
        //}
        public List<(string Username, DateTime LastActive)> GetOnlineUsers()
        {
            return _map.Select(kv => (kv.Key, kv.Value)).ToList();
        }

    }
}
