using ConaviWeb.Model.Reporteador;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static QRCoder.PayloadGenerator;

namespace ConaviWeb.Controllers.Reporteador
{
    public class QRController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public QRController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View("../Reporteador/CreateQRCode");
        }
        public IActionResult CreateQRCode(QRCoderModel qRCode)
        {
            var PathConavi = Path.Combine(_environment.WebRootPath, "img", "conavi4.png");
            //PayLoads
            //Bookmark generator = new Bookmark("http://code-bude.net", "CONAVI");
            //ContactData generator = new ContactData(ContactData.ContactOutputType.VCard3, "Alonso", "Cacho");
            //WhatsAppMessage generator = new WhatsAppMessage("Hi John, what do you think about QR codes?");
            Url generator = new Url("https://sistemaintegraloperativo.conavi.gob.mx:9090/doc/RH/Conavi/CACHO_SILVA_ALONSO.PNG");
            string payload = generator.ToString();

            
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(payload, QRCodeGenerator.ECCLevel.H);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap qrCodeImage = QrCode.GetGraphic(20, Color.Black, Color.White, (Bitmap)Bitmap.FromFile(PathConavi),20);
            //Bitmap QrBitmap = QrCode.GetGraphic(60);
            qrCodeImage.Save("QRACS.jpg", ImageFormat.Jpeg);
            byte[] BitmapArray = qrCodeImage.BitmapToByteArray();
            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            ViewBag.QrCodeUri = QrUri;
            var fileQR = BitmapToBytesCode(qrCodeImage);
            //return File(fileQR, "application/image");
            return View("../Reporteador/CreateQRCode");
        }
        [NonAction]
        private static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }

    //Extension method to convert Bitmap to Byte Array
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
    
}
