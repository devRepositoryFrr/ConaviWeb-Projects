using ConaviWeb.Data.Minuta;
using ConaviWeb.Model;
using ConaviWeb.Model.Minuta;
using ConaviWeb.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;
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
using System.Globalization;
using iText.Kernel.Geom;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConaviWeb.Controllers.Minutas
{
    public class CapturaRTitularController : Controller
    {
        private readonly IMinutaRepository _minutaRepository;
        private readonly IWebHostEnvironment _environment;
        public CapturaRTitularController(IMinutaRepository minutaRepository, IWebHostEnvironment environment)
        {
            _minutaRepository = minutaRepository;
            _environment = environment;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var sector = await _minutaRepository.GetSector();
            var temas = await _minutaRepository.GetTemas();
            var responsable = await _minutaRepository.GetResponsable();
            var gestion = await _minutaRepository.GetGestion();
            var estatus = await _minutaRepository.GetEstatus();
            //_ = ReunionPdfAsync(1);
            ViewData["Sector"] = sector;
            ViewData["Temas"] = temas;
            ViewData["Responsable"] = responsable;
            ViewData["Gestion"] = gestion;
            ViewData["Estatus"] = estatus;
            if (TempData.ContainsKey("Alert"))
                ViewBag.Alert = TempData["Alert"].ToString();
            return View("../Minuta/CapturaRTitular");
        }

        public async Task<IActionResult> CapturaRTitularAsync(ReunionTitular reunion)
        {
            var success = await _minutaRepository.InsertReunion(reunion);
            if (!success)
            {
                TempData["Alert"] = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al guardar la reunión");
                return RedirectToAction("Index");
            }
            TempData["Alert"] = AlertService.ShowAlert(Alerts.Success, "Se guardo la reunión con exito");
            return RedirectToAction("Index");
        }
        [Route("ReunionPdf/{id?}")]
        public async Task ReunionPdfAsync(int id)
        {
            var reunion = await _minutaRepository.GetRTitular(id);
            var fecha = String.Concat(reunion.FchCarga.Day, " de ", reunion.FchCarga.ToString("MMMM", CultureInfo.CreateSpecificCulture("es")), " de ", reunion.FchCarga.Year);
            var _header = System.IO.Path.Combine(_environment.WebRootPath, "img", "sedatu_25.png");
            var _footer = System.IO.Path.Combine(_environment.WebRootPath, "img", "fsedatu25.jpg");
            var pathCarta = System.IO.Path.Combine(_environment.WebRootPath, "doc", "RTitular");
            if (!Directory.Exists(pathCarta))
            {
                Directory.CreateDirectory(pathCarta);
            }
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(System.IO.Path.Combine(pathCarta, "reunion_S00" + id + ".pdf")));
            Document doc = new Document(pdfDoc, PageSize.A4.Rotate());
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, _header, _footer));

            //doc.SetMargins(60, 40, 70, 40); //nuevo margen

            PdfFont cursiva = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont Bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            doc.SetFont(Bold);

            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            Paragraph sL = new Paragraph("");
            doc.Add(sL);
            Paragraph p = new Paragraph();
            p.SetFontSize(7);
            Paragraph titulo = new Paragraph("Ficha de Reunión")
                .SetFontColor(new DeviceRgb(159, 34, 65))
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(300, 500, 200)
                .SetCharacterSpacing(1)
                .SetFont(Bold)
                .SetFontSize(14);
            doc.Add(titulo);

            Paragraph pf = new Paragraph();
            pf.SetFixedPosition(740, 500, 200);
            Text tf = new Text("Folio    ");
            tf.SetFontSize(10).SetFont(Bold);
            pf.Add(tf);
            Text tf1 = new Text("S00" + reunion.Id);
            tf1.SetFontSize(10);
            tf1.SetUnderline();
            pf.Add(tf1);
            doc.Add(pf);

            Paragraph pd = new Paragraph();
            pd.SetFixedPosition(620, 485, 200);
            Text td = new Text("Ciudad de México a ");
            td.SetFontSize(10).SetFont(Bold);
            pd.Add(td);
            Text td1 = new Text(fecha);
            td1.SetFontSize(10);
            td1.SetUnderline();
            pd.Add(td1);
            doc.Add(pd);


            Table table = new Table(new float[4]).UseAllAvailableWidth();
            table.SetMarginTop(70);

            Paragraph p1 = new Paragraph();
            Text t1 = new Text("Tema: ");
            t1.SetFontSize(10).SetFont(Bold);
            p1.Add(t1);
            Text t2 = new Text(reunion.Tema);
            t2.SetFontSize(8);
            t2.SetUnderline();
            p1.Add(t2);
            table.AddCell(new Cell().Add(p1));

            Paragraph p2 = new Paragraph();
            Text t3 = new Text("Responsable: ");
            t3.SetFontSize(10).SetFont(Bold);
            p2.Add(t3);
            Text t4 = new Text(reunion.Responsable);
            t4.SetFontSize(8);
            t4.SetUnderline();
            p2.Add(t4);
            table.AddCell(new Cell().Add(p2));

            Paragraph p3 = new Paragraph();
            Text t5 = new Text("Modalidad: ");
            t5.SetFontSize(10).SetFont(Bold);
            p3.Add(t5);
            Text t6 = new Text(reunion.Modalidad);
            t6.SetFontSize(8);
            t6.SetUnderline();
            p3.Add(t6);
            table.AddCell(new Cell().Add(p3));

            Paragraph p4 = new Paragraph();
            Text t7 = new Text("Liga: ");
            t7.SetFontSize(10).SetFont(Bold);
            p4.Add(t7);
            Text t8 = new Text(reunion.Liga);
            t8.SetFontSize(8);
            t8.SetUnderline();
            p4.Add(t8);
            table.AddCell(new Cell().Add(p4));

            Paragraph p5 = new Paragraph();
            Text t9 = new Text("Asunto: ");
            t9.SetFontSize(10).SetFont(Bold);
            p5.Add(t9);
            Text t10 = new Text(reunion.Asunto);
            t10.SetFontSize(8);
            t10.SetUnderline();
            p5.Add(t10);
            table.AddCell(new Cell(1, 3).Add(p5));

            Paragraph p6 = new Paragraph();
            Text t11 = new Text("Sector: ");
            t11.SetFontSize(10).SetFont(Bold);
            p6.Add(t11);
            Text t12 = new Text(reunion.Sector);
            t12.SetFontSize(8);
            t12.SetUnderline();
            p6.Add(t12);
            table.AddCell(new Cell().Add(p6));

            Paragraph p7 = new Paragraph();
            Text t13 = new Text("Solicitante: ");
            t13.SetFontSize(10).SetFont(Bold);
            p7.Add(t13);
            Text t14 = new Text(reunion.NombreSol);
            t14.SetFontSize(8);
            t14.SetUnderline();
            p7.Add(t14);
            table.AddCell(new Cell().Add(p7));

            Paragraph p8 = new Paragraph();
            Text t15 = new Text("Cargo: ");
            t15.SetFontSize(10).SetFont(Bold);
            p8.Add(t15);
            Text t16 = new Text(reunion.CargoSol);
            t16.SetFontSize(8);
            t16.SetUnderline();
            p8.Add(t16);
            table.AddCell(new Cell().Add(p8));

            Paragraph p9 = new Paragraph();
            Text t17 = new Text("Institución: ");
            t17.SetFontSize(10).SetFont(Bold);
            p9.Add(t17);
            Text t18 = new Text(reunion.InstSol);
            t18.SetFontSize(8);
            t18.SetUnderline();
            p9.Add(t18);
            table.AddCell(new Cell().Add(p9));

            table.AddCell(new Cell().Add(sL));

            Paragraph p10 = new Paragraph();
            Text t19 = new Text("Objetivo de la reunión: ");
            t19.SetFontSize(10).SetFont(Bold);
            p10.Add(t19);
            Text t20 = new Text(reunion.Objetivo);
            t20.SetFontSize(8);
            t20.SetUnderline();
            p10.Add(t20);
            table.AddCell(new Cell(1, 4).Add(p10));

            RemoveBorder(table);
            doc.Add(table);

            Table table1 = new Table(new float[4]).UseAllAvailableWidth();
            table1.SetMarginBottom(5);

            Paragraph p11 = new Paragraph();
            Text t21 = new Text("Lugar: ");
            t21.SetFontSize(10).SetFont(Bold);
            p11.Add(t21);
            Text t22 = new Text(reunion.Lugar);
            t22.SetFontSize(8);
            t22.SetUnderline();
            p11.Add(t22);
            table1.AddCell(new Cell().Add(p11));

            Paragraph p12 = new Paragraph();
            Text t23 = new Text("Sala: ");
            t23.SetFontSize(10).SetFont(Bold);
            p12.Add(t23);
            Text t24 = new Text(reunion.Sala);
            t24.SetFontSize(8);
            t24.SetUnderline();
            p12.Add(t24);
            table1.AddCell(new Cell().Add(p12));

            Paragraph p13 = new Paragraph();
            Text t25 = new Text("Tiempo estimado: ");
            t25.SetFontSize(10).SetFont(Bold);
            p13.Add(t25);
            Text t26 = new Text(reunion.Tiempo);
            t26.SetFontSize(8);
            t26.SetUnderline();
            p13.Add(t26);
            table1.AddCell(new Cell().Add(p13));

            Paragraph p14 = new Paragraph();
            Text t27 = new Text("¿Se permiten celulares? ");
            t27.SetFontSize(10).SetFont(Bold);
            p14.Add(t27);
            Text t28 = new Text(reunion.RadioCel);
            t28.SetFontSize(8);
            t28.SetUnderline();
            p14.Add(t28);
            table1.AddCell(new Cell().Add(p14));

            RemoveBorder(table1);
            doc.Add(table1);


            Table tablePart = new Table(new float[2]).UseAllAvailableWidth();
            tablePart.SetMarginBottom(5);

            Paragraph tituloP = new Paragraph("Participantes")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetCharacterSpacing(1)
                .SetFont(Bold)
                .SetFontSize(10);
            doc.Add(tituloP);
            Table tableP = new Table(new float[1]).SetWidth(UnitValue.CreatePercentValue(100));
            tableP.SetMarginBottom(5);
            tableP.AddCell(new Cell().Add(new Paragraph("SEDATU").SetFont(Bold)
                        .SetFontSize(10)))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            var json = JsonConvert.DeserializeObject<List<PSedatu>>(reunion.PSedatu);
            //dynamic json = JsonConvert.DeserializeObject(reunion.PSedatu);
            if (json.Any()) {
                foreach (var v in json)
                {

                    tableP.AddCell(new Paragraph(v.num + ") " + v.name))
                        .SetFont(fonte)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                }
            }
            //doc.Add(tableP);
            Table tablePE = new Table(new float[1]).SetWidth(UnitValue.CreatePercentValue(100));
            tablePE.SetMarginBottom(5);
            //tablePE.SetFixedPosition(430, 274, 375);
            tablePE.AddCell(new Cell().Add(new Paragraph("Externos").SetFont(Bold)
                        .SetFontSize(10)))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            var jsonE = JsonConvert.DeserializeObject<List<PSedatu>>(reunion.PExterno);
            //dynamic json = JsonConvert.DeserializeObject(reunion.PSedatu);
            if (jsonE.Any())
            {
                foreach (var v in jsonE)
                {

                    tablePE.AddCell(new Paragraph(v.num + ") " + v.name))
                        .SetFont(fonte)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);


                }
            }
            //doc.Add(tablePE);

            tablePart.AddCell(tableP);
            tablePart.AddCell(tablePE);
            RemoveBorder(tablePart);
            doc.Add(tablePart);

            Paragraph p15 = new Paragraph();
            Text t29 = new Text("¿Se requieren accesos directos?    ");
            t29.SetFontSize(10).SetFont(Bold);
            p15.Add(t29);
            Text t30 = new Text(reunion.RadioAD);
            t30.SetFontSize(8);
            t30.SetUnderline();
            p15.Add(t30);
            doc.Add(p15);
            if (reunion.RadioAD != "NO") { 
            Table tableAD = new Table(new float[2]).SetWidth(UnitValue.CreatePercentValue(100));
            tableAD.SetMarginBottom(5);
            tableAD.AddCell(new Cell().Add(new Paragraph("Nombre").SetFontSize(10)
                        .SetFont(Bold)))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
            tableAD.AddCell(new Cell().Add(new Paragraph("Contacto").SetFontSize(10)
                        .SetFont(Bold)))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

            var jsonAD = JsonConvert.DeserializeObject<List<AccesoD>>(reunion.AccesoD);
            //dynamic json = JsonConvert.DeserializeObject(reunion.PSedatu);
            if (jsonAD.Any()) {
                foreach (var v in jsonAD)
                {

                    tableAD.AddCell(new Paragraph(v.num + ") " + v.name))
                        .SetFont(fonte)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    tableAD.AddCell(new Paragraph(v.contacto))
                        .SetFont(fonte)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                }
            }
            doc.Add(tableAD);
        }
            Paragraph tituloOD = new Paragraph("Orden del día:")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetCharacterSpacing(1)
                .SetFont(Bold)
                .SetFontSize(10);
            doc.Add(tituloOD);
            
            Table tableOD = new Table(new float[1]).SetWidth(UnitValue.CreatePercentValue(100));
            tableOD.SetMarginBottom(5);

            var jsonOD = JsonConvert.DeserializeObject<List<Orden>>(reunion.Orden);
            //dynamic json = JsonConvert.DeserializeObject(reunion.PSedatu);
            if (jsonOD.Any())
            {
                foreach (var v in jsonOD)
                {

                    tableOD.AddCell(new Paragraph(v.num + ") " + v.orden))
                        .SetFont(fonte)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                }
            }
            doc.Add(tableOD);
            

            Paragraph p16 = new Paragraph();
            Text t31 = new Text("Insumos necesarios:    ");
            t31.SetFontSize(10).SetFont(Bold);
            p16.Add(t31);
            Text t32 = new Text(reunion.Insumos);
            t32.SetFontSize(8);
            t32.SetUnderline();
            p16.Add(t32);
            doc.Add(p16);

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

                //Color redColor = new DeviceRgb(159, 34, 65);
                //SolidLine line = new SolidLine(9f);
                //line.SetColor(redColor);
                //LineSeparator ls = new LineSeparator(line);
                //ls.SetMarginTop(0);
                //canvas.Add(ls);
                //Paragraph titulo = new Paragraph("Ficha de Reunión")
                //.SetTextAlignment(TextAlignment.CENTER)
                //.SetFontColor(new DeviceRgb(159, 34, 65))
                //.SetFixedPosition(40, 800, 150)
                //.SetCharacterSpacing(1)
                //.SetFont(Bold)
                //.SetFontSize(14);
                //canvas.Add(titulo);
                Image img = new Image(ImageDataFactory
                .Create(_header))
                .SetMarginTop(5)
                .SetMarginLeft(40)
                .ScaleToFit(770, 200);
                canvas.Add(img);
                Image img2 = new Image(ImageDataFactory
                .Create(_footer))
                .SetMarginTop(480)
                .SetMarginLeft(36)
                .ScaleToFit(770, 200);
                canvas.Add(img2);
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
        [Route("DownRT/{id?}")]
        public async Task<IActionResult> DownRTAsync(int id)
        {
            // Since this is just and example, I am using a local file located inside wwwroot
            // Afterwards file is converted into a stream
            var path = System.IO.Path.Combine(_environment.WebRootPath, "doc", "RTitular", "reunion_S00" + id + ".pdf");
            if (!System.IO.File.Exists(path))
            {
                await ReunionPdfAsync(id);
                //return RedirectToAction("Index", "ListaAcuerdos");
            }
            var fs = new FileStream(path, FileMode.Open);
            // Return the file. A byte array can also be used instead of a stream
            return File(fs, "application/octet-stream", "reunion_S00" + id + ".pdf");
        }
        private class PSedatu
        {
            public string num { get; set; }
            public string name { get; set; }
        }
        private class AccesoD
        {
            public string num { get; set; }
            public string name { get; set; }
            public string contacto { get; set; }
        }
        private class Orden
        {
        public string num { get; set; }
        public string orden { get; set; }
        }
        private static void RemoveBorder(Table table)
        {
            foreach (var  iElement in  table.GetChildren())
            {
                ((Cell)iElement).SetBorder(Border.NO_BORDER);
            }
        }
    }
}
