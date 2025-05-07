using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class DocType
    {
        [Key]
        public int ID_DocType { get; set; }
        public string Nombre { get; set; }
    }
}
