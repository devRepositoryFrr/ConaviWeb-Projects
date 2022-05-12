using ConaviWeb.Commons;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.RH;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
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
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.RH
{
    public class AsistenciaController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public AsistenciaController( IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View("../RH/Asistencia");
        }
        
        [HttpPost]
        public IActionResult GeneratePDF([FromBody] Asistencia asistencia)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "RH", "result.pdf");
            var iHeader = System.IO.Path.Combine(_environment.WebRootPath, "img", "headerConavi.png");
            var iFooter = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(pathPdf));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));
            doc.SetMargins(80, 40, 70, 40);

            LineSeparator ls = new LineSeparator(new SolidLine());
            Paragraph saltoDeLineaa = new Paragraph("");
            doc.Add(saltoDeLineaa);
            Paragraph subheader = new Paragraph("Hoja de Asistencia por Periodo")
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontColor(DeviceGray.WHITE)
                 .SetBackgroundColor(new DeviceRgb(130, 27, 63))
                 .SetFontSize(12);
            doc.Add(subheader);

            Paragraph Periodo = new Paragraph("Periodo del " + asistencia.Periodo)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(8);
                doc.Add(Periodo);
                doc.Add(ls);
                Paragraph saltoDeLinea = new Paragraph("");
                doc.Add(saltoDeLinea);

                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
                Table nombres = new Table(2, false);
                
                nombres.SetBorder(Border.NO_BORDER);
                Cell cell21 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(font)
                  .SetFontSize(8)
                  .SetBorder(Border.NO_BORDER)
                  .Add(new Paragraph("Nombre: " + user.Name));
                Cell cell22 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(8)
                   .SetBorder(Border.NO_BORDER)
                   .SetFont(font)
                   .Add(new Paragraph(" "));

                Cell cell31 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(font)
                   .SetBorder(Border.NO_BORDER)
                      .SetFontSize(8)
                   .Add(new Paragraph("Puesto: " + user.Cargo));
                Cell cell32 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(font)
                   .SetBorder(Border.NO_BORDER)
                   .SetFontSize(8)
                   .Add(new Paragraph(""));

                Cell cell41 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(font)
                   .SetFontSize(8)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph("Area: " + user.Area));
                Cell cell42 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(font)
                      .SetFontSize(98)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(" "));


                nombres.AddCell(cell21);
                nombres.AddCell(cell22);
                nombres.AddCell(cell31);
                nombres.AddCell(cell32);
                nombres.AddCell(cell41);
                nombres.AddCell(cell42);
                doc.Add(nombres);
                doc.Add(ls);

                Paragraph saltoDeLinea1 = new Paragraph("");
                doc.Add(saltoDeLinea1);

                //CREATE TABLE 
                float[] columnWidths = { 2, 4, 1, 1, 1, 1, 1 };
                Table table = new Table(UnitValue.CreatePercentArray(columnWidths));

                PdfFont f = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                Cell cell = new Cell(1, 7)
                    //.Add(new Paragraph("This is a header"))
                    .SetFont(f)
                    .SetFontSize(5)
                    .SetFontColor(DeviceGray.WHITE)
                    .SetBackgroundColor(new DeviceRgb(130, 27, 63))
                    .SetTextAlignment(TextAlignment.CENTER);
                table.AddHeaderCell(cell);




                for (int i = 0; i < 1; i++)
                {
                    Cell[] headerFooter =
                    {
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Fecha").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Nombre").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Número").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Entrada").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Salida").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Firma").SetFontColor(DeviceGray.WHITE)),
                            new Cell().SetBackgroundColor(new DeviceRgb(130, 27, 63)).SetFontSize(10).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Comentarios").SetFontColor(DeviceGray.WHITE))

                        };

                    foreach (Cell hfCell in headerFooter)
                    {
                        if (i == 0)
                        {

                            table.AddHeaderCell(hfCell);

                        }
                        else
                        {
                            table.AddFooterCell(hfCell);



                        }
                    }
                }

                for (int counter = 0; counter < asistencia.Fechas.Count(); counter++)
                {
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph(asistencia.Fechas[counter].ToString())));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph(user.Name)));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph(user.NuEmpleado)));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph("09:00 AM")));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph("07:00 PM")));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph()));
                    table.AddCell(new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetFontSize(8).Add(new Paragraph()));

                }
                doc.Add(table);

            Paragraph att = new Paragraph("Atentamente")
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFontColor(DeviceGray.WHITE)
                  .SetOpacity(50)
                  .SetBackgroundColor(new DeviceRgb(130, 27, 63))
                  .SetFontSize(10);
            doc.Add(att);



            Paragraph fm = new Paragraph("____________________")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetRelativePosition(-150, 80, 90, 40)
           .SetFontSize(10);
            doc.Add(fm);




            Paragraph jefe = new Paragraph(asistencia.NombreJefe)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetRelativePosition(-150, 70, 90, 40)
            .SetFontSize(8);
            doc.Add(jefe);




            Paragraph fm2 = new Paragraph("____________________")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFixedPosition(100, 200, 200, 700)
            .SetRelativePosition(-100, 34, 1500, 200)
           .SetFontSize(10);
            doc.Add(fm2);

            Paragraph representante = new Paragraph("Recursos Humanos")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFixedPosition(100, 200, 200, 700)
            .SetRelativePosition(-100, 23, 1500, 200)
           .SetFontSize(8);
            doc.Add(representante);



            Paragraph fm3 = new Paragraph("____________________")
          .SetTextAlignment(TextAlignment.CENTER)
            .SetRelativePosition(150, -12, 80, 40)
          .SetFontSize(10);
            doc.Add(fm3);

            Paragraph colaborador = new Paragraph(user.Name)
           .SetTextAlignment(TextAlignment.CENTER)

            .SetRelativePosition(150, -22, 80, 40)
           .SetFontSize(8);
            doc.Add(colaborador);


            doc.Close();
            return RedirectToAction("Index");
            }

            private class TextFooterEventHandler : IEventHandler
            {
                protected Document doc;
                protected string _header;
                protected string _footer;
            public TextFooterEventHandler(Document doc , string iHeader, string iFooter)
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

