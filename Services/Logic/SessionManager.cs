using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    public static class SessionManager
    {
        private static AppUser currentUser;

        public static void SetUser(AppUser user)
        {
            currentUser = user;
        }

        public static AppUser GetCurrentUser()
        {
            return currentUser;
        }

        public static void Logout()
        {
            currentUser = null;
        }

        public static bool IsLoggedIn()
        {
            return currentUser != null;
        }
    }
}
