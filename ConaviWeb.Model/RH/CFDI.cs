using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.RH
{
    public class CFDI
    {
        public string SERIE { get; set; }
        public string FOLIO { get; set; }
        public string RFCEmisor { get; set; }
        public string RFCReceptor { get; set; }
        public string TOTAL { get; set; }
        public string UUID { get; set; }
        public string estado { get; set; }
        public string codigoEstatus { get; set; }
        public string esCancelable { get; set; }
        public string estatusCancelacion { get; set; }
        public string validacionEFOS { get; set; }
    }
}
