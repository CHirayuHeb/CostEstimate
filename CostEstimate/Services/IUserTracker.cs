using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEstimate.Services
{
    public interface IUserTracker
    {
        void UserLoggedIn(string username);
        void UserLoggedOut(string username);
        //List<string> GetOnlineUsers();
        List<(string Username, DateTime LastActive)> GetOnlineUsers();
    }
}
