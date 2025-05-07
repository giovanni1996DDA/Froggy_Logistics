using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class RolRolRelation
    {
        [Key]
        public Guid? FatherId { get; set; }
        [Key]
        public Guid? ChildId { get; set; }
    }
}
