using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Request
{
    public class UserRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Usuario")]
        public string SUser { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Primer Apellido")]
        public string LName { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Segundo Apellido")]
        public string SLName { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; }


    }
}
