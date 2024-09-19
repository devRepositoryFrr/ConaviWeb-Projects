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
    public class PmvC4Controller : Controller
    {
        private readonly IReporteadorRepository _reporteadorRepository;
        private readonly IWebHostEnvironment _environment;
        public PmvC4Controller(IWebHostEnvironment environment, IReporteadorRepository reporteadorRepository)
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
            string[] ids =  {
               
"2404079618"
            };
            foreach (var id_unico in ids)
            {
                var beneficiario = await _reporteadorRepository.GetPMV24C4(id_unico);
                GenerateSavePDFAsync(beneficiario);
            }
            return Ok();
        }
        public void GenerateSavePDFAsync(PevC4 beneficiario)
        {
            var fileName = beneficiario.id_unico + ".pdf";
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "PMVC4", "AGOSTO", beneficiario.Estado);
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
        public Document GetPDF(Document doc, PevC4 beneficiario)
        {
            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fonts = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            //TITULO DEL DOCUMENTO

            Paragraph titulo = new Paragraph("CUESTIONARIO PVS CONCLUSIÓN")
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
             .Add(new Paragraph(beneficiario.nombre));

            Cell Rcurp = new Cell(2, 2)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph(beneficiario.curp));
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
             .Add(new Paragraph("¿Cuál es la situación en la que se encuentran los trabajos de ampliación o mejoramiento de la vivienda?"));
            Cell Rsituacion = new Cell(5, 4)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph(beneficiario.situacion_trabajos));
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

            if (beneficiario.situacion_trabajos == "OBRA EN PROCESO")
            {
            Table bloque2 = new Table(1);
            Cell tituloBloque2 = new Cell(1, 1)
            .Add(new Paragraph("BLOQUE B ''CUESTIONARIO''"))
            .SetFont(fonts)
            .SetFontSize(10)
           .SetBorder(Border.NO_BORDER)
           .SetFontColor(DeviceGray.BLACK)
           .SetTextAlignment(TextAlignment.CENTER);
            bloque2.AddHeaderCell(tituloBloque2);
            Cell procesoObra = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonts)
             .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
             .SetVerticalAlignment((VerticalAlignment.MIDDLE))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(8)
             .Add(new Paragraph("¿La persona beneficiaria tiene una obra en proceso?"));
            bloque2.AddCell(procesoObra);
            Cell RprocesoObra = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph(beneficiario.proceso_obra));
            bloque2.AddCell(RprocesoObra);
            Cell PorqueObra = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("¿Por qué razón la persona beneficiaria tiene aún su obra en proceso?"));
            bloque2.AddCell(PorqueObra);
            Cell RPorqueObra = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph(beneficiario.porque_proceso_obra));
            bloque2.AddCell(RPorqueObra);
            Cell apoyoDom = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("¿El apoyo se está  aplicando en el domicilio registrado en el PMV?"));
            bloque2.AddCell(apoyoDom);
            Cell RApoyoDom = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph(beneficiario.proceso_apoyo_aplico_domicilio));
            bloque2.AddCell(RApoyoDom);
            Cell sinPago = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("¿La persona beneficiaria o algún miembro de la familia beneficiaria está participando en los trabajos de la obra sin pago?"));
            bloque2.AddCell(sinPago);
            Cell RSinPago = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_miembro_sin_pago));
            bloque2.AddCell(RSinPago);
            Cell personasContra = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Para la realización de los trabajos de obra, ¿a cuantas personas ha contratado la persona beneficiaria (albañiles y ayudantes, plomeros, trabajadores de herrería, etc.)"));
            bloque2.AddCell(personasContra);
            Cell RPersonasContra = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_personas_contradadas));
            bloque2.AddCell(RPersonasContra);
            Cell escaMat = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por la escasez de materiales?"));
            bloque2.AddCell(escaMat);
            Cell REscaMat = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_escasez_materiales));
            bloque2.AddCell(REscaMat);
            Cell costoMat = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por el incremento en el costo de los materiales?"));
            bloque2.AddCell(costoMat);
            Cell RCostoMat = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_costo_materiales));
            bloque2.AddCell(RCostoMat);
            Cell faltaMano = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por falta de mano de obra?"));
            bloque2.AddCell(faltaMano);
            Cell RFaltaMano = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_falta_manoobra));
            bloque2.AddCell(RFaltaMano);
            Cell costoMano = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por el incremento en el costo de la mano de obra?"));
            bloque2.AddCell(costoMano);
            Cell RCostoMano = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_costo_manoobra));
            bloque2.AddCell(RCostoMano);
            Cell permisoMun = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por problemas con permisos y tramites con el Municipio?"));
            bloque2.AddCell(permisoMun);
            Cell RPermisoMun = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_permiso_municipio));
            bloque2.AddCell(RPermisoMun);
            Cell asesoriaTec = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por falta de asesoria tecnica?"));
            bloque2.AddCell(asesoriaTec);
            Cell RAsesoriaTec = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_asesoria_tecnica));
            bloque2.AddCell(RAsesoriaTec);
            Cell poliRec = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por que le solicitaron una parte de su recurso como pago por alguna gestión o le han presionado para que apoye a alguna persona, grupo o partido político?"));
            bloque2.AddCell(poliRec);
            Cell RPoliRec = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(new Paragraph(beneficiario.proceso_problema_pago_gestion));
            bloque2.AddCell(RPoliRec);
                doc.Add(bloque2);
                doc.Add(new AreaBreak());
                Table bloque3 = new Table(1);
                Cell tituloBloque3 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE C ''EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque3.AddHeaderCell(tituloBloque3);
                Cell actaDef = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("Acta de defunción"));
                bloque3.AddCell(actaDef);
                ////Update the encoded png image
                var image = new Image(ImageDataFactory.Create(beneficiario.imgActaDef));
                Cell RActaDef = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(image.SetMaxWidth(150));
                bloque3.AddCell(RActaDef);
                Cell actaHechos1 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 1"));
            bloque3.AddCell(actaHechos1);
            var actaH1 = new Image(ImageDataFactory.Create(beneficiario.img1));
            Cell RActaHechos1 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(actaH1.SetMaxWidth(150));
            bloque3.AddCell(RActaHechos1);
            Cell actaHechos2 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 2"));
            bloque3.AddCell(actaHechos2);
            var actaH2 = new Image(ImageDataFactory.Create(beneficiario.img2));
            Cell RActaHechos2 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(actaH2.SetMaxWidth(150));
            bloque3.AddCell(RActaHechos2);
            Cell actaHechos3 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 3"));
            bloque3.AddCell(actaHechos3);
            var actaH3 = new Image(ImageDataFactory.Create(beneficiario.img3));
            Cell RActaHechos3 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(actaH3.SetMaxWidth(150));
            bloque3.AddCell(RActaHechos3);
            Cell actaHechos4 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 4"));
            bloque3.AddCell(actaHechos4);
            var actaH4 = new Image(ImageDataFactory.Create(beneficiario.img4));
            Cell RActaHechos4 = new Cell(1, 1)
                            .SetTextAlignment(TextAlignment.CENTER)
                            .SetFont(fonte)
                            .SetFontColor(new DeviceRgb(130, 27, 63))
                            .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                            .SetBorder(Border.NO_BORDER)
                            .SetFontSize(7)
                            .Add(actaH4.SetMaxWidth(150));
            bloque3.AddCell(RActaHechos4);
            doc.Add(bloque3);
            }
            if (beneficiario.situacion_trabajos == "OBRA SUSPENDIDA")
            {
                Table bloque2 = new Table(1);
                Cell tituloBloque2 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE B ''CUESTIONARIO Y EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque2.AddHeaderCell(tituloBloque2);

                Cell suspend = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("¿La persona beneficiaria suspendio la obra?"));
                bloque2.AddCell(suspend);
                Cell Rsuspend = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.suspendio_obra));
                bloque2.AddCell(Rsuspend);
                Cell porqueObra = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("¿Por qué razón la persona beneficiaria tiene suspendio la obra?"));
                bloque2.AddCell(porqueObra);
                Cell RporqueObra = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.porque_suspendio_obra));
                bloque2.AddCell(RporqueObra);
                Cell apoyoDom = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿El apoyo se está  aplicando en el domicilio registrado en el PMV?"));
                bloque2.AddCell(apoyoDom);
                Cell RApoyoDom = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.suspende_apoyo_aplico_domicilio));
                bloque2.AddCell(RApoyoDom);
                Cell sinPago = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿La persona beneficiaria o algún miembro de la familia beneficiaria está participando en los trabajos de la obra sin pago?"));
                bloque2.AddCell(sinPago);
                Cell RSinPago = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_miembro_sin_pago));
                bloque2.AddCell(RSinPago);
                Cell personasContra = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Para la realización de los trabajos de obra, ¿a cuantas personas ha contratado la persona beneficiaria (albañiles y ayudantes, plomeros, trabajadores de herrería, etc.)"));
                bloque2.AddCell(personasContra);
                Cell RPersonasContra = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_personas_contradadas));
                bloque2.AddCell(RPersonasContra);
                Cell escaMat = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por la escasez de materiales?"));
                bloque2.AddCell(escaMat);
                Cell REscaMat = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_escasez_materiales));
                bloque2.AddCell(REscaMat);
                Cell costoMat = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por el incremento en el costo de los materiales?"));
                bloque2.AddCell(costoMat);
                Cell RCostoMat = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_costo_materiales));
                bloque2.AddCell(RCostoMat);
                Cell faltaMano = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por falta de mano de obra?"));
                bloque2.AddCell(faltaMano);
                Cell RFaltaMano = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_falta_manoobra));
                bloque2.AddCell(RFaltaMano);
                Cell costoMano = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por el incremento en el costo de la mano de obra?"));
                bloque2.AddCell(costoMano);
                Cell RCostoMano = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_costo_manoobra));
                bloque2.AddCell(RCostoMano);
                Cell permisoMun = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por problemas con permisos y tramites con el Municipio?"));
                bloque2.AddCell(permisoMun);
                Cell RPermisoMun = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_permiso_municipio));
                bloque2.AddCell(RPermisoMun);
                Cell asesoriaTec = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por falta de asesoria tecnica?"));
                bloque2.AddCell(asesoriaTec);
                Cell RAsesoriaTec = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_asesoria_tecnica));
                bloque2.AddCell(RAsesoriaTec);
                Cell poliRec = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria ha tenido problemas en la ejecución de su obra por que le solicitaron una parte de su recurso como pago por alguna gestión o le han presionado para que apoye a alguna persona, grupo o partido político?"));
                bloque2.AddCell(poliRec);
                Cell RPoliRec = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.suspende_problema_pago_gestion));
                bloque2.AddCell(RPoliRec);
                doc.Add(bloque2);
                doc.Add(new AreaBreak());
                Table bloque3 = new Table(1);
                Cell tituloBloque3 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE C ''EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque3.AddHeaderCell(tituloBloque3);
                Cell actaDef = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("Acta de defunción"));
                bloque3.AddCell(actaDef);
                ////Update the encoded png image
                var image = new Image(ImageDataFactory.Create(beneficiario.imgActaDef));
                Cell RActaDef = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(image.SetMaxWidth(150));
                bloque3.AddCell(RActaDef);
                Cell actaHechos1 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 1"));
                bloque3.AddCell(actaHechos1);
                var actaH1 = new Image(ImageDataFactory.Create(beneficiario.img1));
                Cell RActaHechos1 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH1.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos1);
                Cell actaHechos2 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 2"));
                bloque3.AddCell(actaHechos2);
                var actaH2 = new Image(ImageDataFactory.Create(beneficiario.img2));
                Cell RActaHechos2 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH2.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos2);
                Cell actaHechos3 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 3"));
                bloque3.AddCell(actaHechos3);
                var actaH3 = new Image(ImageDataFactory.Create(beneficiario.img3));
                Cell RActaHechos3 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH3.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos3);
                Cell actaHechos4 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 4"));
                bloque3.AddCell(actaHechos4);
                var actaH4 = new Image(ImageDataFactory.Create(beneficiario.img4));
                Cell RActaHechos4 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH4.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos4);
                doc.Add(bloque3);
            }
            if (beneficiario.situacion_trabajos == "OBRA NO INICIADA")
            {
                Table bloque2 = new Table(1);
                Cell tituloBloque2 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE B ''CUESTIONARIO Y EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque2.AddHeaderCell(tituloBloque2);

                Cell noInicio = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("¿La persona beneficiaria no ha iniciado la obra?"));
                bloque2.AddCell(noInicio);
                Cell RnoInicio = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.no_inicio_obra));
                bloque2.AddCell(RnoInicio);
                Cell porqueObra = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("¿Por qué razón la persona beneficiaria no inicio la obra?"));
                bloque2.AddCell(porqueObra);
                Cell RporqueObra = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.porque_no_inicio_obra));
                bloque2.AddCell(RporqueObra);
                doc.Add(bloque2);
                doc.Add(new AreaBreak());
                Table bloque3 = new Table(1);
                Cell tituloBloque3 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE C ''EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque3.AddHeaderCell(tituloBloque3);
                Cell actaDef = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFont(fonts)
              .SetWidth(10)
              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(8)
              .Add(new Paragraph("Acta de defunción"));
                bloque3.AddCell(actaDef);
                ////Update the encoded png image
                var image = new Image(ImageDataFactory.Create(beneficiario.imgActaDef));
                Cell RActaDef = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(image.SetMaxWidth(150));
                bloque3.AddCell(RActaDef);
                Cell actaHechos1 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Acta de hechos 1"));
                bloque3.AddCell(actaHechos1);
                var actaH1 = new Image(ImageDataFactory.Create(beneficiario.img1));
                Cell RActaHechos1 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH1.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos1);
                Cell actaHechos2 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 2"));
                bloque3.AddCell(actaHechos2);
                var actaH2 = new Image(ImageDataFactory.Create(beneficiario.img2));
                Cell RActaHechos2 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH2.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos2);
                Cell actaHechos3 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 3"));
                bloque3.AddCell(actaHechos3);
                var actaH3 = new Image(ImageDataFactory.Create(beneficiario.img3));
                Cell RActaHechos3 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH3.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos3);
                Cell actaHechos4 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Acta de hechos 4"));
                bloque3.AddCell(actaHechos4);
                var actaH4 = new Image(ImageDataFactory.Create(beneficiario.img4));
                Cell RActaHechos4 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH4.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos4);
                doc.Add(bloque3);
            }
            if (beneficiario.situacion_trabajos == "OBRA CONCLUIDA")
            {
                Table bloque2 = new Table(1);
                Cell tituloBloque2 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE B ''CUESTIONARIO Y EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque2.AddHeaderCell(tituloBloque2);
                Cell apoyoDom = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿El apoyo se está  aplicando en el domicilio registrado en el PMV?"));
                bloque2.AddCell(apoyoDom);
                Cell RApoyoDom = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetFontColor(new DeviceRgb(130, 27, 63))
                    .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(beneficiario.concluye_apoyo_aplico_domicilio));
                bloque2.AddCell(RApoyoDom);
                Cell tiempo = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿En cuanto tiempo hizo su obra de mojaramiento o ampliación?"));
                bloque2.AddCell(tiempo);
                Cell Rtiempo = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.tiempo_obra));
                bloque2.AddCell(Rtiempo);
                Cell sinPago = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("¿La persona beneficiaria o algún miembro de la familia beneficiaria participo en los trabajos de la obra sin pago?"));
                bloque2.AddCell(sinPago);
                Cell RSinPago = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_miembro_sin_pago));
                bloque2.AddCell(RSinPago);
                Cell personasContra = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Para la realización de los trabajos de obra, ¿a cuantas personas contratado la persona beneficiaria(albañiles y ayudantes, plomeros, trabajadores de herrería, etc)"));
                bloque2.AddCell(personasContra);
                Cell RPersonasContra = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_miembro_sin_pago));
                bloque2.AddCell(RPersonasContra);
                Cell recAd = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿utilizo los recursos de su apoyo para construir una recamara adicional?"));
                bloque2.AddCell(recAd);
                Cell RrecAd = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_recamara));
                bloque2.AddCell(RrecAd);
                Cell recBano = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para construir un baño?"));
                bloque2.AddCell(recBano);
                Cell RrecBano = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_bano));
                bloque2.AddCell(RrecBano);
                Cell recCocina = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para construir una cocina?"));
                bloque2.AddCell(recCocina);
                Cell RrecCocina = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_cocina));
                bloque2.AddCell(RrecCocina);
                Cell recCuarto = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para construir otros cuartos?"));
                bloque2.AddCell(recCuarto);
                Cell RrecCuarto = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_cuartos));
                bloque2.AddCell(RrecCuarto);
                Cell recInsta = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para sustituir o mejorar sus instalaciones (agua, drenaje, luz)?"));
                bloque2.AddCell(recInsta);
                Cell RrecInsta = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_instalaciones));
                bloque2.AddCell(RrecInsta);
                Cell recPV = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para colocar,sustituir o mejorar sus puertas y/o ventanas?"));
                bloque2.AddCell(recPV);
                Cell RrecPV = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_puertas_ventanas));
                bloque2.AddCell(RrecPV);
                Cell recLoPin = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para colocar,sustituir o mejorar acabados (loseta, pintura, yeso, aplanados, impermeabilizante, etc) otros cuartos?"));
                bloque2.AddCell(recLoPin);
                Cell RrecLoPin = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_acabados));
                bloque2.AddCell(RrecLoPin);
                Cell recEstr = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para trabajos de estructura (losa, firme, refuerzo de muros, losas o cimientos, muros adicionales, etc)"));
                bloque2.AddCell(recEstr);
                Cell RrecEstr = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_estructura));
                bloque2.AddCell(RrecEstr);
                Cell recOtras = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Utilizo los recursos de su apoyo para trabajos no considerados en otras opciones: biodigestor, calentador solar, captación de agua de lluvia, cisternas, fosas sépticas, tinacos, páneles solares, etc?"));
                bloque2.AddCell(recOtras);
                Cell RrecOtras = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.recursos_otras_opciones));
                bloque2.AddCell(RrecOtras);
                Cell aporOtra = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Además del dinero otorgado mediante el apoyo,¿el beneficiario aportó alguna otra cantidad?"));
                bloque2.AddCell(aporOtra);
                Cell RaporOtra = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.beneficiario_aporto));
                bloque2.AddCell(RaporOtra);
                Cell cantApor = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿Qué cantidad aportó?"));
                bloque2.AddCell(cantApor);
                Cell RcantApor = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.cantidad_aporto));
                bloque2.AddCell(RcantApor);
                Cell ocupoRec = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("¿En qué ocupó ese recurso?"));
                bloque2.AddCell(ocupoRec);
                Cell RocupoRec = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.ocupo_recurso));
                bloque2.AddCell(RocupoRec);
                Cell escaMat = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por la escasez de materiales?"));
                bloque2.AddCell(escaMat);
                Cell REscaMat = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_escasez_materiales));
                bloque2.AddCell(REscaMat);
                Cell costoMat = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por el incremento en el costo de los materiales?"));
                bloque2.AddCell(costoMat);
                Cell RCostoMat = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_costo_materiales));
                bloque2.AddCell(RCostoMat);
                Cell faltaMano = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por falta de mano de obra?"));
                bloque2.AddCell(faltaMano);
                Cell RFaltaMano = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_falta_manoobra));
                bloque2.AddCell(RFaltaMano);
                Cell costoMano = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por incremento en el costo de la mano de obra?"));
                bloque2.AddCell(costoMano);
                Cell RCostoMano = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_costo_manoobra));
                bloque2.AddCell(RCostoMano);
                Cell permisoMun = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por problemas con permisos y trámites con el municipio?"));
                bloque2.AddCell(permisoMun);
                Cell RPermisoMun = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_permiso_municipio));
                bloque2.AddCell(RPermisoMun);
                Cell asesoriaTec = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("La persona beneficiaria tuvo algun problema en la ejecución de su obra por falta de asesoria tecnica?"));
                bloque2.AddCell(asesoriaTec);
                Cell RAsesoriaTec = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_asesoria_tecnica));
                bloque2.AddCell(RAsesoriaTec);
                Cell poliRec = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Hasta este momento, ¿la persona beneficiaria tuvo algun problema en la ejecución de su obra por que le solicitaron una parte de su recurso como pago por alguna gestión o le han presionado para que apoye a alguna persona, grupo o partido político?"));
                bloque2.AddCell(poliRec);
                Cell RPoliRec = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.concluye_problema_pago_gestion));
                bloque2.AddCell(RPoliRec);
                Cell ayuPro = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("En una calificacion del 1 al 10 ,¿Que tanto le ayudó el programa a mejorar las condiciones de su vivienda? (Donde 1 es el mas bajo y el 10 es el más alto)"));
                bloque2.AddCell(ayuPro);
                Cell RayuPro = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.ayudo_programa));
                bloque2.AddCell(RayuPro);
                Cell calPro = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("En una escala del 1 al 10 ,¿cómo calificaria el Programa?(Donde 1 es el más bajo y 10 es el más alto)"));
                bloque2.AddCell(calPro);
                Cell RcalPro = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(new Paragraph(beneficiario.califica_programa));
                bloque2.AddCell(RcalPro);
                doc.Add(bloque2);
                doc.Add(new AreaBreak());
                Table bloque3 = new Table(1);
                Cell tituloBloque3 = new Cell(1, 1)
                .Add(new Paragraph("BLOQUE C ''EVIDENCIA''"))
                .SetFont(fonts)
                .SetFontSize(10)
               .SetBorder(Border.NO_BORDER)
               .SetFontColor(DeviceGray.BLACK)
               .SetTextAlignment(TextAlignment.CENTER);
                bloque3.AddHeaderCell(tituloBloque3);
                Cell actaHechos1 = new Cell(1, 1)
                          .SetTextAlignment(TextAlignment.CENTER)
                          .SetFont(fonts)
                          .SetWidth(10)
                          .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                          .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                          .SetBorder(Border.NO_BORDER)
                          .SetFontSize(8)
                          .Add(new Paragraph("Constancia 1"));
                bloque3.AddCell(actaHechos1);
                var actaH1 = new Image(ImageDataFactory.Create(beneficiario.img1));
                Cell RActaHechos1 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH1.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos1);
                Cell actaHechos2 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Constancia 2"));
                bloque3.AddCell(actaHechos2);
                var actaH2 = new Image(ImageDataFactory.Create(beneficiario.img2));
                Cell RActaHechos2 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH2.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos2);
                Cell actaHechos3 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Constancia 3"));
                bloque3.AddCell(actaHechos3);
                var actaH3 = new Image(ImageDataFactory.Create(beneficiario.img3));
                Cell RActaHechos3 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH3.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos3);
                Cell actaHechos4 = new Cell(1, 1)
                              .SetTextAlignment(TextAlignment.CENTER)
                              .SetFont(fonts)
                              .SetWidth(10)
                              .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                              .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                              .SetBorder(Border.NO_BORDER)
                              .SetFontSize(8)
                              .Add(new Paragraph("Constancia 4"));
                bloque3.AddCell(actaHechos4);
                var actaH4 = new Image(ImageDataFactory.Create(beneficiario.img4));
                Cell RActaHechos4 = new Cell(1, 1)
                                .SetTextAlignment(TextAlignment.CENTER)
                                .SetFont(fonte)
                                .SetFontColor(new DeviceRgb(130, 27, 63))
                                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                                .SetBorder(Border.NO_BORDER)
                                .SetFontSize(7)
                                .Add(actaH4.SetMaxWidth(150));
                bloque3.AddCell(RActaHechos4);
                doc.Add(bloque3);
            }
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
