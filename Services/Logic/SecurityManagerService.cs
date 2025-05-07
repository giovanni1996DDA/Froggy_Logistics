using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Logic
{
    internal class SecurityManagerService
    {
        #region singleton
        private static SecurityManagerService instance = new SecurityManagerService();

        public static SecurityManagerService Instance
        {
            get
            {
                return instance;
            }
        }
        private SecurityManagerService() { }
        #endregion

        public bool HasAccess(User user, Acceso requestedAcceso)
        {
            return UserService.Instance.GetPermisos(user.Accesos).Any(acceso => acceso.Id == requestedAcceso.Id);
        }
    }
}