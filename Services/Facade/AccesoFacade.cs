using Services.Domain;
using Services.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Services.Facade
{
    public static class AccesoFacade
    {
        public static Acceso GetOne(Acceso acc)
        {
            return AccesoService.Instance.GetOne(acc);
        }

        public static Rol GetOne(Rol rol)
        {
            return AccesoService.Instance.GetOne(rol);
        }
        public static Permiso GetOne(Permiso permiso)
        {
            return AccesoService.Instance.GetOne(permiso);
        }
        public static List<Rol> Get(Rol rol)
        {
            return AccesoService.Instance.Get(rol);
        }
        public static List<Permiso> Get(Permiso rol)
        {
            return AccesoService.Instance.Get(rol);
        }
        public static void CreateOrUpdate(Rol rol)
        {
            AccesoService.Instance.CreateOrUpdate(rol);
        }
        public static void CreateOrUpdate(Permiso permiso)
        {
            AccesoService.Instance.CreateOrUpdate(permiso);
        }
        public static void AgregarPermisoaRol(Rol rolPadre, Permiso permisoHijo)
        {
            AccesoService.Instance.AgregarPermisoaRol(rolPadre, permisoHijo);
        }
        public static void RemoveRolFromRol(Rol rolPadre, Rol rolHijo)
        {
            AccesoService.Instance.RemoveRolFromRol(rolPadre, rolHijo);
        }
        public static void RemovePermisoFromRol(Rol rolPadre, Permiso permisoHijo)
        {
            AccesoService.Instance.RemovePermisoFromRol(rolPadre, permisoHijo);
        }
        public static bool Exists(Rol rol)
        {
            return AccesoService.Instance.Exists(rol);
        }
        public static bool Exists(Permiso permiso)
        {
            return AccesoService.Instance.Exists(permiso);
        }
        public static List<Permiso> GetPermisosFromUser(AppUser user)
        {
            return AccesoService.Instance.GetAllPermisosFromUser(user);
        }
        public static void Delete(Rol deletingRole)
        {
            AccesoService.Instance.Delete(deletingRole);
        }
        public static void Delete(Permiso deletingPermiso)
        {
            AccesoService.Instance.Delete(deletingPermiso);
        }
    }
}
