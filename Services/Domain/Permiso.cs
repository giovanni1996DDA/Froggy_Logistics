using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Services.Domain
{
    public class Permiso : Acceso
    {
        /// <summary>
        /// Leaf de composite de permisos
        /// </summary>
        /// 
        public string Form { get; set; }

        public int? TipoPermiso { get; set; }

        /// <summary>
        /// Nunca tiene hijos, es un Leaf.
        /// </summary>
        public override bool HasChildren { 
            get 
            {
                return false;
            }
        }
        public Permiso()
        {
            /*IdPermiso = Guid.NewGuid();
            this.Id = IdPermiso;*/
        }
        /// 
        /// <param name="component"></param>
        public override void Add(Acceso component)
        {

            throw new Exception("No se puede agregar un elemento");

        }

        /// 
        /// <param name="component"></param>
        public override void Remove(Acceso component)
        {

            throw new Exception("No se puede quitar un elemento");

        }
    }

    public enum Modulo
    {
        UI,
        Control,
        UseCases
    }
}
