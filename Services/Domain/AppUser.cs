using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class AppUser
    {
        [Key]
        public Guid? ID_User { get; set; }
        [Key]
        public int Legajo { get; set; }
        [Required
            (ErrorMessage = "El usuario es obligatorio.")]
        [Key]
        public string Username { get; set; }
        [Required
            (ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression
            (@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número, un símbolo y al menos 8 caracteres.")]
        public string Password { get; set; }
        [Required (ErrorMessage = "Se debe especificar un email.")]
        public string Email { get; set; }
        [Required
            (ErrorMessage = "Se debe especificar un nombre.")]
        public string Nombre { get; set; }
        [Required
            (ErrorMessage = "Se debe especificar un apellido.")]
        public string Apellido { get; set; }
        public bool Estado { get; set; }
        [NotMapped]
        public List<Acceso> Accesos { get; set; } = new List<Acceso>();
        public AppUser()
        {
            //Id = Guid.NewGuid();
        }
    }
}
