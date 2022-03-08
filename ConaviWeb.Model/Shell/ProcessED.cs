using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Shell
{
    public class ProcessED
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime DateVoBo1 { get; set; }
        public DateTime DateVoBo2 { get; set; }
        public DateTime DateED { get; set; }
        public string ProcessType { get; set; }
        public int IdUser { get; set; }
    }
}
