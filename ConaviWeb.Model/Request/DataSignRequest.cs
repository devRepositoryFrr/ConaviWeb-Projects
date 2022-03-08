using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ConaviWeb.Model.Request
{
    public class DataSignRequest
    {
        [Display(Name = "Archivo .key")]
        public IFormFile KeySat { get; set; }
        [Display(Name = "Archivo .cer")]
        public IFormFile CerSat { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Contraseña")]
        public string PassFirmante { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Archivos")]
        public string ArrayFiles { get; set; }
    }
}
