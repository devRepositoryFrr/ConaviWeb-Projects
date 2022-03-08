using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Response
{
    public class FileResponse
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime DateUpload { get; set; }
        public string Status { get; set; }
        public int IdPartition { get; set; }
    }
}
