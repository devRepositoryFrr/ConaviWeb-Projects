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
using System.Threading.Tasks;

namespace ConaviWeb.Controllers.Reporteador
{
    public class PevC2srController : Controller
    {
        private readonly IReporteadorRepository _reporteadorRepository;
        private readonly IWebHostEnvironment _environment;
        public PevC2srController(IWebHostEnvironment environment, IReporteadorRepository reporteadorRepository)
        {
            _reporteadorRepository = reporteadorRepository;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public async Task<IActionResult> GetAllBenef()
        {
            var beneficiario = await _reporteadorRepository.GetBeneficiario("AECD610914HTCNRL05");
            //foreach (var beneficiario in beneficiarios)
            //{
                GenerateSavePDFAsync(beneficiario);
            //}
            return Ok();
        }
        public void GenerateSavePDFAsync(PevC2sr beneficiario)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var fileName = beneficiario.CURPR + ".pdf";
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "Reporteador", beneficiario.CURPR);
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

        //public async Task<IActionResult> GenerateStreamPDFAsync(int id)
        //{

        //    var viaticos = await _reporteadorRepository.GetBeneficiario(id);
        //    var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
        //    var fileName = beneficiario.Folio + ".pdf";
        //    var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "RH", id.ToString());
        //    if (!Directory.Exists(pathPdf))
        //        ProccessFileTools.CreateDirectory(pathPdf);
        //    var file = System.IO.Path.Combine(pathPdf, fileName);
        //    var iHeader = System.IO.Path.Combine(_environment.WebRootPath, "img", "headerConavi.png");
        //    var iFooter = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
        //    MemoryStream ms = new MemoryStream();
        //    PdfWriter writer = new PdfWriter(ms);
        //    PdfDocument pdfDoc = new PdfDocument(writer);
        //    Document doc = new Document(pdfDoc);
        //    writer.SetCloseStream(false);

        //    pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));
        //    //MARGEN DEL DOCUMENTO
        //    doc.SetMargins(70, 50, 70, 50);
        //    //LOGICA PDF
        //    GetPDF(doc, viaticos);
        //    doc.Close();

        //    byte[] byteInfo = ms.ToArray();
        //    ms.Write(byteInfo, 0, byteInfo.Length);
        //    ms.Position = 0;
        //    FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

        //    //Uncomment this to return the file as a download
        //    //fileStreamResult.FileDownloadName = "Output.pdf";

        //    return fileStreamResult;
        //}

        public Document GetPDF(Document doc, PevC2sr beneficiario)
        {
            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fonts = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            //TITULO DEL DOCUMENTO

            Paragraph titulo = new Paragraph("VISITA DE CONFORMACIÓN DEL APOYO ")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetCharacterSpacing(1)
                .SetFont(fonts)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetFontSize(12);
            doc.Add(titulo);

            Paragraph titulo2 = new Paragraph("CUESTIONARIO 2")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetCharacterSpacing(1)
               .SetFont(fonts)
               .SetFontSize(10);
            doc.Add(titulo2);

            Paragraph titulo3 = new Paragraph("(SIN PREVIO REGISTRO)")
               .SetTextAlignment(TextAlignment.CENTER)
               .SetCharacterSpacing(1)
               .SetFont(fonts)
               .SetFontSize(9);
            doc.Add(titulo3);


            //INICIO DEL BLOQUE A
            //TablaBloque1  
            float[] tama = { 10, 10, 10, 10 };
            Table bloque1 = new Table(UnitValue.CreatePercentArray(tama));
            bloque1.SetRelativePosition(5, 15, 50, 40);
            Cell vehiculo = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE A ''INFORMACIÓN DE LA PERSONA SOLICITANTE''"))
            .SetFont(fonts)

            .SetFontSize(10)
           .SetBorder(Border.NO_BORDER)
           .SetFontColor(DeviceGray.BLACK)
           .SetTextAlignment(TextAlignment.CENTER);
            bloque1.AddHeaderCell(vehiculo);

            Cell nombre = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph(beneficiario.Nombre));

            Cell curp = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("CURP de la persona solicitante"));

            Cell entidad = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Entidad"));


            Cell municipio = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Municipio"));

