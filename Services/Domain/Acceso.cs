using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
/// <summary>
/// Representa el composite de los permisos del usuario
/// </summary>
    public abstract class Acceso
    {
        [Key, Column(Order = 0)] // Primera columna en la clave primaria compuesta
        public Guid? Id { get; set; }
        public int Codigo { get; set; }

        [Required(ErrorMessage = "Se debe especificar un nombre.")]
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        [NotMapped]
        public abstract bool HasChildren { get; }

        public Acceso()
        {
        }


/// <summary>
/// Agrega un hijo
/// </summary>
/// <param name="component"></param>
        public abstract void Add(Acceso component);

/// <summary>
/// Quita un hijo
/// </summary>
/// <param name="component"></param>
        public abstract void Remove(Acceso component);

    }
}
