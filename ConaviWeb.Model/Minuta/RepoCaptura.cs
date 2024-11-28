using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class RepoCaptura
    {
        public int Tipo_documento { get; set; }
        public string Folio { get; set; }
        public string Numero_interno { get; set; }
        public DateTime Fecha_sesion { get; set; }
        public int Numero_sesion { get; set; }
        public int Numero_progresivo { get; set; }
        public int Eje_id { get; set; }
        public DateTime Fecha_captura { get; set; }
        public DateTime Fecha_cumplimiento { get; set; }
        public int Tema_id { get; set; }
        public string Descripcion { get; set; }
        public string Responsable_institucional { get; set; }
        public int Enlace_institucional { get; set; }
        public string Corresponsables { get; set; }
        public int Responsable_coas { get; set; }
        public string Gestiones_realizadas { get; set; }
        public DateTime Fecha_termino { get; set; }
        public string Evidencia_cumplimiento { get; set; }
        public int Estatus_id { get; set; }
    }
}
