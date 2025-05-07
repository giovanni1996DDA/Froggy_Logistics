using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class UserRolRelation
    {
        [Key]
        public Guid? ID_User { get; set; }
        [Key]
        public Guid? ID_Rol { get; set; }
    }
}
