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
        private static User currentUser;

        public static void SetUser(User user)
        {
            currentUser = user;
        }

        public static User GetCurrentUser()
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
