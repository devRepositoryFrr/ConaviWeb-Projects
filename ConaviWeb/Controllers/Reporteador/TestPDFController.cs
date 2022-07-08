using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using Document = iText.Layout.Document;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfFont = iText.Kernel.Font.PdfFont;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace ConaviWeb.Controllers.Reporteador
{
    public class TestPDFController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public TestPDFController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public IActionResult Index()
        {
            var pathTest = Path.Combine(_environment.WebRootPath, "doc", "test.pdf");
            var result = Path.Combine(_environment.WebRootPath, "doc", "test2.pdf");
            ManipulatePdf(pathTest);
            return View();
        }

        protected void ManipulatePdf(String pathTest)
        {
            var iHeader = System.IO.Path.Combine(_environment.WebRootPath, "img", "headerConavi.png");
            var iFooter = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            var nuFirma = 1;
            var nufirma2 = nuFirma - 1;
            PdfFont font_title = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            PdfFont font_content = PdfFontFactory.CreateFont(StandardFonts.COURIER);
            //Crear documento

            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(pathTest));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));


            //Editar documento

            //PdfReader reader = new PdfReader(Path.Combine(_environment.WebRootPath, "doc", "test" + nufirma2 + ".pdf"));
            //PdfDocument pdfDoc = new PdfDocument(reader, new PdfWriter(Path.Combine(_environment.WebRootPath, "doc", "test"+ nuFirma + ".pdf")));
            //Document doc = new Document(pdfDoc);
            //pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));




            doc.SetMargins(60, 40, 70, 40); //nuevo margen
            PdfFont cursiva = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLDOBLIQUE);
            

            float[] columnWidths = { 7, 7 };
            
            if (nuFirma == 1)
            {
                Table encabezado = new Table(UnitValue.CreatePercentArray(columnWidths));
                encabezado.SetWidth(500);
                encabezado.SetMarginRight(0);
                encabezado.SetRelativePosition(0, 1, 0, 40);
                Cell comite = new Cell(1, 4)
                .Add(new Paragraph("COMITÉ DE FINANCIAMIENTO DE LA COMISIÓN NACIONAL DE VIVIENDA"))
                .SetFontSize(9)
                .SetFont(cursiva)
                .SetHeight(12)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                encabezado.AddHeaderCell(comite);
                doc.Add(encabezado);


            //***Aqui Inicia la tabla del director General***//
            Table directora = new Table(UnitValue.CreatePercentArray(columnWidths));
            directora.SetMaxWidth(500);
            directora.SetMarginRight(0);
            directora.SetFixedPosition(1, 40, 595, 500);
            Cell conavi = new Cell(1, 4)
            .Add(new Paragraph("CONAVI"))
            .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            .SetFontSize(9)
            .SetFont(cursiva)
            .SetHeight(12)
            //.SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            encabezado.AddCell(conavi);
            Cell dir = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(cursiva)
            .SetHeight(15)
            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
            .SetFontSize(8)
            .Add(new Paragraph("Directora General"));
            Cell program = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(cursiva)
            .SetHeight(23)
            .SetFontSize(8)
            .Add(new Paragraph("Dra. Edna Elena Vega Rangel"))
            .Add(new Paragraph("Presidenta"));
            Cell cFirma = new Cell(1, 4)
            .Add(new Paragraph(""))
            .SetFontSize(9)
            .SetHeight(120)
            //.SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            directora.AddCell(conavi);
            directora.AddCell(dir);
            directora.AddCell(program);
            directora.AddCell(cFirma);
                doc.Add(directora);
                //Firma -----------------------------------------------|-----------
                Table firma = new Table(4, true);
                firma.SetBorder(Border.NO_BORDER);
                firma.SetMaxWidth(480);
                firma.SetFixedPosition(1, 55, 600, 440);

                Cell hCadenaOriginal = new Cell(1, 4)
                      .SetTextAlignment(TextAlignment.LEFT)
                      .SetFont(font_title)
                      .SetFontSize(7)
                      .SetHeight(10)
                      .SetWidth(10)
                      //.SetBorder(Border.NO_BORDER)
                      .SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.1f))
                      .SetBorderTop(new SolidBorder(ColorConstants.BLACK, 0.1f))
                      .SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.1f))
                      .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                      .Add(new Paragraph("Cadena Original"));
                Cell cadenaOriginal = new Cell(1, 3)
                    //.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.1f))
                    //.SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.1f))
                    .SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("||Firma|Firma Electrónica|2305221852058343|FInterno PFI SFI||RORF8910042A5|0|CONAVI|2022-05-23T18:55:53-05:00|44-1F-52-6F-1D-8C-A0-02-2E-C2-6A-63-38-21-2F-33-4D-56-C3-4C-B4-B8-60-0F-51-2D-89-38-21-63-C3-08-75-D9-99-09-1C-BB-E6-0F-82-87-08-AB-93-CC-48-12-59-98-8C-8F-62-5D-1B-5C-3F-89-9E-20-F5-9A-2E-19|00001000000500619765||"))//cadena original
                    .SetFont(font_content)
                    .SetFontSize(6)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetHeight(40)
                    .SetWidth(20);

                Cell hFirmaEConavi = new Cell(1, 4)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(font_title)
                 .SetFontSize(7)
                 //.SetBorder(Border.NO_BORDER)
                 .SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.1f))
                 .SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.1f))
                 .SetHeight(10)
                 .SetWidth(10)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .Add(new Paragraph("Firma electrónica "));
                Cell firmaEConavi = new Cell(1, 3)
                    .SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.1f))
                    .SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.1f))
                    //.SetBorder(Border.NO_BORDER)
                    .Add(new Paragraph("hZmqI0im6zzfK/ek6caUZx+gFT0UfqAcpQXfUDN8pFIpRmLXrP2vXGYUeC1UlNl2uRdTFLQWGFDg/sCLfu0/wxlLXm8ODZ7wS+aTWLzR0YkmjJITd9yaZm8r8cGAn4KsfU/b4xA8trP+K4BUgaBTQAO+8Ba/Se7vKE0EmTLuJ8huw3c9D1cdejVpqTGNNrQeY01Un+sF8Nj8az4WLRU1xISxPhoBVfZ3uNbG92Kc8sSqRIwoCtC9AgbR99e5U0dRNhyNMmce4NSrB7XqvCzh/Au7S7rvBuV2QxxdwR69ov6Adf+Lim4OLylnqVijgGkZKb3JPHGp1hp5zmaX2MZMcA=="))//Sello
                    .SetFont(font_content)
                    .SetFontSize(6)
                    .SetTextAlignment(TextAlignment.JUSTIFIED)
                    .SetHeight(40);
                // Upload image
                ImageData imageData = ImageDataFactory.Create(Path.Combine(_environment.WebRootPath, "img", "QR.jpg"));
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(40, 40);
                Cell imagenqr = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
               //.SetBorder(Border.NO_BORDER)
               .SetMarginLeft(40)
               .SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 0.1f))
              .SetBorderRight(new SolidBorder(ColorConstants.BLACK, 0.1f))
               .SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 0.1f))
               .SetHeight(40)
               .Add(image);

                firma.AddCell(hCadenaOriginal);
                firma.AddCell(cadenaOriginal);
                firma.AddCell(new Cell(1, 4).SetBorder(Border.NO_BORDER));
                firma.AddCell(hFirmaEConavi);
                firma.AddCell(firmaEConavi);
                firma.AddCell(imagenqr);
                doc.Add(firma);

            

        }

            if (nuFirma == 2)
            {

            
            //**TABLA DE SUBDIRECTOR GENERAL**//
            Table subDirector = new Table(UnitValue.CreatePercentArray(columnWidths));
            subDirector.SetWidth(500);
            subDirector.SetMarginRight(0);
            subDirector.SetFixedPosition(1, 40, 443, 500);
            Cell sub = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(cursiva)
            .SetHeight(15)
            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
            .SetFontSize(8)
            .Add(new Paragraph("Subdirector General de Administración y Financiamiento"));
            Cell silva = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(cursiva)
            .SetHeight(23)
            .SetFontSize(8)
            .Add(new Paragraph("Mtro. Alonso Cacho Silva"))
            .Add(new Paragraph("Secretario Ejecutivo"));
            Cell firmaSilva = new Cell(1, 4)
            .Add(new Paragraph(""))
            .SetFontSize(9)
            .SetHeight(120)
            //.SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            subDirector.AddCell(sub);
            subDirector.AddCell(silva);
            subDirector.AddCell(firmaSilva);
            doc.Add(subDirector);

            }

            if (nuFirma == 3)
            {
                //**AQUI EMPIEZA LA TABLA DE SEDATU**//
                Table sedatu = new Table(UnitValue.CreatePercentArray(columnWidths));
                sedatu.SetWidth(500);
                sedatu.SetMarginRight(0);
                sedatu.SetFixedPosition(1, 40, 291, 500);
                Cell SEDATU = new Cell(1, 4)
                .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                .Add(new Paragraph("SEDATU"))
                .SetFontSize(9)
                .SetFont(cursiva)
                .SetHeight(12)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                Cell secretariosedatu = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(15)
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Subsecretario de Ordenamiento Territorial y Agrario"));
                Cell Cervantes = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Arq. David Ricardo Cervantes Peredo"))
                .Add(new Paragraph("Vocal Suplente"));
                Cell firma2 = new Cell(1, 4)
                .Add(new Paragraph(""))
                .SetFontSize(9)
                .SetHeight(120)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                sedatu.AddCell(SEDATU);
                sedatu.AddCell(secretariosedatu);
                sedatu.AddCell(Cervantes);
                sedatu.AddCell(firma2);
                //encabezado.AddCell(sedatu);
                doc.Add(sedatu);

            }

            if (nuFirma == 4)
            {
                //**  SHCP  **//
                Table secrecp = new Table(UnitValue.CreatePercentArray(columnWidths));
                secrecp.SetWidth(500);
                secrecp.SetMarginRight(0);
                secrecp.SetFixedPosition(1, 40, 139, 500);
                Cell SHCP = new Cell(1, 4)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .Add(new Paragraph("SHCP"))
                     .SetFontSize(9)
                     .SetFont(cursiva)
                     .SetHeight(12)
                     //.SetBorder(Border.NO_BORDER)
                     .SetFontColor(DeviceGray.BLACK)
                     .SetTextAlignment(TextAlignment.CENTER);
                Cell Titular = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                    .SetHeight(15)
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetFontSize(5)
                    .Add(new Paragraph("Titular de la Unidad de Seguros, Pensiones y Seguridad Social de la Secretaría de Hacienda y Crédito Público"));
                Cell Santana = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                    .SetHeight(23)
                    .SetFontSize(8)
                    .Add(new Paragraph("Mtro. Héctor Santana Suárez"))
                    .Add(new Paragraph("Vocal"));
                Cell firma3 = new Cell(1, 4)
                    .Add(new Paragraph(""))
                    .SetFontSize(9)
                    .SetHeight(120)
                    //.SetBorder(Border.NO_BORDER)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetTextAlignment(TextAlignment.CENTER);

                secrecp.AddCell(SHCP);
                secrecp.AddCell(Titular);
                secrecp.AddCell(Santana);
                secrecp.AddCell(firma3);
                //encabezado.AddCell(secrecp);
                doc.Add(secrecp);

            }

            if (nuFirma == 5)
            {

                //doc.Add(new AreaBreak(AreaBreakType.NEXT_AREA));
                Table table2 = new Table(UnitValue.CreatePercentArray(columnWidths));
                table2.SetWidth(500);
                table2.SetMarginRight(0);
                table2.SetFixedPosition(2, 40, 595, 500);
                Cell INSUS = new Cell(1, 4)
               .Add(new Paragraph("INSUS"))
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetFontSize(9)
               .SetHeight(12)
               .SetFont(cursiva)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                Cell Sustentable = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(cursiva)
                .SetHeight(15)
                //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Director General del Instituto Nacional de Suelo Sustentable"));
                Cell Iracheta = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Dr. Alfonso Iracheta Carroll"))
                .Add(new Paragraph("Vocal"));
                Cell firma4 = new Cell(1, 4)
                 .Add(new Paragraph(""))
                 .SetFontSize(9)
                 .SetHeight(120)
                 //.SetBorder(Border.NO_BORDER)
                 .SetFontColor(DeviceGray.BLACK)
                 .SetTextAlignment(TextAlignment.CENTER);
                table2.AddCell(INSUS);
                table2.AddCell(Sustentable);
                table2.AddCell(Iracheta);
                table2.AddCell(firma4);
                doc.Add(table2);
            }

            if (nuFirma == 6)
            {
                Table cnvi = new Table(UnitValue.CreatePercentArray(columnWidths));
                cnvi.SetWidth(500);
                cnvi.SetMarginRight(0);
                cnvi.SetFixedPosition(2, 40, 443, 500);
                Cell CONAVI = new Cell(1, 4)
               .Add(new Paragraph("CONAVI"))
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetFontSize(9)
               .SetFont(cursiva)
               .SetHeight(12)
               //.SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                Cell SubdirectoraG = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(15)
                //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Subdirectora General de Análisis de Vivienda, Prospectiva y Sustentabilidad"));
                Cell Silvia = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                 .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Lic. Silvia Circe Díaz Duarte"))
                .Add(new Paragraph("Vocal"));
                Cell firma5 = new Cell(1, 4)
                .Add(new Paragraph(""))
                .SetFontSize(9)
                .SetHeight(120)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                cnvi.AddCell(CONAVI);
                cnvi.AddCell(SubdirectoraG);
                cnvi.AddCell(Silvia);
                cnvi.AddCell(firma5);
                doc.Add(cnvi);

            }

            if (nuFirma == 7)
            {
                Table mayc = new Table(UnitValue.CreatePercentArray(columnWidths));
                mayc.SetWidth(500);
                mayc.SetMarginRight(0);
                mayc.SetFixedPosition(2, 40, 291, 500);
                Cell SubdirectorG = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(cursiva)
                .SetHeight(15)
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Subdirector General de Asuntos Jurídicos y Secretariado Técnico"));
                Cell May = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Lic. David May Flores"))
                .Add(new Paragraph("Vocal"));
                Cell firma6 = new Cell(1, 4)
                .Add(new Paragraph(""))
                .SetFontSize(9)
                .SetHeight(120)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);

                mayc.AddCell(SubdirectorG);
                mayc.AddCell(May);
                mayc.AddCell(firma6);
                doc.Add(mayc);


            }

            if (nuFirma == 8)
            {
                Table mac = new Table(UnitValue.CreatePercentArray(columnWidths));
                mac.SetWidth(500);
                mac.SetMarginRight(0);
                mac.SetFixedPosition(2, 40, 139, 500);
                Cell Coordinación = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                   .SetFont(cursiva)
                  .SetHeight(15)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetFontSize(8)
                  .Add(new Paragraph("Coordinación General de Administración"));
                Cell Maciel = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                    .SetHeight(23)
                    .SetFontSize(8)
                    .Add(new Paragraph("Lic. José Luis Maciel Hernández"))
                    .Add(new Paragraph("Vocal"));
                Cell firma7 = new Cell(1, 4)
                .Add(new Paragraph(""))
                .SetFontSize(9)
                .SetHeight(120)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                mac.AddCell(Coordinación);
                mac.AddCell(Maciel);
                mac.AddCell(firma7);
                doc.Add(mac);
            }

            if (nuFirma == 9)
            {
                //doc.Add(new AreaBreak(AreaBreakType.NEXT_AREA));
                Table table3 = new Table(UnitValue.CreatePercentArray(columnWidths));
                table3.SetWidth(500);
                table3.SetMarginRight(0);
                table3.SetFixedPosition(3, 40, 595, 500);
                Cell INSUS1 = new Cell(1, 4)
                    .Add(new Paragraph("COMITÉ DE FINANCIAMIENTO DE LA COMISIÓN NACIONAL DE VIVIENDA"))
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER);
                Cell ProgramaciónyP = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(cursiva)
                    .SetHeight(15)
                    //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetFontSize(8)
                    .Add(new Paragraph("Director de Programación y Presupuesto"));
                Cell Gordillo = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFont(cursiva)
                   .SetHeight(23)
                   .SetFontSize(8)
                   .Add(new Paragraph("Lic. Francisco Javier Gordillo Paniagua"))
                   .Add(new Paragraph("Invitado Permanente"));
                Cell firma41 = new Cell(1, 4)
                    .Add(new Paragraph(""))
                    .SetFontSize(9)
                    .SetHeight(120)
                    //.SetBorder(Border.NO_BORDER)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetTextAlignment(TextAlignment.CENTER);
                table3.AddCell(INSUS1);
                table3.AddCell(ProgramaciónyP);
                table3.AddCell(Gordillo);
                table3.AddCell(firma41);
                doc.Add(table3);

            }

            if (nuFirma == 10)
            {

                Table t4 = new Table(UnitValue.CreatePercentArray(columnWidths));
                t4.SetWidth(500);
                t4.SetMarginRight(0);
                t4.SetFixedPosition(3, 40, 443, 500);
                Cell OperaciónyS = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(cursiva)
                    .SetHeight(15)
                    //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetFontSize(8)
                    .Add(new Paragraph("Subdirector General de Operación y Seguimiento"));
                Cell Granados = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                     .SetHeight(23)
                    .SetFontSize(8)
                    .Add(new Paragraph("Arq. Juan Javier Granados Barrón"))
                    .Add(new Paragraph("Invitado Permanente"));
                Cell firma51 = new Cell(1, 4)
                    .Add(new Paragraph(""))
                    .SetFontSize(9)
                    .SetHeight(120)
                    //.SetBorder(Border.NO_BORDER)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetTextAlignment(TextAlignment.CENTER);
                t4.AddCell(OperaciónyS);
                t4.AddCell(Granados);
                t4.AddCell(firma51);
                doc.Add(t4);


            }

            if (nuFirma == 11)
            {

                Table t5 = new Table(UnitValue.CreatePercentArray(columnWidths));
                t5.SetWidth(500);
                t5.SetMarginRight(0);
                t5.SetFixedPosition(3, 40, 291, 500);
                Cell Cofinanciamiento = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                    .SetHeight(15)
                    //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetFontSize(8)
                    .Add(new Paragraph("Director de Cofinanciamiento"));
                Cell Urtaza = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                     .SetHeight(23)
                    .SetFontSize(8)
                    .Add(new Paragraph("Mtro. Alejandro Lenin Sierra Urtaza"))
                    .Add(new Paragraph("Invitado "));
                Cell firma61 = new Cell(1, 4)
                    .Add(new Paragraph(""))
                    .SetFontSize(9)
                    .SetHeight(120)
                    //.SetBorder(Border.NO_BORDER)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetTextAlignment(TextAlignment.CENTER);
                t5.AddCell(Cofinanciamiento);
                t5.AddCell(Urtaza);
                t5.AddCell(firma61);
                doc.Add(t5);

            }

            if (nuFirma == 12)
            {

                Table t6 = new Table(UnitValue.CreatePercentArray(columnWidths));
                t6.SetWidth(500);
                t6.SetMarginRight(0);
                t6.SetFixedPosition(3, 40, 139, 500);
                Cell Financieros = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(cursiva)
                     .SetHeight(15)
                     //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                     .SetFontSize(8)
                     .Add(new Paragraph("Director de Esquemas Financieros"));
                Cell Avellaneda = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(cursiva)
                     .SetHeight(23)
                    .SetFontSize(8)
                    .Add(new Paragraph("Lic. David Avellaneda Agüero"))
                    .Add(new Paragraph("Invitado"));
                Cell firma71 = new Cell(1, 4)
                    .Add(new Paragraph(""))
                    .SetFontSize(9)
                    .SetHeight(120)
                    //.SetBorder(Border.NO_BORDER)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetTextAlignment(TextAlignment.CENTER);
                t6.AddCell(Financieros);
                t6.AddCell(Avellaneda);
                t6.AddCell(firma71);
                doc.Add(t6);

            }

            if (nuFirma == 13)
            {
                //doc.Add(new AreaBreak(AreaBreakType.NEXT_AREA));
                Table t7 = new Table(UnitValue.CreatePercentArray(columnWidths));
                t7.SetWidth(500);
                t7.SetMarginRight(0);
                t7.SetFixedPosition(4, 40, 595, 500);
                Cell INSUS1 = new Cell(1, 4)
                    .Add(new Paragraph("COMITÉ DE FINANCIAMIENTO DE LA COMISIÓN NACIONAL DE VIVIENDA"))
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetTextAlignment(TextAlignment.CENTER);
                Cell SFP = new Cell(1, 4)
               .Add(new Paragraph("SFP"))
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetFontSize(9)
               .SetHeight(12)
                  .SetFont(cursiva)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                Cell Comisario = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(cursiva)
                .SetHeight(15)
                //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Subdelegado y Comisario Público Suplente"));
                Cell Tovar = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Mtro. Carlos Tovar Vázquez"))
                .Add(new Paragraph("Suplente del Invitado Permanente"));
                Cell firmaspf = new Cell(1, 4)
                  .Add(new Paragraph(""))
                  .SetFontSize(9)
                  .SetHeight(120)
                  //.SetBorder(Border.NO_BORDER)
                  .SetFontColor(DeviceGray.BLACK)
                  .SetTextAlignment(TextAlignment.CENTER);
                t7.AddCell(INSUS1);
                t7.AddCell(SFP);
                t7.AddCell(Comisario);
                t7.AddCell(Tovar);
                t7.AddCell(firmaspf);
                doc.Add(t7);

            }


            if (nuFirma == 14)
            {
                Table ta8 = new Table(UnitValue.CreatePercentArray(columnWidths));
                ta8.SetWidth(500);
                ta8.SetMarginRight(0);
                ta8.SetFixedPosition(4, 40, 443, 500);
                Cell OIC = new Cell(1, 4)
               .Add(new Paragraph("OIC"))
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetFontSize(9)
               .SetFont(cursiva)
               .SetHeight(12)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
                Cell Control = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                .SetHeight(15)
                //.SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(8)
                .Add(new Paragraph("Titular del Órgano Interno de Control"));
                Cell Lozano = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(cursiva)
                 .SetHeight(23)
                .SetFontSize(8)
                .Add(new Paragraph("Mtra. Vianey Fabiola Lozano Rangel"))
                .Add(new Paragraph("Invitada Permanente"));

                Cell firmacontrol = new Cell(1, 4)
                .Add(new Paragraph(""))
                .SetFontSize(9)
                .SetHeight(120)
                //.SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);


                ta8.AddCell(OIC);
                ta8.AddCell(Control);
                ta8.AddCell(Lozano);
                ta8.AddCell(firmacontrol);
                doc.Add(ta8);


            }


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
