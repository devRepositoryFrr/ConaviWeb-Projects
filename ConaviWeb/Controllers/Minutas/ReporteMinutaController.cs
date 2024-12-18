using ConaviWeb.Data.Minuta;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using iText.Barcodes;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Renderer;
using Document = iText.Layout.Document;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfFont = iText.Kernel.Font.PdfFont;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using Rectangle = iText.Kernel.Geom.Rectangle;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace ConaviWeb.Controllers.Minutas
{
    public class ReporteMinutaController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public ReporteMinutaController(IWebHostEnvironment environment, IMinutaRepository minutaRepository)
        {
            _environment = environment;
            _minutaRepository = minutaRepository;
        }
        [Route("Download/{id?}")]
        public IActionResult Download(int id)
        {
            // Since this is just and example, I am using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            var path = Path.Combine(_environment.WebRootPath, "doc", "Minutas", "minuta_" + id + ".pdf");
            if (!System.IO.File.Exists(path))
            {
                return RedirectToAction("Index","ListaAcuerdos");
            }
            var fs = new FileStream(path, FileMode.Open);
            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", "minuta_" + id + ".pdf");
        }
        public async Task MinutaPdfAsync(int id)
        {
            var acuerdos = await _minutaRepository.GetAcuerdos(id);
            var minuta = await _minutaRepository.GetMinuta(id);
            var fecha = String.Concat(minuta.FchCreate.Day," de ", minuta.FchCreate.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")), " de ", minuta.FchCreate.Year);
            var _header = System.IO.Path.Combine(_environment.WebRootPath, "img", "SEDATU_Logo_(2024).png");
            var _footer = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            var pathCarta = Path.Combine(_environment.WebRootPath, "doc", "Minutas");
            if (!Directory.Exists(pathCarta))
            {
                Directory.CreateDirectory(pathCarta);
            }
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(Path.Combine(pathCarta,"minuta_"+minuta.Id+".pdf")));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, _header, _footer));

            doc.SetMargins(60, 40, 70, 40); //nuevo margen
            
            PdfFont cursiva = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont Bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            doc.SetFont(Bold);

            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            Paragraph saltoDeLinea = new Paragraph("");
            doc.Add(saltoDeLinea);

            Paragraph p = new Paragraph("Ciudad de México a " + fecha)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetMarginBottom(10)
                .SetFontSize(9);
            doc.Add(p);

            Table tableGnral = new Table(UnitValue.CreatePercentArray(1));
            tableGnral.SetWidth(UnitValue.CreatePercentValue(100));

            Cell cell = new Cell();

            p= new Paragraph("Tema:");
            cell.Add(p.SetFont(Bold).SetFontSize(10));

            p = new Paragraph(minuta.Tema)
                .SetMarginLeft(20)
                .SetMarginBottom(20);
            cell.Add(p);

            p = new Paragraph("Asunto tratado:");
            cell.Add(p.SetFont(Bold).SetFontSize(10));

            p = new Paragraph(minuta.Asunto)
                .SetMarginLeft(20)
                .SetMarginBottom(20);
            cell.Add(p);

            p = new Paragraph("Participantes:");
            cell.Add(p.SetFont(Bold).SetFontSize(10));

            p = new Paragraph("SEDATU: " + minuta.Interno)
                .SetMarginLeft(20);
            cell.Add(p);

            p = new Paragraph(minuta.Externo)
                .SetMarginLeft(20)
                .SetMarginBottom(20);
            cell.Add(p);

            tableGnral.AddCell(cell.SetBorder(Border.NO_BORDER).SetFont(fonte).SetFontSize(8));

            doc.Add(tableGnral);

            Table tableContext = new Table(UnitValue.CreatePercentArray(new float[] { 1 }));
            tableContext.SetWidth(UnitValue.CreatePercentValue(100));

            cell = new Cell();
            Cell cellContext = new Cell();

            p = new Paragraph("Contexto");
            cell.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);

            p = new Paragraph(minuta.Contexto)
                .SetMarginBottom(12);
            cellContext.Add(p);

            tableContext.AddCell(cell.SetFont(fonte).SetFontSize(10));
            tableContext.AddCell(cellContext.SetFont(fonte).SetFontSize(8));
            doc.Add(tableContext);

            Table tableDesc = new Table(UnitValue.CreatePercentArray(new float[] { 1 }));
            tableDesc.SetWidth(UnitValue.CreatePercentValue(100));

            cell = new Cell();
            Cell cellDesc = new Cell();

            p = new Paragraph("Descripción");
            cell.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);

            p = new Paragraph(minuta.Descripcion)
                .SetMarginBottom(12);
            cellDesc.Add(p);

            tableDesc.AddCell(cell.SetFont(fonte).SetFontSize(10));
            tableDesc.AddCell(cellDesc.SetFont(fonte).SetFontSize(8));
            doc.Add(tableDesc);

            doc.Add(saltoDeLinea);
            doc.Add(saltoDeLinea);

            float[] tamCol = { .3F, 1, 4, .5F };
            Table tableAcu = new Table(UnitValue.CreatePercentArray(tamCol),true);
            tableAcu.SetWidth(UnitValue.CreatePercentValue(100));
            Cell cellNum = new Cell();
            Cell cellRes = new Cell();
            Cell cellAcu = new Cell();
            Cell cellFto = new Cell();
            p = new Paragraph("Núm");
            cellNum.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);
            p = new Paragraph("Responsable");
            cellRes.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);
            p = new Paragraph("Acuerdo");
            cellAcu.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);
            p = new Paragraph("Fecha de término");
            cellFto.SetFontColor(new DeviceRgb(255, 255, 255))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetBackgroundColor(new DeviceRgb(159, 34, 65))
                .Add(p);
            tableAcu.AddCell(cellNum.SetFont(fonte).SetFontSize(8));
            tableAcu.AddCell(cellRes.SetFont(fonte).SetFontSize(8));
            tableAcu.AddCell(cellAcu.SetFont(fonte).SetFontSize(8));
            tableAcu.AddCell(cellFto.SetFont(fonte).SetFontSize(8));

            int i = 0;
            var len = acuerdos.Count();
            foreach (var item in acuerdos)
            {
                i++;
                tableAcu.AddCell(new Paragraph(i.ToString()))
                    .SetFont(fonte)
                    .SetFontSize(8)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                tableAcu.AddCell(new Paragraph(item.Responsable)).SetFont(fonte).SetFontSize(8);
                tableAcu.AddCell(new Paragraph(item.Descripcion)).SetFont(fonte).SetFontSize(8);
                tableAcu.AddCell(new Paragraph(item.FechaTermino)).SetFont(fonte).SetFontSize(8);
            }
            
            doc.Add(tableAcu);
            tableAcu.Complete();
            doc.Close();
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
                PdfFont Bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
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
                
                Color redColor = new DeviceRgb(159, 34, 65);
                SolidLine line = new SolidLine(9f);
                line.SetColor(redColor);
                LineSeparator ls = new LineSeparator(line);
                ls.SetMarginTop(0);
                canvas.Add(ls);
                Paragraph titulo = new Paragraph("Minuta de acuerdos")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontColor(new DeviceRgb(159, 34, 65))
                .SetFixedPosition(40, 800, 150)
                .SetCharacterSpacing(1)
                .SetFont(Bold)
                .SetFontSize(14);
                canvas.Add(titulo);
                Image img = new Image(ImageDataFactory
                .Create(_header))
                .SetMarginTop(10)
                .SetMarginLeft(367)
                .ScaleToFit(190, 170)
                .SetTextAlignment(TextAlignment.CENTER);
                canvas.Add(img);
            }
        }

        private class TableBorderRenderer : TableRenderer
        {
            public TableBorderRenderer(Table modelElement)
                : base(modelElement)
            {
            }

            // If renderer overflows on the next area, iText uses getNextRender() method to create a renderer for the overflow part.
            // If getNextRenderer isn't overriden, the default method will be used and thus a default rather than custom
            // renderer will be created
            public override IRenderer GetNextRenderer()
            {
                return new TableBorderRenderer((Table)modelElement);
            }

            protected override void DrawBorders(DrawContext drawContext)
            {
                Rectangle rect = GetOccupiedAreaBBox();
                Color redColor = new DeviceRgb(152, 56, 39);
                drawContext.GetCanvas()
                    .SaveState()
                    .RoundRectangle(rect.GetLeft(), rect.GetBottom(), rect.GetWidth(), rect.GetHeight(), 8)
                    .SetColor(redColor, false)
                    .Stroke()
                    .RestoreState();
            }
        }
        public IActionResult DownloadFile(int id)
        {
            // Since this is just and example, I am using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            var path = Path.Combine(_environment.WebRootPath, "doc", "Minutas", "minuta_" + id + ".pdf");
            var fs = new FileStream(path, FileMode.Open);

            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", fs.Name);
        }
    }
}
