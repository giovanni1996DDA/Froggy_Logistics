using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class RolPermisoRelation
    {
        [Key]
        public Guid? ID_Rol { get; set; }
        [Key]
        public Guid? ID_Permiso { get; set; }
    }
}
