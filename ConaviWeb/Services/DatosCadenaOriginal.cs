using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConaviWeb.Services
{
    public class DatosCadenaOriginal
    {
        //Datos generales archivo
        public string Sello { get; set; }

        public string Sistema { get; set; }

        public string Movimiento { get; set; }

        public string Formato { get; set; }

        public string Folio { get; set; }

        public string CurpBenef { get; set; }

        public string NombreBenef { get; set; }

        public string Tema { get; set; }

        //Datos generales firmante

        public string NombreFirmante { get; set; }

        public string RfcFirmante { get; set; }

        public int NumEmpleadoFirmante { get; set; }

        public string AreaFirmante { get; set; }

        public string PuestoFirmante { get; set; }

        //Datos firma

        public string HashArchivo { get; set; }

        public string CertificateNumber { get; set; }

        public string AlgoritmoFirma { get; set; }

        public DateTime TimeStampOCSP { get; set; }

        public DateTime TimeStampSign { get; set; }

        public int IdArchivo { get; set; }

        public string CadenaOriginal { get; set; }
    }
}
