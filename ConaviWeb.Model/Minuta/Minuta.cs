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
        public string Folio { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Tema")]
        public string Tema { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Contexto")]
        public string Contexto { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }
        public int Estatus { get; set; }
        public List<Participante> Participantes { get; set; }
        public List<Acuerdo> Acuerdos { get; set; }
}
}
