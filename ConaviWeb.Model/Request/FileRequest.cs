using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Request
{
    public class FileRequest
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name ="Archivo(s)")]
        public IFormFileCollection FileCollection { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Partición")]
        public int Partition { get; set; }
    }
}
