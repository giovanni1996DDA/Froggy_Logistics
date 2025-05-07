using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Facade
{
    public class SecurityManagerFacade
    {
        public bool RequestForPermission(AppUser user, Acceso acceso)
        {
            return SecurityManagerService.Instance.HasAccess(user, acceso);
        }
    }
}
