using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Reporteador
{
    public class QRCoderModel
    {
        [Display(Name = "Enter QRCode Text")]
        public string QRCodeText { get; set; }
    }
}
