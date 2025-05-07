using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade
{
    public static class SessionManagerFacade
    {
        public static User GetLoggedUser()
        {
            return SessionManager.GetCurrentUser();
        }
    }
}
