using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostEstimate.Models.Common;
using CostEstimate.Services;
using Microsoft.AspNetCore.Mvc;

namespace CostEstimate.Controllers.Admin
{
    public class AdminController : Controller
    {
        private readonly IUserTracker _tracker;

        // Inject service เข้ามา
        public AdminController(IUserTracker tracker)
        {
            _tracker = tracker;
        }

        // แสดงรายชื่อ User ที่ออนไลน์
        public IActionResult OnlineUsers(Class @class)
        {
            @class._ListUserActivity = new List<UserActivity>();
            var users = _tracker.GetOnlineUsers();
            for (int i = 0; i < users.Count(); i++)
            {
                @class._ListUserActivity.Add(new UserActivity
                {
                    No = (i+1).ToString(),
                    Username = users[i].Username.ToString(),
                    LastActivityUtc = users[i].LastActive,
                });

            }

            return View(@class);
        }
    }
}