using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain
{
    public class User
    {
        [Key]
        public Guid? Id { get; set; }
        [Required
            (ErrorMessage = "El usuario es obligatorio.")]
        [Key]
        public string UserName { get; set; }
        [Required
            (ErrorMessage = "La contraseña es obligatoria.")]
        [RegularExpression
            (@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número, un símbolo y al menos 8 caracteres.")]
        public string Password { get; set; }
        [Required
            (ErrorMessage = "Se debe especificar un nombre.")]
        public string Nombre { get; set; }
        [Required
            (ErrorMessage = "Se debe especificar un apellido.")]
        public string Apellido { get; set; }
        [Required
            (ErrorMessage = "Se debe especificar un email.")]
        public string Email { get; set; }
        [NotMapped] // Hay que sacarlo.
        public bool IsAnulated { get; set; }
        [NotMapped] // Hay que sacarlo.
        public bool PasswordResetted { get; set; }

        [NotMapped]
        public List<Acceso> Accesos { get; set; } = new List<Acceso>();
        public User()
        {
            //Id = Guid.NewGuid();
        }
    }
}
