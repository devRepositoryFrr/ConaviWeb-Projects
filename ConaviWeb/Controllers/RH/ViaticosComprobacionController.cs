using ConaviWeb.Data.RH;
using ConaviWeb.Model.RH;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace ConaviWeb.Controllers.RH
{
    public class ViaticosComprobacionController : Controller
    {
        private readonly IRHRepository _rHRepository;
        public ViaticosComprobacionController(IRHRepository rHRepository)
        {
            _rHRepository = rHRepository;
        }
        public IActionResult Index()
        {
            return View("../RH/ViaticosComprobacion");
        }
        [HttpPost]
        public async Task<IActionResult> ValidaCFDIAsync([FromForm] IFormFile file, string folio)
        {
            CFDI xmlActual = new();
            byte[] archivoActual = ConvertToBytes(file);
            Stream stream = new MemoryStream(archivoActual);
            XmlTextReader xmlReader = new XmlTextReader(stream);
            string content = null;
            CFDI acuse = new();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.Name == "cfdi:Emisor"))
                {
                    if (xmlReader.HasAttributes)
                        xmlActual.RFCEmisor = xmlReader.GetAttribute("Rfc") == null ? xmlReader.GetAttribute("rfc") : xmlReader.GetAttribute("Rfc");
                }

                if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.Name == "cfdi:Receptor"))
                {
                    if (xmlReader.HasAttributes)
                        xmlActual.RFCReceptor = xmlReader.GetAttribute("Rfc") == null ? xmlReader.GetAttribute("rfc") : xmlReader.GetAttribute("Rfc");
                }

                if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.Name == "cfdi:Comprobante"))
                {
                    if (xmlReader.HasAttributes)
                    {
                        xmlActual.TOTAL = xmlReader.GetAttribute("Total") == null ? xmlReader.GetAttribute("total") : xmlReader.GetAttribute("Total");
                        //xmlActual.SERIE = xmlReader.GetAttribute("Serie") == null ? xmlReader.GetAttribute("serie") : xmlReader.GetAttribute("Serie");
                        //xmlActual.FOLIO = xmlReader.GetAttribute("Folio") == null ? xmlReader.GetAttribute("folio") : xmlReader.GetAttribute("Folio");
                    }
                }

                if (xmlReader.NodeType == XmlNodeType.Element && (xmlReader.Name == "tfd:TimbreFiscalDigital"))
                {
                    if (xmlReader.HasAttributes)
                        xmlActual.UUID = xmlReader.GetAttribute("UUID");
                }
            }
            using (var client = new HttpClient())
                {
                    //Send it
                    var response = await client.PostAsJsonAsync("https://localhost:44311/api/ConsultaCFDI", xmlActual);
                    content = await response.Content.ReadAsStringAsync();
                acuse = JsonSerializer.Deserialize<CFDI>(content);
                xmlActual.codigoEstatus = acuse.codigoEstatus;
                xmlActual.esCancelable = acuse.esCancelable;
                xmlActual.estado = acuse.estado;
                xmlActual.estatusCancelacion = acuse.estatusCancelacion;
                xmlActual.validacionEFOS = acuse.validacionEFOS;
                xmlActual.FOLIO = folio;
                //return Ok(contents);
            }
            if (acuse !=null)
            {
                var success = await _rHRepository.InsertComprobacion(xmlActual);
            }
            return Ok(acuse);
        }
        public byte[] ConvertToBytes(IFormFile file)
        {
            byte[] data = null;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                data = ms.ToArray();
                
                // act on the Base64 data
            }

            return data;
        }
        public async Task<IActionResult> GetComprobaciones(string folio)
        {
            
            var cfdi = await _rHRepository.GetComprobaciones(folio);
            return Json(new { data = cfdi });
        }
    }
}