            //Campos de base de datos
            Cell nombretxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Nombre +" "+ beneficiario.Primer_apellido + " " + beneficiario.Segundo_apellido));

            Cell curpntxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.CURPR));

            Cell entidadtxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Nombre_estado));

            Cell municipiotxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Nombre_municipio));




            Cell direccion = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Dirección (Calle, número, colonia)"));

            Cell cp = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Código Postal"));

            Cell comprobante = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("La persona solicitante, ¿cuenta con comprobante de la propiedad?"));


            Cell viviendacom = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("La persona solicitante, ¿es la propietaria de la vivienda? "));

            //Campos de base de datos
            Cell direcciontxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Direccion));

            Cell cptxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Cp));

            Cell comprobantetxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.C_cpropiedad));

            Cell viviendacomtxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Beneficiario_propietario));




            Cell autorizacion = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonts)
           .SetWidth(10)
           .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(8)
           .Add(new Paragraph("¿Cuenta con la autorización del propietario para la realización de los trabajos? "));

            Cell visitada = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿La vivienda visitada es?"));

            Cell medios = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Medios de contacto"));

            Cell vacio = new Cell(1, 1)
             .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph(""));

            //Campos de base de datos
            Cell autorizaciontxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Autorizacion_trabajos));

            Cell visitadatxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Desc_vivienda));

            Cell mediostxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Telefono + " | " + beneficiario.Alternativo));
            bloque1.AddCell(nombre);
            bloque1.AddCell(curp);
            bloque1.AddCell(entidad);
            bloque1.AddCell(municipio);
            bloque1.AddCell(nombretxt);
            bloque1.AddCell(curpntxt);
            bloque1.AddCell(entidadtxt);
            bloque1.AddCell(municipiotxt);
            bloque1.AddCell(direccion);
            bloque1.AddCell(cp);
            bloque1.AddCell(comprobante);
            bloque1.AddCell(viviendacom);
            bloque1.AddCell(direcciontxt);
            bloque1.AddCell(cptxt);
            bloque1.AddCell(comprobantetxt);
            bloque1.AddCell(viviendacomtxt);
            bloque1.AddCell(autorizacion);
            bloque1.AddCell(visitada);
            bloque1.AddCell(medios);
            bloque1.AddCell(vacio);
            bloque1.AddCell(autorizaciontxt);
            bloque1.AddCell(visitadatxt);
            bloque1.AddCell(mediostxt);
            doc.Add(bloque1);

            //InicioBloque1
            //TablaBloque1

            float[] tama1 = { 25 };
            Table bloque1a = new Table(UnitValue.CreatePercentArray(tama1));
            bloque1a.SetRelativePosition(55, 30, 50, 40);
            Cell header = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE 1 ''RIESGOS EN EL ENTORNO DE LA VIVIENDA''"))
            .SetFont(fonts)
            .SetFontSize(10)
           .SetBorder(Border.NO_BORDER)
           .SetFontColor(DeviceGray.BLACK)
           .SetTextAlignment(TextAlignment.CENTER);
            bloque1.AddHeaderCell(vehiculo);

            Cell riesgos = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿En el lugar en el que se ubica la vivienda o en sus cercanías encontramos alguna de las siguientes situaciones?"));
            //Campo de Base de Datos
            Cell riesgostxt = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(fonte)
            .SetFontColor(new DeviceRgb(130, 27, 63))
            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(7)
            .Add(new Paragraph("Existen: " + beneficiario.Ilegal == "SI" ? "Asentamiento Ilegal, " : "" +
                                beneficiario.Autopista == "SI" ? "Cerca de Autopista, " : "" +
                                beneficiario.Tren == "SI" ? "Cerca de Tren, " : "" +
                                beneficiario.Torres == "SI" ? "Asentamiento Ilegal, " : "" +
                                beneficiario.Ductos == "SI" ? "Asentamiento Ilegal, " : "" +
                                beneficiario.Derrumbes == "SI" ? "Asentamiento Ilegal, " : "" +
                                beneficiario.Rios == "SI" ? "Asentamiento Ilegal, " : "" +
                                beneficiario.Ningun_s == "SI" ? "Ninguno " : "" ));
            bloque1a.AddCell(header);
            bloque1a.AddCell(riesgos);
            bloque1a.AddCell(riesgostxt);
            doc.Add(bloque1a);



            //InicioBloque1
            //TablaBloque1

            float[] tama2 = { 25 };
            Table bloque2 = new Table(UnitValue.CreatePercentArray(tama2));
            bloque2.SetRelativePosition(35, 35, 50, 40);
            Cell header1 = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE 2 ''RIESGOS INTERNOS PARA LA VIVIENDA''"))
            .SetFont(fonts)
            .SetFontSize(10)
            .SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            bloque1.AddHeaderCell(vehiculo);
            Cell situaciones = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Dentro de la vivienda se observa alguna de las siguientes situaciones de riesgo para la misma o para quienes la habitan?"));
            //Campo de Base de Datos
            Cell situacionestxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph("Existen: " + beneficiario.Gmuros == "SI" ? "Riesgos en muros, " : "" +
                                beneficiario.Gpisos == "SI" ? "Riesgos en pisos, " : "" +
                                beneficiario.Dtecho == "SI" ? "Riesgos en techo, " : "" +
                                beneficiario.Inclinacion == "SI" ? "Riesgos en incluinación, " : "" +
                                beneficiario.Ningun_r == "SI" ? "Ninguno" : ""));
            bloque2.AddCell(header1);
            bloque2.AddCell(situaciones);
            bloque2.AddCell(situacionestxt);
            doc.Add(bloque2);






            //INICIO DEL BLOQUE 3
            //TablaBloque3  
            float[] tama3 = { 10, 10, 10, 10 };
            Table bloque3 = new Table(UnitValue.CreatePercentArray(tama3));
            bloque3.SetRelativePosition(5, 40, 50, 40);
            Cell header3 = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE 3 ''DATOS SOCIOECONÓMICOS DE LOS HABITANTES DE LA VIVIENDA''"))
            .SetFont(fonts)
            .SetFontSize(10)
           .SetBorder(Border.NO_BORDER)
           .SetFontColor(DeviceGray.BLACK)
           .SetTextAlignment(TextAlignment.CENTER);
            bloque3.AddHeaderCell(header3);

            Cell condicionesvivienda = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Las condiciones de la vivienda y de la zona corresponden a las características socioeconómicas de las personas a las que va dirigido el Programa?"));

            Cell ingresomensual = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Aproximadamente  ¿cuál es su ingreso total mensual? "));

            Cell ayudatrabajos = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuenta con quien le puede ayudar en sus trabajos de obra o tiene la posibilidad de contratar a alguien que le guíe o se encargue de la obra?"));


            Cell numhabitantes = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuál es el número de habitantes de la vivienda?"));

            //Campos de base de datos
            Cell condicionesviviendatxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Condiciones_vivienda));

            Cell ingresomensualtxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Ingreso_mensual_total));

            Cell ayudatrabajostxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Cuenta_ayude_trabajos));

            Cell numhabitantestxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Numero_habitantes));


            //Parte 2 de la tabla

            Cell contribuyen = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Además de usted, ¿cuántos integrantes de la familia contribuyen al ingreso de la vivienda?"));

            Cell adultosmayores = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Habitan adultos mayores en la vivienda?"));

            Cell niñas = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas niñas habitan la vivienda?"));


            Cell niños = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas niños habitan la vivienda? "));

            //Campos de base de datos
            Cell contribuyentxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.Integrantes_contribuyen));

            Cell adultosmayorestxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_mayores));

            Cell niñastxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_ninas));

            Cell niñostxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_ninos));




            //Parte 3 de la tabla

            Cell menores = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas personas que habitan la vivienda son menores de edad jefes de familia?"));

            Cell indigena = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas personas que habitan la vivienda pertenecen a algún pueblo indígena?"));

            Cell madressolteras = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas personas que habitan en la vivienda son madres solteras jefas de familia?"));


            Cell discapacidad = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas personas que habitan en la vivienda tienen alguna discapacidad permanente?"));

            //Campos de base de datos
            Cell menorestxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_menores_jefes));

            Cell indigenatxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_indigenas));

            Cell madressolterastxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_solteras_jefas));

            Cell discapacidadtxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_discapacidad));
            bloque3.AddCell(condicionesvivienda);
            bloque3.AddCell(ingresomensual);
            bloque3.AddCell(ayudatrabajos);
            bloque3.AddCell(numhabitantes);
            bloque3.AddCell(condicionesviviendatxt);
            bloque3.AddCell(ingresomensualtxt);
            bloque3.AddCell(ayudatrabajostxt);
            bloque3.AddCell(numhabitantestxt);
            bloque3.AddCell(contribuyen);
            bloque3.AddCell(adultosmayores);
            bloque3.AddCell(niñas);
            bloque3.AddCell(niños);
            bloque3.AddCell(contribuyentxt);
            bloque3.AddCell(adultosmayorestxt);
            bloque3.AddCell(niñastxt);
            bloque3.AddCell(niñostxt);
            bloque3.AddCell(menores);
            bloque3.AddCell(indigena);
            bloque3.AddCell(madressolteras);
            bloque3.AddCell(discapacidad);
            bloque3.AddCell(menorestxt);
            bloque3.AddCell(indigenatxt);
            bloque3.AddCell(madressolterastxt);
            bloque3.AddCell(discapacidadtxt);
            doc.Add(bloque3);





            //InicioBloque4
            //TablaBloque4

            float[] tama4 = { 5, 5, 5, 5 };
            Table bloque4 = new Table(UnitValue.CreatePercentArray(tama4));
            bloque4.SetRelativePosition(4, 45, 50, 40);
            Cell header4 = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE 4 ''CARACTERÍSTICAS DE LA VIVIENDA''"))
            .SetFont(fonts)
            .SetFontSize(10)
            .SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            bloque4.AddHeaderCell(header4);

            Cell recamaras = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿Cuántas recámaras tiene la vivienda? "));

            Cell material = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿De qué material es el techo de la vivienda?"));

            Cell tipopiso = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonts)
         .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(8)
           .Add(new Paragraph("¿Con qué tipo de piso cuenta la vivienda?"));

            Cell muros = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonts)
           .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(8)
           .Add(new Paragraph("¿De qué material son los muros de la vivienda? "));

            //Datos Base de Datos
            Cell recamarastxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph(beneficiario.N_cuartos));

            Cell materialtxt = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph(beneficiario.N_techo));

            Cell tipopisotxt = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonte)
           .SetFontColor(new DeviceRgb(130, 27, 63))
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(7)
           .Add(new Paragraph(beneficiario.N_piso));

            Cell murostxt = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonte)
           .SetFontColor(new DeviceRgb(130, 27, 63))
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(7)
           .Add(new Paragraph(beneficiario.N_muros));
            bloque4.AddCell(recamaras);
            bloque4.AddCell(material);
            bloque4.AddCell(tipopiso);
            bloque4.AddCell(muros);
            bloque4.AddCell(recamarastxt);
            bloque4.AddCell(materialtxt);
            bloque4.AddCell(tipopisotxt);
            bloque4.AddCell(murostxt);
            doc.Add(bloque4);
            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            //Bloque Imagenes
            //TablaBloqueImagenes

            float[] tamaI = { 5, 5, 5, 5 };
            Table bloqueI = new Table(UnitValue.CreatePercentArray(tamaI));
            bloqueI.SetRelativePosition(4, 45, 50, 40);
            Cell headerI = new Cell(1, 4)
            .Add(new Paragraph("BLOQUE 5 ''Evidencia Fotográfica''"))
            .SetFont(fonts)
            .SetFontSize(10)
            .SetBorder(Border.NO_BORDER)
            .SetFontColor(DeviceGray.BLACK)
            .SetTextAlignment(TextAlignment.CENTER);
            bloqueI.AddHeaderCell(headerI);
            ImageData imageData = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/"+beneficiario.CURPR+"/"+beneficiario.Img_curp_correcion);
            Image pdfImg = new Image(imageData);
            Cell image = new Cell(1, 1)
             .SetWidth(10)
             .SetBorder(Border.NO_BORDER)
             .Add(pdfImg.SetAutoScale(true));

            bloqueI.AddCell(image);
            doc.Add(bloqueI);
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
