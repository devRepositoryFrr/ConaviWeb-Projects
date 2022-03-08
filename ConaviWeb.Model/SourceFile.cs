using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model
{
    public class SourceFile
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime DateUpload { get; set; }
        public string Hash { get; set; }
        public int IdUser { get; set; }
        public int NuSign { get; set; }
        public int IdStatus { get; set; }
        public int IdPartition { get; set; }

    }
}