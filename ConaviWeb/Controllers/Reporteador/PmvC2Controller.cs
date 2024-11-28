using ConaviWeb.Commons;
using ConaviWeb.Data.Reporteador;
using ConaviWeb.Model.Reporteador;
using ConaviWeb.Model.Response;
using ConaviWeb.Tools;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Reporteador
{
    public class PmvC2Controller : Controller
    {
        private readonly IReporteadorRepository _reporteadorRepository;
        private readonly IWebHostEnvironment _environment;
        public PmvC2Controller(IWebHostEnvironment environment, IReporteadorRepository reporteadorRepository)
        {
            _reporteadorRepository = reporteadorRepository;
            _environment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            //await GetAllBenef();
            await GetAllBenef23();
            return Ok();
        }
        [HttpGet]

        public async Task<IActionResult> GetAllBenef()
        {
            int[] meses = { 3, 4, 5, 6, 7, 8 };
            
            foreach (var mes in meses)
            {
                var ids = await _reporteadorRepository.GetPMV24C2Ids(mes);
                foreach (var id_unico in ids)
                {
                    var beneficiario = await _reporteadorRepository.GetPMV24C2(id_unico);
                    GenerateSavePDFAsync(beneficiario);
                }
            }
                
            
            return Ok();
        }
        public async Task<IActionResult> GetAllBenef23()
        {
            string[] ids = {"2304023096",
"2304023144",
"2304024094",
"2304024118",
"2304024129",
"2304035255"};
                foreach (var id_unico in ids)
                {
                    var beneficiario = await _reporteadorRepository.GetPMVC2(id_unico);
                    GenerateSavePDFAsync(beneficiario);
                }


            return Ok();
        }
        public void GenerateSavePDFAsync(PevSol beneficiario)
        {
            var fileName = beneficiario.Id_unico + ".pdf";
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "PMV23SOL",beneficiario.Mes, beneficiario.Estado);
            if (!Directory.Exists(pathPdf))
                ProccessFileTools.CreateDirectory(pathPdf);
            var file = System.IO.Path.Combine(pathPdf, fileName);
            var iHeader = System.IO.Path.Combine(_environment.WebRootPath, "img", "headerConavi.png");
            var iFooter = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            PdfWriter writer = new PdfWriter(file);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document doc = new Document(pdfDoc);

            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));
            //MARGEN DEL DOCUMENTO
            doc.SetMargins(70, 50, 70, 50);
            //LOGICA PDF
            GetPDF(doc, beneficiario);
            doc.Close();
            //return file;
        }
        public Document GetPDF(Document doc, PevSol beneficiario)
        {
            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fonts = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            //TITULO DEL DOCUMENTO

            Paragraph titulo = new Paragraph("CUESTIONARIO PVS ENTREGA DE APOYOS")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetCharacterSpacing(1)
                .SetFont(fonts)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetFontSize(12);
            doc.Add(titulo);

            //INICIO DEL BLOQUE A
            //TablaBloque1  
            float[] tama = { 10, 10, 10, 10 };
            Table bloque1 = new Table(UnitValue.CreatePercentArray(tama));
            //bloque1.SetFixedPosition(1, 50, 500, 500);
            Cell tituloBloque1 = new Cell(1, 4)
                .Add(new Paragraph("BLOQUE A ''INFORMACIÓN DE LA PERSONA SOLICITANTE''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);

            Cell nombre = new Cell(1, 2)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Nombre"));
            Cell curp = new Cell(1, 2)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("CURP"));


            //Campos de base de datos
            Cell Rnombre = new Cell(2, 2)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Nombre));

            Cell Rcurp = new Cell(2, 2)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph(beneficiario.Curp));
            Cell estado = new Cell(3, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Entidad"));
            Cell municipio = new Cell(3, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Municipio"));
            Cell localidad= new Cell(3, 3)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Localidad"));
            Cell Restado = new Cell(4, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Estado));
            Cell Rmunicipio = new Cell(4, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Municipio));
            Cell Rlocalidad = new Cell(4, 3)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(fonte)
            .SetFontColor(new DeviceRgb(130, 27, 63))
            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(7)
            .Add(new Paragraph(beneficiario.Localidad));
            Cell situacion = new Cell(5, 4)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Qué línea de apoyo tiene la persona beneficiaria?"));
            Cell Rsituacion = new Cell(5, 4)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph(beneficiario.Linea_apoyo));
            bloque1.AddHeaderCell(tituloBloque1);
            bloque1.AddCell(nombre);
            bloque1.AddCell(curp);
            bloque1.AddCell(Rnombre);
            bloque1.AddCell(Rcurp);
            bloque1.AddCell(estado);
            bloque1.AddCell(municipio);
            bloque1.AddCell(localidad);
            bloque1.AddCell(Restado);
            bloque1.AddCell(Rmunicipio);
            bloque1.AddCell(Rlocalidad);
            bloque1.AddCell(situacion);
            bloque1.AddCell(Rsituacion);
            bloque1.AddCell(new Cell(6,4)
               .SetBorder(Border.NO_BORDER)
               .SetHeight(5));
            doc.Add(bloque1);

            Table bloque2 = new Table(UnitValue.CreatePercentArray(tama));
            Cell tituloBloque2 = new Cell(1, 4)
                .Add(new Paragraph("BLOQUE B ''CUESTIONARIO''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque2.AddHeaderCell(tituloBloque2);
                Cell apoyoDom = new Cell(2, 4)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿En qué utilizaría su apoyo de Mejoramiento o Ampliación?"));
                bloque2.AddCell(apoyoDom);
                Cell RApoyoDom = new Cell(3, 4)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.P1.Replace("|","\n")));
                bloque2.AddCell(RApoyoDom);
            Cell tiempo = new Cell(4, 4)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿Cuenta con quién le ayude para la realización de los trabajos?"));
            bloque2.AddCell(tiempo);
            Cell Rtiempo = new Cell(5, 4)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.Cuenta_ayude_trabajos));
            bloque2.AddCell(Rtiempo);
            bloque2.AddCell(new Cell(6, 4)
               .SetBorder(Border.NO_BORDER)
               .SetHeight(5));
            doc.Add(bloque2);
                Table bloque3 = new Table(UnitValue.CreatePercentArray(tama));
            Cell tituloBloque3 = new Cell(1, 4)
                .Add(new Paragraph("BLOQUE C ''EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque3.AddHeaderCell(tituloBloque3);
                Cell actaHechos1 = new Cell(2, 4)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Carta"));
                bloque3.AddCell(actaHechos1);
                var actaH1 = new Image(ImageDataFactory.Create(beneficiario.Img1));
            Cell RActaHechos1 = new Cell(3, 4)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH1.SetMaxWidth(200));
            bloque3.AddCell(RActaHechos1);
            Cell actaHechos2 = new Cell(4, 4)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Entrega"));
                bloque3.AddCell(actaHechos2);
                var actaH2 = new Image(ImageDataFactory.Create(beneficiario.Img2));
            Cell RActaHechos2 = new Cell(5, 4)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH2.SetMaxWidth(200));
            bloque3.AddCell(RActaHechos2);
            doc.Add(bloque3);
            return doc;
        }

        private class TextFooterEventHandler : IEventHandler
        {
            protected Document doc;
            protected string _header;
            protected string _footer;
            public TextFooterEventHandler(Document doc, string iHeader, string iFooter)
            {
                this.doc = doc;
                _header = iHeader;
                _footer = iFooter;
            }

            public void HandleEvent(Event currentEvent)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
                Rectangle pageSize = docEvent.GetPage().GetPageSize();
                PdfFont font = null;
                try
                {
                    font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                }
                catch (IOException e)
                {
                    Console.Error.WriteLine(e.Message);
                }

                float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
                                 + (pageSize.GetRight() - doc.GetRightMargin())) / 2;
                float headerY = pageSize.GetTop() - doc.GetTopMargin() + 10;
                float footerY = doc.GetBottomMargin();
                Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
                canvas
                    .SetFont(font)
                    .SetFontSize(5)
                    .ShowTextAligned("", coordX, headerY, TextAlignment.CENTER)
                    .ShowTextAligned("", coordX, footerY, TextAlignment.CENTER)

                    .Close();
                Image img = new Image(ImageDataFactory
                .Create(_header))
                .SetTextAlignment(TextAlignment.CENTER);
                canvas.Add(img);
                Image footer = new Image(ImageDataFactory
                  .Create(_footer))
                  .SetFixedPosition(10, 0)
                  .ScaleAbsolute(580, 70)
                  .SetTextAlignment(TextAlignment.CENTER);
                canvas.Add(footer);

            }

        }
        }
}
