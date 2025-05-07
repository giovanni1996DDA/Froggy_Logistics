using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class UserPermisoRelation
    {
        [Key]
        public Guid? IdUser { get; set; }
        [Key]
        public Guid? IdPermiso { get; set; }
    }
}
