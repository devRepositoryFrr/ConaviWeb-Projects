using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Model.Response
{
    public class Response
    {
        public int Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
