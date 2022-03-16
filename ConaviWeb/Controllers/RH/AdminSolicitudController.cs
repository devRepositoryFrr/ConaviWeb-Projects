using ConaviWeb.Commons;
using ConaviWeb.Data.RH;
using ConaviWeb.Model.Response;
using ConaviWeb.Model.RH;
using ConaviWeb.Services;
using ConaviWeb.Tools;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ConaviWeb.Models.AlertsViewModel;

namespace ConaviWeb.Controllers.RH
{
    public class AdminSolicitudController : Controller
    {
        private readonly IRHRepository _rHRepository;
        private readonly IWebHostEnvironment _environment;
        public AdminSolicitudController(IRHRepository rHRepository, IWebHostEnvironment environment)
        {
            _rHRepository = rHRepository;
            _environment = environment;
        }
        public IActionResult Index()
        {
            return View("../RH/AdminSolicitud");
        }
        [HttpGet]
        public async Task<IActionResult> GetSolicitudesAsync()
        {

            var success = await _rHRepository.GetSolicitudes();
            if (!success.Any())
            {
                return BadRequest();
            }
            return Json(new { data = success });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSolicitudAsync([FromBody] Viaticos viaticos)
        {
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            viaticos.IdUsuario = user.Id;

            var success = await _rHRepository.UpdateViaticos(viaticos);
            if (!success)
            {
                var alertJson = AlertService.ShowAlert(Alerts.Danger, "Ocurrio un error al registrar los datos de vuelo");
                return Ok(alertJson);
            }
            var alert = AlertService.ShowAlert(Alerts.Success, "Se registraron los vuelos con exito");
            return Ok(alert);

        }

        //[HttpPost]
        public async Task<IActionResult> GeneratePDFAsync(int id, string type)
        {

            var viaticos = await _rHRepository.GetSolicitud(id);
            var user = HttpContext.Session.GetObject<UserResponse>("ComplexObject");
            var fileName = viaticos.Folio + ".pdf";
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "RH", id.ToString());
            if (!Directory.Exists(pathPdf))
                ProccessFileTools.CreateDirectory(pathPdf);
            var file = System.IO.Path.Combine(pathPdf, fileName);
            var iHeader = System.IO.Path.Combine(_environment.WebRootPath, "img", "headerConavi.png");
            var iFooter = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdfDoc = new PdfDocument(writer);
            Document doc = new Document(pdfDoc);
            writer.SetCloseStream(false);
            //PdfDocument pdfDoc = new PdfDocument(new PdfWriter(file));
            //Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, iHeader, iFooter));
            //MARGEN DEL DOCUMENTO
            doc.SetMargins(70, 50, 70, 50);
            //Solicitud
            LineSeparator ls = new LineSeparator(new SolidLine());
            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            PdfFont fonts = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
            Paragraph saltoDeLineaa = new Paragraph("                                                                                                                                                                                                                                                                                                                                                                                   ");
            doc.Add(saltoDeLineaa);
            Paragraph comprobacion = new Paragraph("SOLICITUD DE VIÁTICOS Y ANTICIPADOS NO.  ")
                .SetTextAlignment(TextAlignment.LEFT)
                .SetCharacterSpacing(1)
                .SetFont(fonts)
                .SetFontColor(DeviceGray.BLACK)
                .SetFontSize(9);
            doc.Add(comprobacion);
            Paragraph comp = new Paragraph(viaticos.Folio) //NUMERO DE SOLICITUD DE VIATICOS 
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetCharacterSpacing(1)
                 .SetFont(fonts)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetRelativePosition(100, -22, 80, 40)
                 .SetFontSize(9);
            doc.Add(comp);
            Paragraph fecha = new Paragraph("FECHA")
                .SetRelativePosition(-23, -25, 10, 40)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(8)
                .SetFont(fonts)
                .SetFontColor(DeviceGray.BLACK);
            doc.Add(fecha);
            Paragraph fechadato = new Paragraph(viaticos.FechaSol.ToString()) //FECHA DE SOLICITUD 
              .SetFontColor(new DeviceRgb(130, 27, 63))
              .SetCharacterSpacing(1)
              .SetFont(fonts)
              .SetRelativePosition(100, -47, 8, 40)
              .SetTextAlignment(TextAlignment.CENTER)
              .SetFontSize(9);
            doc.Add(fechadato);
            Paragraph datos = new Paragraph("DATOS DEL SOLICITANTE")
                .SetRelativePosition(1, -50, 50, 40)
                ///.SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(9)
                .SetFont(fonts)
                .SetFontColor(DeviceGray.BLACK);
            doc.Add(datos);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            Table puestos = new Table(2, false);
            puestos.SetBorder(Border.NO_BORDER);
            puestos.SetRelativePosition(1, -45, 50, 40);
            Cell cell101 = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonts)
              .SetFontSize(9)
              .SetWidth(0)
              .SetBorder(Border.NO_BORDER)
              .Add(new Paragraph("NOMBRE"));
            Cell cell02 = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFontSize(8)
               .SetBorder(Border.NO_BORDER)
               .SetWidth(500)
               .SetFont(fonts)
               .SetFontColor(new DeviceRgb(130, 27, 63))
               .Add(new Paragraph(viaticos.Nombre)); //NOMBRE DEL SOLICITANTE 
            Cell cell103 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(9)
                .SetBorder(Border.NO_BORDER)
                .SetFont(fonts)
                .Add(new Paragraph("PUESTO"));
            Cell cell104 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFont(fonts)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(10)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .Add(new Paragraph(viaticos.Puesto));//PUESTO DEL SOLICITANTE
            puestos.AddCell(cell101);
            puestos.AddCell(cell02);
            puestos.AddCell(cell103);
            puestos.AddCell(cell104);
            doc.Add(puestos);
            Table puesto = new Table(2, false);
            puesto.SetBorder(Border.NO_BORDER);
            puesto.SetRelativePosition(1, -45, 5, 40);
            Cell cell105 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(9)
                .SetFont(fonts)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("ÁREA DE ADSCRIPCIÓN"));
            Cell cell106 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFont(fonts)
                .SetFontSize(10)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph(viaticos.Area_adscripcion)); //AREA DE ADSCRIPCION 
            puesto.AddCell(cell105);
            puesto.AddCell(cell106);
            doc.Add(puesto);
            Table requerido = new Table(2, false);
            requerido.SetBorder(Border.NO_BORDER);
            requerido.SetRelativePosition(1, -45, 5, 40);
            Cell req = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(9)
                .SetFont(fonts)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("SERVICIO REQUERIDO: "));
            Cell sol = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetFont(fonts)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(9)
                .Add(new Paragraph("SOLICITUD DE VIÁTICOS ANTICIPADOS")); //RERVICIO REQUERIDO
            requerido.AddCell(req);
            requerido.AddCell(sol);
            doc.Add(requerido);
            //CREA TABLA DESCRIPCION DE LA COMISION 
            float[] columnWidths = { 3, 1, 2 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            PdfFont f = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            table.SetRelativePosition(1, -20, 5, 40);
            Cell cell115 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(8)
                .SetFont(fonts)
                .Add(new Paragraph("DESCRIPCIÓN DE LA COMISIÓN"));
            Cell celltext = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(8)
                .SetFont(fonte)
                .Add(new Paragraph(viaticos.Descripcion_comision));  //DESCRIPCION DE LA COMISION 
            Cell cell116 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetFontSize(8)
                .Add(new Paragraph("UNIDAD"));
            Cell celltext2 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontSize(8)
                .Add(new Paragraph(viaticos.Dias_duracion)); //UNIDAD 
            Cell cell117 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetFontSize(8)
                .Add(new Paragraph("CANTIDAD"));
            Cell celltext3 = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonte)
                .SetFontSize(8)
                //CANTIDAD 
                .Add(new Paragraph(viaticos.Cuota_diaria));
            Cell cellsint = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(fonte)
               .SetFontSize(8)
               .SetBorder(Border.NO_BORDER)
               .Add(new Paragraph(""));
            Cell cellmon = new Cell(1, 1)
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFont(fonts)
               .SetFontSize(8)
               .Add(new Paragraph("MONTO TOTAL"));
            Cell celltot = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetFontColor(new DeviceRgb(130, 27, 63))
                .SetFontSize(8)
                .Add(new Paragraph(viaticos.Total_peajes)); //MONTO TOTAL 
            table.AddCell(cell115);
            table.AddCell(cell116);
            table.AddCell(cell117);
            table.AddCell(celltext);
            table.AddCell(celltext2);
            table.AddCell(celltext3);
            table.AddCell(cellsint);
            table.AddCell(cellmon);
            table.AddCell(celltot);
            doc.Add(table);

            //**AgregamosNuevoDocumento**//
            //*--*OficioNuevoDocumento*--*
            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            Paragraph comprobacionOf = new Paragraph("OFICIO DE COMISIÓN DEN DE MINISTRIÓN DE VIÁTICOS Y PASAJES NACIONALES E INTERNAL")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetCharacterSpacing(1)
                .SetFont(fonts)
                .SetFontColor(DeviceGray.BLACK)
                .SetFontSize(8);
            doc.Add(comprobacionOf);
            Table puestosOf = new Table(1, false);
            puestosOf.SetBorder(Border.NO_BORDER);
            puestosOf.SetRelativePosition(220, 5, 50, 40);
            Cell cell101Of = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .SetFontColor(new DeviceRgb(130, 27, 63))
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetWidth(0)
                  .Add(new Paragraph(viaticos.FechaSol)); //FECHA DE ELABORACION
            Cell cell102 = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .SetFont(fonts)
                      .SetWidth(10)
                      .SetHeight(12)
                      .SetBorder(Border.NO_BORDER)
                      .SetFontSize(8)
                      .Add(new Paragraph("FECHA DE ELABORACION"));
            puestosOf.AddCell(cell101Of);
            puestosOf.AddCell(cell102);
            doc.Add(puestosOf);
            Table progresivo = new Table(1, false);
            progresivo.SetBorder(Border.NO_BORDER);
            progresivo.SetRelativePosition(380, -28, 50, 40);
            Cell numero = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .SetFont(fonts)
                      .SetWidth(10)
                      .SetHeight(12)
                      .SetBorder(Border.NO_BORDER)
                      .SetFontSize(8)
                      .SetFontColor(new DeviceRgb(130, 27, 63))
                      .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                      .SetWidth(0)
                      .Add(new Paragraph(viaticos.Folio)); //NUMERO DE FOLIO
             Cell progre = new Cell(1, 1)
                      .SetTextAlignment(TextAlignment.CENTER)
                      .SetFont(fonts)
                      .SetWidth(10)
                      .SetHeight(12)
                      .SetBorder(Border.NO_BORDER)
                      .SetFontSize(8)
                      .Add(new Paragraph("NÚMERO PROGRESIVO"));
            progresivo.AddCell(numero);
            progresivo.AddCell(progre);
            doc.Add(progresivo);
            Paragraph res = new Paragraph("Unidad Responsable:     COMISION NACIONAL DE VIVIENDA ") //UNIDAD RESPONSABLE DE VIATICOS
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetRelativePosition(1, -30, 50, 40)
                    .SetCharacterSpacing(1)
                    .SetFont(fonts)
                    .SetFontColor(DeviceGray.BLACK)
                    .SetFontSize(7);
            doc.Add(res);
            float[] columnWidthsOf = { 2, 4, 1, 5 };
            Table tableOf = new Table(UnitValue.CreatePercentArray(columnWidthsOf));
            tableOf.SetRelativePosition(1, -35, 50, 40);
            Cell cell = new Cell(1, 4)
                .Add(new Paragraph("Datos del comisionado"))
                .SetFont(fonts)
                .SetFontSize(10)
                .SetHeight(12)
                .SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
            tableOf.AddHeaderCell(cell);
            Cell nombre = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonts)
                 .SetWidth(10)
                 .SetHeight(12)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(9)
                 .SetWidth(0)
                 .Add(new Paragraph("Nombre"));
            Cell txtnombre = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonte)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph(viaticos.Nombre)); //NOMBRE DEL COMISIONADO
            Cell rfc = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.RIGHT)
                 .SetFont(fonts)
                 .SetWidth(10)
                 .SetHeight(12)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("RFC"));
            Cell txtrfc = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  //.SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("SICE950711318")); //FALTA ESTE CAMPO PARA SOLICITAR EL RFC DEL SOLICITATE
            //SeccionPuesto
            Cell puestoOf = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .SetWidth(0)
                  .Add(new Paragraph("Puesto o Categoria"));
            Cell txtpuesto = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonte)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph(viaticos.Puesto)); //PUESTO DEL COMISIONADO 
            Cell nivel = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.RIGHT)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("Nivel"));
            Cell txtnivel = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("INGENIERO")); //FALTA CAMPTURA DEL NIVEL COMISIONADO
            //SeccionArea
             Cell Area = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .Add(new Paragraph("Área de Adscripción"));
              Cell txtArea = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonte)
                  .SetWidth(10)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(6)
                  .Add(new Paragraph(viaticos.Area_adscripcion)); //AREA DE ADSCRIPCION 
            tableOf.AddCell(nombre);
            tableOf.AddCell(txtnombre);
            tableOf.AddCell(rfc);
            tableOf.AddCell(txtrfc);
            tableOf.AddCell(puestoOf);
            tableOf.AddCell(txtpuesto);
            tableOf.AddCell(nivel);
            tableOf.AddCell(txtnivel);
            tableOf.AddCell(Area);
            tableOf.AddCell(txtArea);
            doc.Add(tableOf);
            float[] columnWidths1 = { 2, 14 };
            Table table1 = new Table(UnitValue.CreatePercentArray(columnWidths1));
            table1.SetRelativePosition(6, -30, 50, 40);
            Cell motivos = new Cell(1, 2)
                .Add(new Paragraph("Motivo de la Comisión"))
                .SetFont(fonts)
                .SetFontSize(9)
                .SetHeight(12)
                .SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
            table1.AddHeaderCell(motivos);
             Cell objetivo = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFont(fonts)
                .SetHeight(12)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(8)
                .SetWidth(0)
                .Add(new Paragraph("Objetivo"));
              Cell txtobjetivos = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(font)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetHeight(12)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(9)
                 .Add(new Paragraph(viaticos.Objetivo)); //OBJETIVO DE LA COMISION 
              Cell observaciones = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonts)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(9)
                  .Add(new Paragraph("Observaciones"));
              Cell txtobservaciones = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonte)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(9)
                  .Add(new Paragraph(viaticos.Observaciones)); //OBSERVACIONES 
              table1.AddCell(objetivo);
              table1.AddCell(txtobjetivos);
              table1.AddCell(observaciones);
              table1.AddCell(txtobservaciones);
            doc.Add(table1);
            float[] columnWidths2 = { 2, 1, 1, 1, 1, 1 };
            Table lugares = new Table(UnitValue.CreatePercentArray(columnWidths2));
            lugares.SetRelativePosition(-5, -20, 50, 40);
            Cell lugar = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetHeight(12)
                .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                .SetBorder(Border.NO_BORDER)
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetFontSize(7)
                .SetHeight(25f)
                .Add(new Paragraph("LUGARES ASIGNADOS EN LA COMISIÓN"));
             Cell medio = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetHeight(22f)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("MEDIO DE TRANSPORTE"));
             Cell periodo = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetHeight(22f)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("PERIODO DE LA COMISIÓN"));
             Cell dias = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(22f)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph("DÍAS DE DURACIÓN"));
             Cell cuota = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetHeight(22f)
                  .SetBorder(Border.NO_BORDER)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetFontSize(7)
                  .Add(new Paragraph("COUTA DIARIA"));
             Cell importe = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("IMPORTE DE VIÁTICOS"));
             Cell txtlugar = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetHeight(12)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph(viaticos.Lugares_asignados_comision)); //LUGARES ASIGANDOS DE LA COMISION
            Cell txtmedio = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph(viaticos.Medio_transporte)); //MEDIO DE TRANSPORTE 
            Cell txtperiodo = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph(viaticos.Periodo_comision_f)); //PERIODO DE LA COMISION 
            Cell txtdias = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph(viaticos.Dias_duracion)); //DIAS DE DURACION 
            Cell txtcuota = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph(viaticos.Cuota_diaria)); //CUOTA DIARIA   
            Cell txtimporte = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetBorder(Border.NO_BORDER)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetFontSize(7)
                  .Add(new Paragraph(viaticos.Importe_viaticos)); //IMPORTE DE VIATICOS 
            Cell total = new Cell(3, 2)
                  .SetTextAlignment(TextAlignment.RIGHT)
                  .SetFont(fonts)
                  .SetFontSize(7)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .SetBorder(Border.NO_BORDER)
                  .Add(new Paragraph("Total:$"));
            Cell txtimportes = new Cell(5, 3)
                  .SetBorder(Border.NO_BORDER)
                  .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                  .Add(new Paragraph(""));
            Cell total2 = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                  .SetFont(fonts)
                  .SetFontColor(new DeviceRgb(130, 27, 63))
                  .SetFontSize(7)
                  .SetBorder(Border.NO_BORDER)
                  .Add(new Paragraph(viaticos.Total_peajes)); //TOTAL DE VIATICOS 
            lugares.AddCell(lugar);
            lugares.AddCell(medio);
            lugares.AddCell(periodo);
            lugares.AddCell(dias);
            lugares.AddCell(cuota);
            lugares.AddCell(importe);
            lugares.AddCell(txtlugar);
            lugares.AddCell(txtmedio);
            lugares.AddCell(txtperiodo);
            lugares.AddCell(txtdias);
            lugares.AddCell(txtcuota);
            lugares.AddCell(txtimporte);
            lugares.AddCell(txtimportes);
            lugares.AddCell(total);
            lugares.AddCell(total2);
            doc.Add(lugares);
            float[] tama = { 2, 3, 2 };
            Table pasajes = new Table(UnitValue.CreatePercentArray(tama));
            pasajes.SetRelativePosition(1, -25, 50, 40);
            Cell vehiculo = new Cell(1, 4)
                .Add(new Paragraph("CUANDO SE UTILICE VEHÍCULO OFICIAL O PROPIO"))
                .SetFont(fonts)
                .SetFontSize(8)
                .SetHeight(12)
                .SetBorder(Border.NO_BORDER)
                .SetFontColor(DeviceGray.BLACK)
                .SetTextAlignment(TextAlignment.CENTER);
            pasajes.AddHeaderCell(vehiculo);
            Cell gasto = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetWidth(10)
                 .SetHeight(12)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph("GASTO ESTIMADO DE PEAJES"));
            Cell dotacion = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetHeight(30)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(6)
                .Add(new Paragraph("DOTACIÓN DE COMBUSTIBLE SEGÚN FÓRMULA " +
                " (KILOMETRAJE DEL RECORRIDO TOTAL ENTRE CINCO POR EL PRECIO DEL LITRO DE GASOLINA MAGNA)"));
            Cell combustible = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .SetHeight(30)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(7)
                .Add(new Paragraph("IMPORTE DE GASTOS CON CARGO A PASAJES"));
            //SECCIONABAJO
            Cell casetas = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER);
            //Subtabla para numero de casetas y gastos previstos
            Table numcas = new Table(2, false);
            numcas.SetBorder(Border.NO_BORDER);
            Cell cas = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(fonts)
                .SetFontSize(7)
                .SetWidth(70)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("No. de casetas"));
            Cell prev = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(7)
                .SetBorder(Border.NO_BORDER)
                .SetWidth(70)
                .SetFont(fonts)
                .Add(new Paragraph("Gasto previsto"));
            Cell txtcas = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontSize(7)
                 .SetBorder(Border.NO_BORDER)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetFont(fonte)
                 .Add(new Paragraph(viaticos.Num_casetas)); //No. Casetas
            Cell txtprev = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontSize(7)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetBorder(Border.NO_BORDER)
                 .SetFont(fonte)
                 .Add(new Paragraph(viaticos.Importe_gastos)); //gastos previstos ¿CUAL SE LE ASIGNA?
            numcas.AddCell(cas);
            numcas.AddCell(prev);
            numcas.AddCell(txtcas);
            numcas.AddCell(txtprev);
            casetas.Add(numcas);
            Cell txtdotacion = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetWidth(10)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .SetHeight(30)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .Add(new Paragraph(viaticos.Dotacion_combustible)); //DotacionDeCombustible
            //CreacionDeTablaParaPeajes
            Table peaje = new Table(2, false);
            peaje.SetBorder(Border.NO_BORDER);
            Cell peajegas = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetFontSize(7)
                 .SetHeight(30)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetWidth(70)
                 .SetBorder(Border.NO_BORDER)
                 .Add(new Paragraph("Peajes Gasolina"));
            Cell txtpeajegas = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFontSize(7)
                 .SetBorder(Border.NO_BORDER)
                 .SetWidth(70)
                 .SetFontColor(new DeviceRgb(130, 27, 63))
                 .SetHeight(20)
                 .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                 .SetFont(fonts)
                 .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                 .Add(new Paragraph(viaticos.Total_peajes)); //PeajesGasolina
            peaje.AddCell(peajegas);
            peaje.AddCell(txtpeajegas);
            Cell peajes1 = new Cell(1, 1)
            .SetBorder(Border.NO_BORDER);//Aqui se le concatena la tabla peaje
            peajes1.Add(peaje);
            pasajes.AddCell(gasto);
            pasajes.AddCell(dotacion);
            pasajes.AddCell(combustible);
            pasajes.AddCell(casetas);
            pasajes.AddCell(txtdotacion);
            pasajes.AddCell(peajes1);
            doc.Add(pasajes);
            float[] columnWidths22 = { 8, 8, 8, 8, 8, 8 };
            Table vuelos = new Table(UnitValue.CreatePercentArray(columnWidths22));
            vuelos.SetRelativePosition(5, -20, 50, 40);
            Cell solicitud = new Cell(1, 6)
                 .Add(new Paragraph("SOLICITUD DE PASAJES ÁREOS"))
                 .SetFont(fonts)
                 .SetFontSize(8)
                 .SetHeight(12)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontColor(DeviceGray.BLACK)
                 .SetTextAlignment(TextAlignment.CENTER);
            vuelos.AddHeaderCell(solicitud);
            Cell ruta = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonts)
                 .SetWidth(10)
                 .SetHeight(12)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(8)
                 .Add(new Paragraph("RUTA"));
            Cell fechasalida = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph("FECHA DE SALIDA"));
            Cell linea = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph("LÍNEA ÁREA"));
            Cell vuelo = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  //.SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph("VUELO"));
            Cell sale = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(8)
                  .SetWidth(0)
                  .Add(new Paragraph("SALE:"));
            Cell llega = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonts)
                  .SetWidth(10)
                  .SetHeight(12)
                  .SetBorder(Border.NO_BORDER)
                  .SetFontSize(7)
                  .Add(new Paragraph("LLEGA")); 
            Cell txt1 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFont(fonte)
                   .SetWidth(10)
                   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                   .SetHeight(12)
                   .SetBorder(Border.NO_BORDER)
                   .SetFontSize(7)
                   .Add(new Paragraph(viaticos.Ruta_i)); //DESTINO AVION 
            Cell txt2 = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetFont(fonte)
                   .SetWidth(10)
                   .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                   .SetHeight(12)
                   .SetBorder(Border.NO_BORDER)
                   .SetFontSize(7)
                   .Add(new Paragraph(viaticos.Fecha_salida)); //FECHA DE SALIDA    
            Cell txt3 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetWidth(10)
                    .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(viaticos.Linea_aerea)); //AEREOLINEA 
            Cell txt4 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(viaticos.Vuelo_i)); //NUMERO DE VUELO FALTA NUMERO DE VUELO
             Cell txt5 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(viaticos.Sale_i)); //FECHA DE SALIDA DEL VUELO 
             Cell txt6 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(viaticos.Llega_i)); //LLEGADA AL DESTINO 
            //ParteBaja
            Cell rutallegada = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonts)
                     .SetWidth(10)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(8)
                     .SetWidth(0)
                     .Add(new Paragraph("RUTA"));
             Cell fecharegreso = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonts)
                     .SetWidth(10)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph("FECHA DE REGRESO"));
            Cell vueloregreso = new Cell(1, 1)
                     .SetBorder(Border.NO_BORDER)
                     .Add(new Paragraph(" "));
            Cell saleregreso = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonts)
                    .SetWidth(10)
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .SetWidth(0)
                    .Add(new Paragraph("VUELO:"));
            Cell llegaregreso = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonts)
                     .SetWidth(10)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph("SALE"));
            Cell txt11 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonts)
                     .SetWidth(10)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph("LLEGA"));
            Cell txt22 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(viaticos.Ruta_f)); //VUELO DE REGRESO
            Cell txt33 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetWidth(10)
                    .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(viaticos.Fecha_regreso)); //FECHA DE REGRESO 
            Cell txt44 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetHeight(12)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(" "));
            Cell txt55 = new Cell(1, 1)
                     .SetTextAlignment(TextAlignment.CENTER)
                     .SetFont(fonte)
                     .SetWidth(10)
                     .SetHeight(12)
                     .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                     .SetBorder(Border.NO_BORDER)
                     .SetFontSize(7)
                     .Add(new Paragraph(viaticos.Vuelo_f)); //NUMERO DE VUELO FALTA NUMERO DE VUELO DE SALIDA
            Cell txt66 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetWidth(10)
                    .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(viaticos.Sale_f)); //FECHA DE SALIDA
            Cell txt77 = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFont(fonte)
                    .SetWidth(10)
                    .SetBackgroundColor(new DeviceRgb(16, 24, 11), 0.1f)
                    .SetHeight(12)
                    .SetBorder(Border.NO_BORDER)
                    .SetFontSize(7)
                    .Add(new Paragraph(viaticos.Llega_f)); //FECHA DE LLEGAD 
            vuelos.AddCell(ruta);
            vuelos.AddCell(fechasalida);
            vuelos.AddCell(linea);
            vuelos.AddCell(vuelo);
            vuelos.AddCell(sale);
            vuelos.AddCell(llega);
            vuelos.AddCell(txt1);   
            vuelos.AddCell(txt2);
            vuelos.AddCell(txt3);
            vuelos.AddCell(txt4);
            vuelos.AddCell(txt5);
            vuelos.AddCell(txt6);
            vuelos.AddCell(rutallegada);
            vuelos.AddCell(fecharegreso);
            vuelos.AddCell(vueloregreso);
            vuelos.AddCell(saleregreso);
            vuelos.AddCell(llegaregreso);
            vuelos.AddCell(txt11);
            vuelos.AddCell(txt22);
            vuelos.AddCell(txt33);
            vuelos.AddCell(txt44);
            vuelos.AddCell(txt55);
            vuelos.AddCell(txt66);
            vuelos.AddCell(txt77);
            doc.Add(vuelos);


            //**AgregamosNuevoDocumentoTARJETACOMISION**//
            //Agregamos la Tarjeta de comision
            doc.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
            Paragraph saltoDeLineaT = new Paragraph("                                                                                                                                                                                                                                                                                                                                                                                   ");
            doc.Add(saltoDeLineaT);
            Paragraph subheader = new Paragraph("TARJETA DE COMISIÓN")
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFont(fonte)
                  .SetCharacterSpacing(1)
                  .SetFontColor(DeviceGray.BLACK)
                  .SetFontSize(12);
            doc.Add(subheader);
            Paragraph fechaT = new Paragraph("Ciudad De México a " + viaticos.FechaSol) //FECHA DE ELABORACION
                  .SetTextAlignment(TextAlignment.CENTER)
                  .SetFontColor(new DeviceRgb(130, 27, 63))
                  .SetCharacterSpacing(1)
                  .SetRelativePosition(150, 12, 80, 40)
                  .SetFontSize(9);
            doc.Add(fechaT);
            doc.Add(saltoDeLineaT);
            doc.Add(saltoDeLineaT);
            doc.Add(saltoDeLineaT);
            Table puestosT = new Table(2, false);
            puestosT.SetBorder(Border.NO_BORDER);
            Cell cell101T = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFont(fonts)
                  .SetFontSize(10)
                  .SetWidth(0)
                  .SetBorder(Border.NO_BORDER)
                  .Add(new Paragraph("DE:"));
            Cell cell102T = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFontSize(9)
                  .SetBorder(Border.NO_BORDER)
                  .SetWidth(500)
                  .SetFontColor(new DeviceRgb(130, 27, 63))
                  .SetFont(fonte)
                  .Add(new Paragraph("NOMBRE SUBDIRECTOR GENERAL QUE COMISIONA "));  //NOMBRE SUBDIRECTOR 
            Cell cell103T = new Cell(1, 1)
                  .SetTextAlignment(TextAlignment.LEFT)
                  .SetFontSize(10)
                  .SetBorder(Border.NO_BORDER)
                  .SetFont(fonts)
                  .Add(new Paragraph("PUESTO: "));
            Cell cell104T = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(fonte)
                   .SetFontColor(new DeviceRgb(130, 27, 63))
                   .SetFontSize(9)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph("SUBDIRECTOR DE AREA")); //PUESTO DEL SUBDIRECTOR 
            Cell cell105T = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(10)
                   .SetFont(fonts)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph("PARA:"));
            Cell cell106T = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(fonte)
                   .SetFontColor(new DeviceRgb(130, 27, 63))
                   .SetFontSize(9)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(viaticos.Nombre)); //NOMBRE DEL COMISIONADO 
            Cell cell1071T = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(fonts)
                   .SetFontSize(10)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph("PUESTO:"));
            Cell cell107T = new Cell(1, 1)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFont(fonte)
                   .SetFontColor(new DeviceRgb(130, 27, 63))
                   .SetFontSize(9)
                   .SetBorder(Border.NO_BORDER)
                   .Add(new Paragraph(viaticos.Puesto)); //NOMBRE DEL PUESTO 
                    puestosT.AddCell(cell101T);
                    puestosT.AddCell(cell102T);
                    puestosT.AddCell(cell103T);
                    puestosT.AddCell(cell104T);
                    puestosT.AddCell(cell105T);
                    puestosT.AddCell(cell106T);
                    puestosT.AddCell(cell1071T);
                    puestosT.AddCell(cell107T);
                    doc.Add(puestosT);
                    doc.Add(saltoDeLineaT);
                    doc.Add(saltoDeLineaT);
                    doc.Add(saltoDeLineaT);
                    doc.Add(saltoDeLineaT);
            //COMISION 
            Paragraph primer = new Paragraph("Sirva el presente para informar que he tenido a bien comisionar " + viaticos.Descripcion_comision)
                     .SetTextAlignment(TextAlignment.JUSTIFIED_ALL)
                     .SetFontColor(DeviceGray.BLACK)
                     .SetFontSize(11);
            doc.Add(primer);
            Paragraph segundo = new Paragraph("Lo anterior con el fin de que lleva cabo las rlque he tenido a bien comisionarlo que he tenido " + viaticos.Objetivo)
                   .SetTextAlignment(TextAlignment.JUSTIFIED_ALL)
                   .SetFontColor(DeviceGray.BLACK)
                   .SetFontSize(11);
            doc.Add(segundo);
            doc.Close();
            byte[] byteInfo = ms.ToArray();
            ms.Write(byteInfo, 0, byteInfo.Length);
            ms.Position = 0;
            FileStreamResult fileStreamResult = new FileStreamResult(ms, "application/pdf");

            //Uncomment this to return the file as a download
            //fileStreamResult.FileDownloadName = "Output.pdf";

            return fileStreamResult;
            //return RedirectToAction("Index");
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
