using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    /// <summary>
    /// Representa una familia de permisos
    /// </summary>
    public class Rol : Acceso
    {        
        private List<Acceso> _Accesos = new List<Acceso>();

        public override bool HasChildren { 
            get 
            { 
                return _Accesos.Any();
            } 
        }
        [NotMapped]
        public List<Acceso> Accesos
        {
            get
            {
                return _Accesos;
            }
        }

        public Rol(Acceso acceso = null)
        {
            if (acceso != null)
                Accesos.Add(acceso);
        }

        /// 
        /// <param name="component"></param>
        public override void Add(Acceso component)
        {
            //if (!_Accesos.Any())
                _Accesos.Add(component);
        }

        /// 
        /// <param name="component"></param>
        public override void Remove(Acceso component)
        {
            //Ver que no puedo quedarme sin hijos...

            //accesos.Remove(component);
            Accesos.RemoveAll(o => o.Id == component.Id);//Linq -> lambda exp. se ve más adelante
        }
    }
}
