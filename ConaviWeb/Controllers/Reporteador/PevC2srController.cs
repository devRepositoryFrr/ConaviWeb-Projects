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
        public async Task<IActionResult> IndexAsync()
        {
            await GetAllBenef();
            return Ok();
        }
        [HttpGet]

        public async Task<IActionResult> GetAllBenef()
        {
            string[] curps =  { "0UBE000624MCSZRLA6" };
            foreach (var curp in curps)
            {
                var beneficiario = await _reporteadorRepository.GetBeneficiario(curp);
                GenerateSavePDFAsync(beneficiario);
            }
            //var beneficiario = await _reporteadorRepository.GetBeneficiario("AEGL870104HCCNNS05");
            //foreach (var beneficiario in beneficiarios)
            //{
                //GenerateSavePDFAsync(beneficiario);
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
            bloque1.SetFixedPosition(1, 50, 540 , 500);
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
             .Add(new Paragraph("Nombre"));
            Cell curp = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("CURP de la persona solicitante"));
            Cell genero = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("Genero"));

            Cell FechaNacimiento = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(fonts)
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetVerticalAlignment((VerticalAlignment.MIDDLE))
               .SetBorder(Border.NO_BORDER)
               .SetFontSize(8)
               .Add(new Paragraph("Fecha de nacimiento"));
         
      
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
            Cell FechaNacimientotxt = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("11-julio-1995"));
            Cell generotxt = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("Masuclino"));

            Cell NombreINEE = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(fonts)
               .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
               .SetVerticalAlignment((VerticalAlignment.MIDDLE))
               .SetBorder(Border.NO_BORDER)
               .SetFontSize(8)
               .Add(new Paragraph("Nombre INE"));        
            Cell Direccion = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetWidth(10)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Entidad"));
            Cell cp = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Municipio"));
            Cell Georeferencia = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Localidad"));
              Cell NombreINE = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("Jimenez")); 
                Cell Entiddad = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("Los cabos San Lucas")); 
            Cell MunicipioW = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(130, 27, 63))
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(7)
             .Add(new Paragraph("La paz"));
            Cell Lcalidad = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(fonte)
            .SetFontColor(new DeviceRgb(130, 27, 63))
            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(7)
            .Add(new Paragraph("San Bartholo"));


            Cell CP = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("CP"));

            Cell Referencia = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("Referencia"));

            Cell TeContacto = new Cell(1, 1)
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFont(fonts)
           .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
           .SetVerticalAlignment((VerticalAlignment.MIDDLE))
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(8)
           .Add(new Paragraph("Telefonos de Contacto"));

            Cell vacio = new Cell(1, 1)
             .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph(""));

            Cell CPtxt = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonte)
              .SetFontColor(new DeviceRgb(130, 27, 63))
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(7)  
              .Add(new Paragraph("56560"));   
            
            Cell Referenciatxt = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonte)
              .SetFontColor(new DeviceRgb(130, 27, 63))
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(7)  
              .Add(new Paragraph("En las banquetas"));

            Cell TeContactotxt = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("556566560"));


            Cell propiedadVEs = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("La persona solicitante,¿Es el propietario de la vivienda?"));
          
            Cell propiedadCEs = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("La persona solicitante,¿cuenta con comprobante de la propiedad?"));

            Cell AutorizacionViv = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("¿Cuenta con la autorización del propietario para la realización de los trabajos? "));

            //Se agrega una celda Vacía

            Cell propiedadVEstxt = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("SI"));

            Cell propiedadCEstxt = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("SI"));

            Cell AutorizacionVivtxt = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("SI"));    

            bloque1.AddCell(nombre);
            bloque1.AddCell(curp);
            bloque1.AddCell(genero); 
            bloque1.AddCell(FechaNacimiento);            
            bloque1.AddCell(nombretxt);
            bloque1.AddCell(curpntxt);
            bloque1.AddCell(generotxt);
            bloque1.AddCell(FechaNacimientotxt);
            bloque1.AddCell(NombreINEE);                             
            bloque1.AddCell(Direccion);
            bloque1.AddCell(cp);
            bloque1.AddCell(Georeferencia);
            bloque1.AddCell(NombreINE);
            bloque1.AddCell(Entiddad);  
            bloque1.AddCell(MunicipioW);
            bloque1.AddCell(Lcalidad);
            bloque1.AddCell(CP);
            bloque1.AddCell(Referencia);
            bloque1.AddCell(TeContacto);
            bloque1.AddCell(vacio);
            bloque1.AddCell(CPtxt);
            bloque1.AddCell(Referenciatxt);
            bloque1.AddCell(TeContactotxt);
            bloque1.AddCell(vacio);
            bloque1.AddCell(propiedadVEs);
            bloque1.AddCell(propiedadCEs);
            bloque1.AddCell(AutorizacionViv);
            bloque1.AddCell(vacio);
            bloque1.AddCell(propiedadVEstxt);
            bloque1.AddCell(propiedadCEstxt);
            bloque1.AddCell(AutorizacionVivtxt);
            doc.Add(bloque1);

            //InicioBloque1
            //TablaBloque1

            float[] tama1 = { 25 };
            Table bloque1a = new Table(UnitValue.CreatePercentArray(tama1));
            bloque1a.SetFixedPosition(1, 50, 500, 500);
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
                                beneficiario.Ningun_s == "SI" ? "Ninguno " : ""));
            bloque1a.AddCell(header);
            bloque1a.AddCell(riesgos);
            bloque1a.AddCell(riesgostxt);
            doc.Add(bloque1a);



            //InicioBloque1
            //TablaBloque1

            float[] tama2 = { 25 };
            Table bloque2 = new Table(UnitValue.CreatePercentArray(tama2));
            bloque2.SetFixedPosition(1, 50, 450, 500);
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
            bloque3.SetFixedPosition(1, 50, 230, 500);
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
            bloque4.SetFixedPosition(1, 50, 150, 500);
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
            //doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            //   //Bloque Imagenes
            //   //TablaBloqueImagenes
            //   float[] tamaI = { 5, 5, 5, 5 };
            //   Table bloqueI = new Table(UnitValue.CreatePercentArray(tamaI));
            //   bloqueI.SetRelativePosition(4, 5, 50, 40);
            //   Cell headerI = new Cell(1, 4)
            //   .Add(new Paragraph("''Evidencia Fotográfica Bloque 1''"))
            //   .SetFont(fonts)
            //   .SetFontSize(10)
            //  .SetBorder(Border.NO_BORDER)
            //   .SetFontColor(DeviceGray.BLACK)
            //   .SetTextAlignment(TextAlignment.CENTER);
            //   bloqueI.AddHeaderCell(headerI);

            //   //Agregar Sub Tabla para pintar img1
            //   Cell imgcurp = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcurp1 = { 150f };
            //   Table curpimg = new Table(imgcurp1);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell41 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("CURP CORRECION"));
            //   curpimg.AddCell(cell41).SetBorder(Border.NO_BORDER);
            //   //img_curp_correcion 
            //   if (beneficiario.Img_curp_correcion != "")
            //   {
            //       ImageData imageData3 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_curp_correcion);
            //       Image pdfImg3 = new Image(imageData3);
            //       Cell image3 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg3.SetAutoScale(true));
            //       curpimg.AddCell(image3);
            //       imgcurp.Add(curpimg)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Imagen 2
            //   Cell imgcurp2 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcurp12 = { 150f };
            //   Table curpimg2 = new Table(imgcurp12);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell42 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("INE CORRECION A"));
            //   curpimg2.AddCell(cell42).SetBorder(Border.NO_BORDER);
            //   //img_ine_correcion
            //   if (beneficiario.Img_ine_correcion != "")
            //   {
            //       ImageData imageData2 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_ine_correcion);
            //       Image pdfImg2 = new Image(imageData2);
            //       Cell image2 = new Cell(1, 1)
            //       .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfImg2.SetAutoScale(true));
            //       curpimg2.AddCell(image2);
            //       imgcurp2.Add(curpimg2)
            //       .SetBorder(Border.NO_BORDER);
            //   }
            //   //Imagen 3
            //   Cell imgcurp3 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcurp123 = { 150f };
            //   Table curpimg3 = new Table(imgcurp123);
            //   curpimg3.SetBorder(Border.NO_BORDER);
            //   Cell cell421 = new Cell()
            //   .SetFontSize(7)
            //   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //   .SetFont(fonts)
            //   .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("INE CORRECION B"));
            //   curpimg3.AddCell(cell421).SetBorder(Border.NO_BORDER);
            //   //img_ine_correcion_b
            //   if (beneficiario.Img_ine_correcion_b != "")
            //   {
            //       ImageData imageData21 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_ine_correcion_b);
            //       Image pdfImg21 = new Image(imageData21);
            //       Cell image21 = new Cell(1, 1)
            //       .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfImg21.SetAutoScale(true));
            //       curpimg3.AddCell(image21);
            //       imgcurp3.Add(curpimg3)
            //       .SetBorder(Border.NO_BORDER);
            //   }
            //   //Imagen 4
            //   Cell imgcurp4 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcurp1234 = { 150f };
            //   Table curpimg4 = new Table(imgcurp1234);
            //   curpimg4.SetBorder(Border.NO_BORDER);
            //   Cell imgcp1 = new Cell()
            //   .SetFontSize(7)
            //   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //   .SetFont(fonts)
            //   .SetBorder(Border.NO_BORDER)
            //   .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("COMPROBANTE"));
            //   curpimg4.AddCell(imgcp1).SetBorder(Border.NO_BORDER);
            //   //img_Comprobante
            //   if (beneficiario.Img_Comprobante != "")
            //   {
            //       ImageData imgCurp = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Comprobante);
            //       Image pdfImg212 = new Image(imgCurp);
            //       Cell image212 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg212.SetAutoScale(true));
            //       curpimg4.AddCell(image212);
            //       imgcurp4.Add(curpimg4)
            //       .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img5
            //   Cell imgcurp5 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur5 = { 150f };
            //   Table curpimg5 = new Table(imgcur5);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell415 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("PROPIEDAD 1"));
            //   curpimg5.AddCell(cell415).SetBorder(Border.NO_BORDER);
            //   //img_Propiedad_1
            //   if (beneficiario.Img_Propiedad_1 != "")
            //   {
            //       ImageData imageData5 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Propiedad_1);
            //       Image pdfImg5 = new Image(imageData5);
            //       Cell image5 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg5.SetAutoScale(true));
            //       curpimg5.AddCell(image5);
            //       imgcurp5.Add(curpimg5)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img6
            //   Cell imgcurp6 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur6 = { 150f };
            //   Table curpimg6 = new Table(imgcur6);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell416 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("PROPIEDAD 2"));
            //   curpimg6.AddCell(cell416).SetBorder(Border.NO_BORDER);
            //   //img_Propiedad_2
            //   if (beneficiario.Img_Propiedad_2 != "")
            //   {
            //       ImageData imageData6 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Propiedad_2);
            //       Image pdfImg6 = new Image(imageData6);
            //       Cell image6 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg6.SetAutoScale(true));
            //       curpimg6.AddCell(image6);
            //       imgcurp6.Add(curpimg6)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img7
            //   Cell imgcurp7 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur7 = { 150f };
            //   Table curpimg7 = new Table(imgcur7);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell417 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("PROPIEDAD 3"));
            //   curpimg7.AddCell(cell417).SetBorder(Border.NO_BORDER);
            //   //img_Propiedad_3
            //   if (beneficiario.Img_Propiedad_3 != "")
            //   {
            //       ImageData imageData7 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Propiedad_3);
            //       Image pdfImg7 = new Image(imageData7);
            //       Cell image7 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg7.SetAutoScale(true));
            //       curpimg7.AddCell(image7);
            //       imgcurp7.Add(curpimg7)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img8
            //   Cell imgcurp8 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur8 = { 150f };
            //   Table curpimg8 = new Table(imgcur8);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell418 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("PROPIEDAD 4"));
            //   curpimg8.AddCell(cell418).SetBorder(Border.NO_BORDER);
            //   //img_Propiedad_4
            //   if (beneficiario.Img_Propiedad_4 != "")
            //   {
            //       ImageData imageData8 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Propiedad_4);
            //       Image pdfImg8 = new Image(imageData8);
            //       Cell image8 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg8.SetAutoScale(true));
            //       curpimg8.AddCell(image8);
            //       imgcurp8.Add(curpimg8)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img9
            //   Cell imgcurp9 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur9 = { 150f };
            //   Table curpimg9 = new Table(imgcur9);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell419 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("AUTORIZACION 1"));
            //   curpimg9.AddCell(cell419).SetBorder(Border.NO_BORDER);
            //   //img_Autorizacion_1
            //   if (beneficiario.Img_Autorizacion_1 != "")
            //   {
            //       ImageData imageData9 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Autorizacion_1);
            //       Image pdfImg9 = new Image(imageData9);
            //       Cell image9 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg9.SetAutoScale(true));
            //       curpimg9.AddCell(image9);
            //       imgcurp9.Add(curpimg9)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img10
            //   Cell imgcurp10 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur10 = { 150f };
            //   Table curpimg10 = new Table(imgcur10);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell4110 = new Cell(1, 1)
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("AUTORIZACION 2"));
            //   curpimg10.AddCell(cell4110).SetBorder(Border.NO_BORDER);
            //   //img_Autorizacion_2
            //   if (beneficiario.Img_Autorizacion_2 != "")
            //   {
            //       ImageData imageData10 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Autorizacion_2);
            //       Image pdfImg10 = new Image(imageData10);
            //       Cell image10 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg10.SetAutoScale(true));
            //       curpimg10.AddCell(image10);
            //       imgcurp10.Add(curpimg10)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img11
            //   Cell imgcurp11 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur11 = { 150f };
            //   Table curpimg11 = new Table(imgcur11);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell4111 = new Cell()
            //  .SetFontSize(8)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("AUTORIZACION 3"));
            //   curpimg11.AddCell(cell4111).SetBorder(Border.NO_BORDER);
            //   //img_Autorizacion_3
            //   if (beneficiario.Img_Autorizacion_3 != "")
            //   {
            //       ImageData imageData11 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Autorizacion_3);
            //       Image pdfImg11 = new Image(imageData11);
            //       Cell image11 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg11.SetAutoScale(true));
            //       curpimg11.AddCell(image11);
            //       imgcurp11.Add(curpimg11)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img12
            //   Cell imgcurp13 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur121 = { 150f };
            //   Table curpimg13 = new Table(imgcur121);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell4113 = new Cell(1, 1)
            //  .SetFontSize(8)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("AUTORIZACION 4"));
            //   curpimg13.AddCell(cell4113).SetBorder(Border.NO_BORDER);
            //   //img_Autorizacion_4
            //   if (beneficiario.Img_Autorizacion_4 != "")
            //   {
            //       ImageData imageData13 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Autorizacion_4);
            //       Image pdfImg13 = new Image(imageData13);
            //       Cell image13 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg13.SetAutoScale(true));
            //       curpimg13.AddCell(image13);
            //       imgcurp13.Add(curpimg13)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Img13
            //   Cell imgcurp14 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur124 = { 150f };
            //   Table curpimg14 = new Table(imgcur124);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell4114 = new Cell(1, 1)
            //  .SetFontSize(8)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("AUTORIZACION 5"));
            //   curpimg14.AddCell(cell4114).SetBorder(Border.NO_BORDER);
            //   //img_Autorizacion_5
            //   if (beneficiario.Img_Autorizacion_5 != "")
            //   {
            //       ImageData imageData14 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Autorizacion_5);
            //       Image pdfImg14 = new Image(imageData14);
            //       Cell image14 = new Cell(1, 1)
            //      .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfImg14.SetAutoScale(true));
            //       curpimg14.AddCell(image14);
            //       imgcurp14.Add(curpimg14)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //img14
            //   Cell imgcurp15 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] imgcur125 = { 150f };
            //   Table curpimg15 = new Table(imgcur125);
            //   curpimg.SetBorder(Border.NO_BORDER);
            //   Cell cell4115 = new Cell(1, 1)
            //  .SetFontSize(8)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("Fima")); //Firma#1
            //   curpimg15.AddCell(cell4115).SetBorder(Border.NO_BORDER);
            //   //img_Firma
            //   if (beneficiario.Img_Firma != "")
            //   {
            //       ImageData imageData15 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_Firma);
            //       Image pdfImg15 = new Image(imageData15).ScaleAbsolute(71, 40);
            //       Cell image15 = new Cell(1, 1)
            //      .SetWidth(5)
            //      //.SetBorder(Border.NO_BORDER)
            //      .Add(pdfImg15.SetAutoScale(true));
            //       curpimg15.AddCell(image15);
            //       imgcurp15.Add(curpimg15)
            //      .SetBorder(Border.NO_BORDER);
            //   }



            //   bloqueI.AddCell(imgcurp);
            //   bloqueI.AddCell(imgcurp2);
            //   bloqueI.AddCell(imgcurp3);
            //   bloqueI.AddCell(imgcurp4);
            //   bloqueI.AddCell(imgcurp5);
            //   bloqueI.AddCell(imgcurp6);
            //   bloqueI.AddCell(imgcurp7);
            //   bloqueI.AddCell(imgcurp8);
            //   bloqueI.AddCell(imgcurp9);
            //   bloqueI.AddCell(imgcurp10);
            //   bloqueI.AddCell(imgcurp11);
            //   bloqueI.AddCell(imgcurp13);
            //   bloqueI.AddCell(imgcurp14);
            //   bloqueI.AddCell(imgcurp15);

            //   doc.Add(bloqueI);





            //   //Informacion del Bloque 2


            //   float[] dimension = { 5, 5, 5, 5 };
            //   Table bloqueI2 = new Table(UnitValue.CreatePercentArray(dimension));
            //   bloqueI2.SetRelativePosition(4, 10, 50, 40);
            //   Cell headerI2 = new Cell(1, 4)
            //   .Add(new Paragraph("''Evidencia Fotográfica Bloque 2''"))
            //   .SetFont(fonts)
            //   .SetFontSize(10)
            //  .SetBorder(Border.NO_BORDER)
            //   .SetFontColor(DeviceGray.BLACK)
            //   .SetTextAlignment(TextAlignment.CENTER);
            //   bloqueI2.AddHeaderCell(headerI2);

            //   //Agregar Sub Tabla para pintar img1
            //   Cell imagen1 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim = { 150f };
            //   Table auto1 = new Table(dim);
            //   auto1.SetBorder(Border.NO_BORDER);
            //   Cell ia1 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B2 1"));
            //   auto1.AddCell(ia1).SetBorder(Border.NO_BORDER);
            //   //img_b2_1
            //   if (beneficiario.Img_b2_1 != "")
            //   {
            //       ImageData Imga1 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b2_1);
            //       Image pdfauto1 = new Image(Imga1);
            //       Cell imga1 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfauto1.SetAutoScale(true));
            //       auto1.AddCell(imga1);
            //       imagen1.Add(auto1)
            //      .SetBorder(Border.NO_BORDER);
            //   }

            //   // Imagen 2
            //   Cell imagen2 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim2 = { 150f };
            //   Table auto2 = new Table(dim2);
            //   auto2.SetBorder(Border.NO_BORDER);
            //   Cell ia2 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B2 2"));
            //   auto2.AddCell(ia2).SetBorder(Border.NO_BORDER);
            //   //img_b2_2
            //   if (beneficiario.Img_b2_2 != "")
            //   {
            //       ImageData Imga2 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b2_2);
            //       Image pdfauto2 = new Image(Imga2);
            //       Cell imga2 = new Cell(1, 1)
            //       .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfauto2.SetAutoScale(true));
            //       auto2.AddCell(imga2);
            //       imagen2.Add(auto2)
            //       .SetBorder(Border.NO_BORDER);
            //   }
            //   //Imagen 3
            //   Cell imagen3 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim3 = { 150f };
            //   Table auto3 = new Table(dim3);
            //   auto3.SetBorder(Border.NO_BORDER);
            //   Cell ia3 = new Cell()
            //   .SetFontSize(7)
            //   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //   .SetFont(fonts)
            //   .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B2 3"));
            //   auto3.AddCell(ia3).SetBorder(Border.NO_BORDER);
            //   //img_b2_3
            //   if (beneficiario.Img_b2_3 != "")
            //   {
            //       ImageData Imga3 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b2_3);
            //       Image pdfauto3 = new Image(Imga3);
            //       Cell imga3 = new Cell(1, 1)
            //       .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfauto3.SetAutoScale(true));
            //       auto3.AddCell(imga3);
            //       imagen3.Add(auto3)
            //       .SetBorder(Border.NO_BORDER);
            //   }

            //   //Imagen 4
            //   Cell imagen4 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim4 = { 150f };
            //   Table auto4 = new Table(dim4);
            //   auto4.SetBorder(Border.NO_BORDER);
            //   Cell ia4 = new Cell()
            //   .SetFontSize(7)
            //   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //   .SetFont(fonts)
            //   .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B2 4"));
            //   auto4.AddCell(ia4).SetBorder(Border.NO_BORDER);
            //   //img_b2_4
            //   if (beneficiario.Img_b2_4 != "")
            //   {
            //       ImageData Imga4 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b2_4);
            //       Image pdfauto4 = new Image(Imga4);
            //       Cell imga4 = new Cell(1, 1)
            //       .SetWidth(10)
            //       .SetBorder(Border.NO_BORDER)
            //       .Add(pdfauto4.SetAutoScale(true));
            //       auto4.AddCell(imga4);
            //       imagen4.Add(auto4)
            //       .SetBorder(Border.NO_BORDER);
            //   }
            //   bloqueI2.AddCell(imagen1);
            //   bloqueI2.AddCell(imagen2);
            //   bloqueI2.AddCell(imagen3);
            //   bloqueI2.AddCell(imagen4);
            //   doc.Add(bloqueI2);





            //   doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            //   //Informacion del Bloque 3


            //   float[] array = { 5, 5, 5, 5 };
            //   Table bloqueI3 = new Table(UnitValue.CreatePercentArray(array));
            //   bloqueI3.SetRelativePosition(4, 15, 50, 40);
            //   Cell headerI3 = new Cell(1, 4)
            //   .Add(new Paragraph("''Evidencia Fotográfica Bloque 3''"))
            //   .SetFont(fonts)
            //   .SetFontSize(10)
            //  .SetBorder(Border.NO_BORDER)
            //   .SetFontColor(DeviceGray.BLACK)
            //   .SetTextAlignment(TextAlignment.CENTER);
            //   bloqueI3.AddHeaderCell(headerI3);

            //   //Agregar Sub Tabla para pintar img1
            //   Cell imgsocio1 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim1 = { 150f };
            //   Table socio1 = new Table(dim1);
            //   socio1.SetBorder(Border.NO_BORDER);
            //   Cell soc1 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B3 1"));
            //   socio1.AddCell(soc1).SetBorder(Border.NO_BORDER);
            //   //img_b3_1
            //   if (beneficiario.Img_b3_1 != "")
            //   {
            //       ImageData Socio1 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b3_1);
            //       Image pdfsoc1 = new Image(Socio1);
            //       Cell imgs1 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfsoc1.SetAutoScale(true));
            //       socio1.AddCell(imgs1);
            //       imgsocio1.Add(socio1)
            //      .SetBorder(Border.NO_BORDER);
            //   }

            //   //Agregar Sub Tabla para pintar img2
            //   Cell imgsocio2 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim5 = { 150f };
            //   Table socio2 = new Table(dim5);
            //   socio1.SetBorder(Border.NO_BORDER);
            //   Cell soc2 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B3 2"));
            //   socio2.AddCell(soc2).SetBorder(Border.NO_BORDER);
            //   //img_b3_2
            //   if (beneficiario.Img_b3_2 != "")
            //   {
            //       ImageData Socio2 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b3_2);
            //       Image pdfsoc2 = new Image(Socio2);
            //       Cell imgs2 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfsoc2.SetAutoScale(true));
            //       socio2.AddCell(imgs2);
            //       imgsocio2.Add(socio2)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Agregar Sub Tabla para pintar img3
            //   Cell imgsocio3 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim6 = { 150f };
            //   Table socio3 = new Table(dim6);
            //   socio3.SetBorder(Border.NO_BORDER);
            //   Cell soc3 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B3 3"));
            //   socio3.AddCell(soc3).SetBorder(Border.NO_BORDER);
            //   //img_b3_3
            //   if (beneficiario.Img_b3_3 != "")
            //   {
            //       ImageData Socio3 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b3_3);
            //       Image pdfsoc3 = new Image(Socio3);
            //       Cell imgs3 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfsoc3.SetAutoScale(true));
            //       socio3.AddCell(imgs3);
            //       imgsocio3.Add(socio3)
            //      .SetBorder(Border.NO_BORDER);
            //   }



            //   //Agregar Sub Tabla para pintar img4
            //   Cell imgsocio4 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] dim7 = { 150f };
            //   Table socio4 = new Table(dim7);
            //   socio4.SetBorder(Border.NO_BORDER);
            //   Cell soc4 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B3 4"));
            //   socio4.AddCell(soc4).SetBorder(Border.NO_BORDER);
            //   //img_b3_4
            //   if (beneficiario.Img_b3_4 != "")
            //   {
            //       ImageData Socio4 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b3_4);
            //       Image pdfsoc4 = new Image(Socio4);
            //       Cell imgs4 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfsoc4.SetAutoScale(true));
            //       socio4.AddCell(imgs4);
            //       imgsocio4.Add(socio4)
            //      .SetBorder(Border.NO_BORDER);
            //   }

            //   bloqueI3.AddCell(imgsocio1);
            //   bloqueI3.AddCell(imgsocio2);
            //   bloqueI3.AddCell(imgsocio3);
            //   bloqueI3.AddCell(imgsocio4);
            //   doc.Add(bloqueI3);





            //   //Informacion del Bloque 4


            //   float[] array2 = { 5, 5, 5, 5 };
            //   Table bloqueI4 = new Table(UnitValue.CreatePercentArray(array2));
            //   bloqueI4.SetRelativePosition(4, 45, 50, 40);
            //   Cell headerI4 = new Cell(1, 4)
            //   .Add(new Paragraph("''Evidencia Fotográfica Bloque 4''"))
            //   .SetFont(fonts)
            //   .SetFontSize(10)
            //   .SetBorder(Border.NO_BORDER)
            //   .SetFontColor(DeviceGray.BLACK)
            //   .SetTextAlignment(TextAlignment.CENTER);
            //   bloqueI4.AddHeaderCell(headerI4);

            //   //Agregar Sub Tabla para pintar img1
            //   Cell caractvivienda = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di1 = { 150f };
            //   Table caravivienda1 = new Table(di1);
            //   caravivienda1.SetBorder(Border.NO_BORDER);
            //   Cell cav = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B4 1"));
            //   caravivienda1.AddCell(cav).SetBorder(Border.NO_BORDER);
            //   //img_b4_1
            //   if (beneficiario.Img_b4_1 != "")
            //   {
            //       ImageData Viv1 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b4_1);
            //       Image pdfviv1 = new Image(Viv1);
            //       Cell imgv1 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv1.SetAutoScale(true));
            //       caravivienda1.AddCell(imgv1);
            //       caractvivienda.Add(caravivienda1)
            //      .SetBorder(Border.NO_BORDER);
            //   }


            //   //Agregar Sub Tabla para pintar img2
            //   Cell caractvivienda2 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di2 = { 150f };
            //   Table caravivienda2 = new Table(di2);
            //   caravivienda2.SetBorder(Border.NO_BORDER);
            //   Cell cav2 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B4 2"));
            //   caravivienda2.AddCell(cav2).SetBorder(Border.NO_BORDER);
            //   //img_b4_2
            //   if (beneficiario.Img_b4_2 != "")
            //   {
            //       ImageData Viv2 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b4_2);
            //       Image pdfviv2 = new Image(Viv2);
            //       Cell imgv2 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv2.SetAutoScale(true));
            //       caravivienda2.AddCell(imgv2);
            //       caractvivienda2.Add(caravivienda2)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Agregar Sub Tabla para pintar img3
            //   Cell caractvivienda3 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di3 = { 150f };
            //   Table caravivienda3 = new Table(di3);
            //   caravivienda3.SetBorder(Border.NO_BORDER);
            //   Cell cav3 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B4 3"));
            //   caravivienda3.AddCell(cav3).SetBorder(Border.NO_BORDER);
            //   //img_b4_3
            //   if (beneficiario.Img_b4_3 != "")
            //   {
            //       ImageData Viv3 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b4_3);
            //       Image pdfviv3 = new Image(Viv3);
            //       Cell imgv3 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv3.SetAutoScale(true));
            //       caravivienda3.AddCell(imgv3);
            //       caractvivienda3.Add(caravivienda3)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Agregar Sub Tabla para pintar img4
            //   Cell caractvivienda4 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di4 = { 150f };
            //   Table caravivienda4 = new Table(di4);
            //   caravivienda4.SetBorder(Border.NO_BORDER);
            //   Cell cav4 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B4 4"));
            //   caravivienda4.AddCell(cav4).SetBorder(Border.NO_BORDER);
            //   //img_b4_4
            //   if (beneficiario.Img_b4_4 != "")
            //   {
            //       ImageData Viv4 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b4_4);
            //       Image pdfviv4 = new Image(Viv4);
            //       Cell imgv4 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv4.SetAutoScale(true));
            //       caravivienda4.AddCell(imgv4);
            //       caractvivienda4.Add(caravivienda4)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   bloqueI4.AddCell(caractvivienda);
            //   bloqueI4.AddCell(caractvivienda2);
            //   bloqueI4.AddCell(caractvivienda3);
            //   bloqueI4.AddCell(caractvivienda4);
            //   doc.Add(bloqueI4);




            //   //Informacion del Bloque 5


            //   float[] array3 = { 5, 5, 5, 5 };
            //   Table bloqueI5 = new Table(UnitValue.CreatePercentArray(array3));
            //   bloqueI5.SetRelativePosition(4, 60, 50, 40);
            //   Cell headerI5 = new Cell(1, 4)
            //   .Add(new Paragraph("''Evidencia Fotográfica Bloque 5''"))
            //   .SetFont(fonts)
            //   .SetFontSize(10)
            //   .SetBorder(Border.NO_BORDER)
            //   .SetFontColor(DeviceGray.BLACK)
            //   .SetTextAlignment(TextAlignment.CENTER);
            //   bloqueI5.AddHeaderCell(headerI5);

            //   //Agregar Sub Tabla para pintar img1
            //   Cell caractvivienda5 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di15 = { 150f };
            //   Table caravivienda15 = new Table(di15);
            //   caravivienda2.SetBorder(Border.NO_BORDER);
            //   Cell cav5 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B5 1"));
            //   caravivienda15.AddCell(cav5).SetBorder(Border.NO_BORDER);
            //   //img_b5_1
            //   if (beneficiario.Img_b5_1 != "")
            //   {
            //       ImageData Viv5 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b5_1);
            //       Image pdfviv5 = new Image(Viv5);
            //       Cell imgv5 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv5.SetAutoScale(true));
            //       caravivienda15.AddCell(imgv5);
            //       caractvivienda5.Add(caravivienda15)
            //      .SetBorder(Border.NO_BORDER);
            //   }

            //   //Agregar Sub Tabla para pintar img2
            //   Cell caractvivienda6 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di6 = { 150f };
            //   Table caravivienda6 = new Table(di6);
            //   caravivienda2.SetBorder(Border.NO_BORDER);
            //   Cell cav6 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B5 2"));
            //   caravivienda6.AddCell(cav6).SetBorder(Border.NO_BORDER);
            //   //img_b5_2
            //   if (beneficiario.Img_b5_2 != "")
            //   {
            //       ImageData Viv6 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b5_2);
            //       Image pdfviv6 = new Image(Viv6);
            //       Cell imgv6 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv6.SetAutoScale(true));
            //       caravivienda6.AddCell(imgv6);
            //       caractvivienda6.Add(caravivienda6)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Agregar Sub Tabla para pintar img3
            //   Cell caractvivienda7 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di7 = { 150f };
            //   Table caravivienda7 = new Table(di7);
            //   caravivienda7.SetBorder(Border.NO_BORDER);
            //   Cell cav7 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B5 3"));
            //   caravivienda7.AddCell(cav7).SetBorder(Border.NO_BORDER);
            //   //img_b5_3
            //   if (beneficiario.Img_b5_3 != "")
            //   {
            //       ImageData Viv7 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b5_3);
            //       Image pdfviv7 = new Image(Viv7);
            //       Cell imgv7 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv7.SetAutoScale(true));
            //       caravivienda7.AddCell(imgv7);
            //       caractvivienda7.Add(caravivienda7)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   //Agregar Sub Tabla para pintar img4
            //   Cell caractvivienda8 = new Cell().SetBorder(Border.NO_BORDER);
            //   float[] di8 = { 150f };
            //   Table caravivienda8 = new Table(di8);
            //   caravivienda4.SetBorder(Border.NO_BORDER);
            //   Cell cav8 = new Cell()
            //  .SetFontSize(7)
            //  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
            //  .SetFont(fonts)
            //  .SetBorder(Border.NO_BORDER)
            //  .SetTextAlignment(TextAlignment.CENTER)
            //   .Add(new Paragraph("IMG B5 4"));
            //   caravivienda8.AddCell(cav8).SetBorder(Border.NO_BORDER);
            //   //img_b5_4
            //   if (beneficiario.Img_b5_4 != "")
            //   {
            //       ImageData Viv8 = ImageDataFactory.Create("https://sistemaintegral.conavi.gob.mx:81/documents/pev_files_c2_sr/" + beneficiario.CURPR + "/" + beneficiario.Img_b5_4);
            //       Image pdfviv8 = new Image(Viv8);
            //       Cell imgv8 = new Cell(1, 1)
            //      .SetWidth(10)
            //      .SetBorder(Border.NO_BORDER)
            //      .Add(pdfviv8.SetAutoScale(true));
            //       caravivienda8.AddCell(imgv8);
            //       caractvivienda8.Add(caravivienda8)
            //      .SetBorder(Border.NO_BORDER);
            //   }
            //   bloqueI5.AddCell(caractvivienda5);
            //   bloqueI5.AddCell(caractvivienda6);
            //   bloqueI5.AddCell(caractvivienda7);
            //   bloqueI5.AddCell(caractvivienda8);
            //   doc.Add(bloqueI5);
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
