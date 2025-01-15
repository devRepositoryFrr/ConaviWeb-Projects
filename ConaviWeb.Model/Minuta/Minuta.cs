using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Minuta
{
    public class Minuta
    {
        public int Id { get; set; }
        public int IdReunion { get; set; }
        public int IdTema { get; set; }
        public string Tema { get; set; }
        public string Asunto { get; set; }
        public string Interno { get; set; }
        public string Externo { get; set; }
        public string Contexto { get; set; }
        public string Descripcion { get; set; }
        public DateTime FchCreate { get; set; }
    }
}
