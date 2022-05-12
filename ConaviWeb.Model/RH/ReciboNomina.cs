using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.RH
{
    public class ReciboNomina
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Anio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Periodo { get; set; }
        public IFormFileCollection Recibos { get; set; }
    }
}
