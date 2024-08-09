using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Expedientes
{
    public class ExpedienteBibliohemerografico
    {
        public int NoProg { get; set; }
        public int Consecutivo { get; set; }
        public int Id { get; set; }
        public int Ejemplar { get; set; }
        public int IdTipoSoporte { get; set; }
        public string ClaveSoporte { get; set; }
        public string Soporte { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Tema { get; set; }
        public string Editorial { get; set; }
        public string IsbnIssn { get; set; }
        public int Anio { get; set; }
        public int Paginas { get; set; }
        public int Volumen { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdInventario { get; set; }
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string EsEditable { get; set; }
        public string Estatus { get; set; }
    }
}