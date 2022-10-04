using ConaviWeb.Data.Reporteador;
using ConaviWeb.Model.Reporteador;
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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Document = iText.Layout.Document;
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfFont = iText.Kernel.Font.PdfFont;
using PdfWriter = iText.Kernel.Pdf.PdfWriter;
using Rectangle = iText.Kernel.Geom.Rectangle;

namespace ConaviWeb.Controllers.Reporteador
{
    public class CartaBAPDFController : Controller
    {
        private readonly IReporteadorRepository _reporteadorRepository;
        private readonly IWebHostEnvironment _environment;
        public CartaBAPDFController(IWebHostEnvironment environment, IReporteadorRepository reporteadorRepository)
        {
            _environment = environment;
            _reporteadorRepository = reporteadorRepository;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var benefs = await _reporteadorRepository.GetCartasBA(5);
            foreach (var benef in benefs)
            {
                ManipulatePdf(benef);
            }

            return Ok();

        }

        public void ManipulatePdf(PevCartaBA benef)
        {
            var _header = System.IO.Path.Combine(_environment.WebRootPath, "img", "magon.png");
            var _footer = System.IO.Path.Combine(_environment.WebRootPath, "img", "footerConavi.png");
            var pathCarta = Path.Combine(_environment.WebRootPath, "doc", "CartasPEV", benef.Path_carta,benef.Estado, benef.Municipio);
            if (!Directory.Exists(pathCarta))
            {
                Directory.CreateDirectory(pathCarta);
            }
            PdfDocument pdfDoc = new PdfDocument(new PdfWriter(Path.Combine(pathCarta, benef.Id_unico+".pdf")));
            Document doc = new Document(pdfDoc);
            pdfDoc.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(doc, _header, _footer));





            doc.SetMargins(60, 40, 70, 40); //nuevo margen
            PdfFont cursiva = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont Bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);


            LineSeparator ls = new LineSeparator(new SolidLine());
            PdfFont fonte = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN);
            Paragraph saltoDeLineaa = new Paragraph("");
            doc.Add(saltoDeLineaa);

            Paragraph entidadCarta = new Paragraph(benef.Edompo)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(new DeviceRgb(152, 56, 39))
                .SetFont(Bold)
                .SetFixedPosition(1, 285, 820, 300)
                .SetCharacterSpacing(1)
                .SetFontSize(10);
            doc.Add(entidadCarta);

            float[] tamFirma = { 1, 5, 5 };
            Table puestos = new Table(UnitValue.CreatePercentArray(tamFirma)).SetMarginTop(20);
            Cell datoVoid = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.RIGHT)
                .SetBorder(Border.NO_BORDER)
                .SetFontSize(8);

            Cell datoINE = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(8)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Datos CURP"));
            Cell datoCURP = new Cell(1, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(8)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Datos INE"));
            Cell cell101 = new Cell(2, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(8)
                .SetWidth(20)
                .SetHeight(17)
                .SetBorder(Border.NO_BORDER)
                .Add(new Paragraph("Titular:"));
            int fuente = 8;
            if (benef.Nombre_curp.Length > 30)
            {
                fuente = 6;
            }
            Paragraph pCurp = new Paragraph(benef.Nombre_curp);
            Paragraph pIne = new Paragraph(benef.Nombre_ine);
            Cell cellNombre = new Cell(2, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(fuente)
                .SetBorder(Border.NO_BORDER)
                .SetCharacterSpacing(2)
                .SetPaddingLeft(10)
                .SetHeight(17)
                //.SetFontColor(new DeviceRgb(0, 191, 255))
                .SetFont(Bold)
                .Add(pCurp);
            pCurp.SetProperty(Property.OVERFLOW_X, OverflowPropertyValue.VISIBLE);//NOMBRESOLICITANTE
            Cell cellNombre2 = new Cell(2, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(fuente)
                .SetBorder(Border.NO_BORDER)
                .SetCharacterSpacing(2)
                .SetHeight(17)
                //.SetFontColor(new DeviceRgb(0, 191, 255))
                .SetFont(Bold)
                .Add(pIne);
            pIne.SetProperty(Property.OVERFLOW_X, OverflowPropertyValue.VISIBLE);//NOMBRESOLICITANTE//NOMBRESOLICITANTE
            Cell cell103 = new Cell(3, 1)
                .SetTextAlignment(TextAlignment.LEFT)
                .SetFontSize(8)
                .SetBorder(Border.NO_BORDER)
                .SetHeight(17)
                .Add(new Paragraph("CURP:"));
            Cell cell104 = new Cell(3, 1)
                 //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetBorder(Border.NO_BORDER)
                 .SetFont(Bold)
                 .SetHeight(17)
                 .SetPaddingLeft(10)
                 .SetCharacterSpacing(2)
                 .SetFontSize(8)
                 .Add(new Paragraph(benef.Curp));
            puestos.AddCell(datoVoid);
            puestos.AddCell(datoINE);
            puestos.AddCell(datoCURP);
            puestos.AddCell(cell101);
            puestos.AddCell(cellNombre);
            puestos.AddCell(cellNombre2);
            puestos.AddCell(cell103);
            puestos.AddCell(cell104);
            doc.Add(puestos);

            Paragraph fecha = new Paragraph("Fecha:")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(1, 180, 680, 150)
                .SetCharacterSpacing(1)
                .SetFontColor(DeviceGray.BLACK)
                .SetFontSize(8);
            doc.Add(fecha);

            Paragraph dataFecha = new Paragraph(benef.Fecha)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFixedPosition(1, 270, 680, 150)
                .SetCharacterSpacing(1)
                .SetBold()
                .SetFontColor(DeviceGray.BLACK)
                .SetFontSize(8);
            doc.Add(dataFecha);

            String code = benef.Id_unico;
            Barcode128 code128 = new Barcode128(pdfDoc);
            code128.SetFont(null);
            code128.SetCode(code);
            code128.SetCodeType(Barcode128.CODE128);
            Image code128Image = new Image(code128.CreateFormXObject(pdfDoc)).ScaleToFit(40, 20).SetFixedPosition(1, 450, 690, 150);


            doc.Add(code128Image);

            Paragraph folio = new Paragraph(benef.Id_unico)
               .SetTextAlignment(TextAlignment.LEFT)
               .SetFixedPosition(1, 455, 670, 150)
               .SetCharacterSpacing(1)
               .SetFont(Bold)
               .SetFontColor(DeviceGray.BLACK)
               .SetFontSize(11);
            doc.Add(folio);


            Paragraph textomedio = new Paragraph("Este programa tiene como propósito proteger al pueblo y ayudarnos a superar el reto económico que enfrentamos. Con estas acciones vamos a impulsar la industria de la construcción, que genera en corto tiempo muchos empleos y contribuye a reactivar la economía de las ciudades, pueblos y comunidades. Y al mismo tiempo vamos a dar atención a una de las principales necesidades de vivienda de las familias de más bajos ingresos y que carecen de seguridad social. Rescatando primero a los que menos tienen, estamos seguros de que las cosas van a salir bien, pero necesitamos la participación de todos, haciendo cada quien lo que le corresponde y en la medida de sus posibilidades, ayudar a los demás. Los recursos para estos apoyos provienen del presupuesto público, de lo que todos aportamos, y son para ayudar a quienes más lo necesitan, sin ningún pago.Por eso te pedimos que lo utilices de manera responsable, para que tú y tu familia tengan una mejor vivienda, más segura, más cómoda y más bonita.")
            .SetTextAlignment(TextAlignment.JUSTIFIED)
            .SetFixedPosition(1, 43, 550, 500)
            .SetCharacterSpacing(1)
            .SetFont(cursiva)
            .SetFontColor(DeviceGray.BLACK)
            .SetFontSize(8);
            doc.Add(textomedio);

            Paragraph gMexico = new Paragraph("GOBIERNO DE MÉXICO")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFixedPosition(1, 170, 525, 250)
           .SetCharacterSpacing(1)
           .SetFont(Bold)
           .SetFontColor(DeviceGray.BLACK)
           .SetFontSize(12);
            doc.Add(gMexico);




            float[] columnWidths = { 1, 7 };
            Table table = new Table(UnitValue.CreatePercentArray(columnWidths));
            table.SetWidth(450);
            table.SetFixedPosition(1, 50, 190, 500);

            Cell cell = new Cell(1, 4)
            .Add(new Paragraph("INSTRUCCIONES PARA ACTIVAR LA CUENTA Y DISPONER DE LOS RECURSOS:"))
            .SetFont(Bold)
            .SetFontSize(10)
            .SetFontColor(new DeviceRgb(152, 56, 39))
            .SetBorder(Border.NO_BORDER)
            .SetPaddingBottom(10)
            .SetTextAlignment(TextAlignment.CENTER);
            table.AddHeaderCell(cell);

            Cell nombre = new Cell(2, 1)
             .SetTextAlignment(TextAlignment.CENTER)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(152, 56, 39))
             .SetBorder(Border.NO_BORDER)
             .SetFontSize(14)
             .SetFont(Bold)
             .Add(new Paragraph("1"));

            Cell txtnombre = new Cell(2, 1)
              .SetTextAlignment(TextAlignment.JUSTIFIED)
              .SetFont(fonte)
              .SetWidth(10)
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(10)
              .SetPaddingBottom(10)
              .SetPaddingRight(10)
             .Add(new Paragraph("Para que la cuenta bancaria del programa sea activada te pedimos te presentes el día y hora que indica tu cita en la sucursal que te haya sido asignada.")); //NOMBRE DEL COMISIONADO
            Cell rfc = new Cell(3, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 //     .SetWidth(10)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
               .SetBorder(Border.NO_BORDER)
                 .SetFontSize(14)
             .SetFont(Bold)
                 .Add(new Paragraph("2"));

            Cell txtrfc = new Cell(3, 1)
              .SetTextAlignment(TextAlignment.JUSTIFIED)
              .SetFont(fonte)
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(10)
              .SetPaddingBottom(10)
              .SetPaddingRight(10)
             .Add(new Paragraph("Es necesario que la persona titular de la cuenta se presente al banco con esta carta, su credencial de elector original vigente y su comprobante de domicilio vigente (no mayor a 3 meses).")); //RFC DEL COMISIONADO 

            Cell puesto = new Cell(4, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(14)
             .SetFont(Bold)
                 .Add(new Paragraph("3"));

            Cell txtpuesto = new Cell(4, 1)
              .SetTextAlignment(TextAlignment.JUSTIFIED)
              .SetFont(fonte)
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(10)
              .SetPaddingBottom(10)
              .SetPaddingRight(10)
              .Add(new Paragraph("Una vez que haya abierto su cuenta, podrá ver el monto del subsidio autorizado y disponer del mismo.")); //PUESTO DEL COMISIONADO 

            Cell nivel = new Cell(5, 1)
                 .SetTextAlignment(TextAlignment.CENTER)
                 .SetFont(fonte)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(14)
             .SetFont(Bold)
                 .Add(new Paragraph("4"));

            Cell txtnivel = new Cell(5, 1)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(10)
              .SetPaddingBottom(10)
              .SetPaddingRight(10)
              .Add(new Paragraph("Los datos con los que se te dará de alta en el Banco son los siguientes:")); //NIVEL DEL COMISIONADO 

            Cell final = new Cell(6, 2)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetBorder(Border.NO_BORDER)
              .SetFontSize(10)
              .SetPaddingBottom(10)
              .SetPaddingRight(10)
              .SetHeight(140);
            //.Add(new Paragraph("FINAL")); //NIVEL DEL COMISIONADO 


            table.AddCell(nombre);
            table.AddCell(txtnombre);
            table.AddCell(rfc);
            table.AddCell(txtrfc);
            table.AddCell(puesto);
            table.AddCell(txtpuesto);
            table.AddCell(nivel);
            table.AddCell(txtnivel);
            table.AddCell(final);
            table.SetNextRenderer(new TableBorderRenderer(table));
            doc.Add(table);



            float[] abajo = { 1, 3, 2, 3 };
            Table table1 = new Table(UnitValue.CreatePercentArray(abajo));
            table1.SetWidth(450);
            table1.SetFixedPosition(1, 80, 218, 450);


            Cell nombre1 = new Cell(1, 2)
             .SetTextAlignment(TextAlignment.LEFT)
             .SetFont(fonte)
             .SetFontColor(new DeviceRgb(152, 56, 39))
             .SetFontSize(5)
             .SetPaddingLeft(10)
             .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph("DATOS GENERALES DE LA PERSONA BENEFICIARIA"));

            Cell txtnombre1 = new Cell(1, 2)
              .SetTextAlignment(TextAlignment.RIGHT)
              .SetFont(Bold)
              .SetFontSize(6)
              .SetPaddingRight(25)
              .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph("Preséntate con credencial INE vigente, en la siguiente dirección, fecha y hora.")); //NOMBRE DEL COMISIONADO
            Cell cuenta = new Cell(2, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetFontSize(7)
                 .SetPaddingLeft(10)
                 .SetFont(Bold)
                 .SetBorder(Border.NO_BORDER)
                  .SetFontColor(new DeviceRgb(152, 56, 39))
                 .Add(new Paragraph("Referencia:"));
            Cell nuCuenta = new Cell(2, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetFont(Bold)
                 .SetFontSize(7)
                 .SetBorder(Border.NO_BORDER)
                  //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph(benef.Referencia));

            
            


            Cell sucursal = new Cell(2, 1)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetFontSize(7)
              .SetPaddingLeft(10)
              .SetFont(Bold)
              .SetBorder(Border.NO_BORDER)
              .SetFontColor(new DeviceRgb(152, 56, 39))
             .Add(new Paragraph("Sucursal del Banco de Azteca:")); //RFC DEL COMISIONADO 

            Cell dSucursal = new Cell(2, 2)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetFontSize(7)
              .SetFont(Bold)
              .SetPaddingRight(25)
              .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph(benef.Sucursal));

            Cell emisora = new Cell(3, 1)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetFontSize(7)
              .SetPaddingLeft(10)
              .SetFont(Bold)
              .SetBorder(Border.NO_BORDER)
              .SetFontColor(new DeviceRgb(152, 56, 39))
             .Add(new Paragraph("Emisora:"));

            Cell descEmisora = new Cell(3, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetFont(Bold)
                 .SetFontSize(7)
                 .SetBorder(Border.NO_BORDER)
                  //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph(benef.Emisora));

            Cell dirSucursal = new Cell(3, 4)
              .SetTextAlignment(TextAlignment.LEFT)
              .SetFont(fonte)
              .SetFontSize(7)
              .SetPaddingLeft(10)
              .SetFont(Bold)
              //.SetFontColor(new DeviceRgb(0, 191, 255))
              .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph(benef.Colonia_sucursal));

            Cell lineA = new Cell(4, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .SetPaddingLeft(10)
                 .SetFont(Bold)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
                 .Add(new Paragraph("Línea de apoyo:"));

            Cell dApoyo = new Cell(4, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetFont(Bold)
                 .SetFontSize(7)
                 .SetBorder(Border.NO_BORDER)
                 //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph(benef.Tipo_apoyo));

            Cell monto = new Cell(5, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetWidth(10)
                 .SetFont(Bold)
                 .SetPaddingLeft(10)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
                 .Add(new Paragraph("Monto:"));

            Cell nuMonto = new Cell(5, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetWidth(7)
                 .SetFont(Bold)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph("$ "+benef.Monto));

            //
            Cell dir = new Cell(6, 1)
           .SetTextAlignment(TextAlignment.LEFT)
           .SetFont(fonte)
           .SetWidth(10)
           .SetFont(Bold)
           .SetPaddingLeft(10)
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(7)
           .SetFontColor(new DeviceRgb(152, 56, 39))
           .Add(new Paragraph("Dirección:"));

            Cell dirDato = new Cell(6, 3)
           .SetTextAlignment(TextAlignment.LEFT)
           .SetFont(fonte)
           .SetWidth(10)
           .SetFont(Bold)
           .SetBorder(Border.NO_BORDER)
           .SetFontSize(7)
           //.SetFontColor(new DeviceRgb(0, 191, 255))
           .Add(new Paragraph(benef.Direccion));

            Cell telefono = new Cell(7, 1)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetFont(fonte)
            .SetBorder(Border.NO_BORDER)
            .SetFont(Bold)
            .SetPaddingLeft(10)
            .SetFontSize(7)
            .SetFontColor(new DeviceRgb(152, 56, 39))
            .Add(new Paragraph("Teléfonos:")); //PUESTO DEL COMISIONADO 

            Cell telDato = new Cell(7, 1)
            .SetTextAlignment(TextAlignment.LEFT)
            .SetFont(fonte)
            .SetBorder(Border.NO_BORDER)
            .SetFontSize(7)
            .SetFont(Bold)
            //.SetFontColor(new DeviceRgb(0, 191, 255))
            .Add(new Paragraph(benef.Telefonos)); //PUESTO DEL COMISIONADO 

            Cell horaatencion = new Cell(7, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetWidth(7)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 .SetPaddingLeft(10)
                 .SetFont(Bold)
                 .SetFontColor(new DeviceRgb(152, 56, 39))
                 .Add(new Paragraph("Fecha y hora de atención:"));

            Cell atencion = new Cell(7, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetWidth(7)
                 .SetFont(Bold)
                 .SetBorder(Border.NO_BORDER)
                 .SetFontSize(7)
                 //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph(benef.Fecha_atencion + " de " + benef.Hora_atencion_inicio + " a " + benef.Hora_atencion_fin));



            table1.AddCell(nombre1);
            table1.AddCell(txtnombre1);
            table1.AddCell(cuenta);
            table1.AddCell(nuCuenta);
            table1.AddCell(sucursal);
            table1.AddCell(dSucursal);
            table1.AddCell(emisora);
            table1.AddCell(descEmisora);
            table1.AddCell(dirSucursal);
            table1.AddCell(lineA);
            table1.AddCell(dApoyo);
            table1.AddCell(monto);
            table1.AddCell(nuMonto);
            table1.AddCell(dir);
            table1.AddCell(dirDato);
            table1.AddCell(telefono);
            table1.AddCell(telDato);
            table1.AddCell(horaatencion);
            table1.AddCell(atencion);
            table1.SetNextRenderer(new TableBorderRenderer(table1));
            doc.Add(table1);


            float[] tableCol = { 2, 1 };
            Table tableTel = new Table(UnitValue.CreatePercentArray(tableCol));
            tableTel.SetWidth(450);
            tableTel.SetFixedPosition(1, 65, 195, 500);


            Cell asociado = new Cell(1, 1)
         .SetTextAlignment(TextAlignment.LEFT)
         .SetFont(fonte)
         .SetBorder(Border.NO_BORDER)
         .SetFontSize(10)
         .SetPaddingLeft(15)
         .Add(new Paragraph("El número telefónico asociado a tu cuenta será:"));

            Cell tel = new Cell(1, 1)
                 .SetTextAlignment(TextAlignment.LEFT)
                 .SetFont(fonte)
                 .SetFontSize(11)
                 .SetBold()
                 .SetCharacterSpacing(9)
                 .SetPaddingRight(40)
                 .SetBorder(Border.NO_BORDER)
                 //.SetFontColor(new DeviceRgb(0, 191, 255))
                 .Add(new Paragraph(benef.Telefono).SetUnderline(1.5f, -4));


            tableTel.AddCell(asociado);
            tableTel.AddCell(tel);
            doc.Add(tableTel);


            float[] izquierda = { 1, 4 };
            Table izquierdas = new Table(UnitValue.CreatePercentArray(izquierda));

            izquierdas.SetFixedPosition(1, 50, 45, 50);



            Cell cinco = new Cell(1, 1)
             .SetTextAlignment(TextAlignment.LEFT)
             .SetFont(fonte)
             .SetPaddingLeft(28)
             .SetFontColor(new DeviceRgb(152, 56, 39))
             .SetFontSize(14)
             .SetBorder(Border.NO_BORDER)
             .SetFont(Bold)
             .Add(new Paragraph("5"));

            Cell txtcinco = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.JUSTIFIED)
              .SetFont(fonte)
              .SetFontSize(6)
              .SetPaddingLeft(10)
              .SetMaxWidth(250)
              .SetBorder(Border.NO_BORDER)
              .SetHeight(130)
             .Add(new Paragraph("Manifiesto que he verificado que mis datos son correctos y verídicos, ratifico que los proporcioné de manera personal y directa. Autorizo expresamente su inclusión en el padrón que determine la dependencia federal correspondiente. Asimismo, manifiesto tener conocimiento del aviso simplificado de privacidad señalado en el sitio https://www.conavi.gob.mx/gobmx/datos_personales/doc/Aviso%20de%20Privacidad%20Simplificado%20PEV%202022.pdf, y autorizo a que el Gobierno Federal me pueda contactar para avisos relacionados con mi bienestar y las actividades del mismo. Acredito mi inclusión en el Programa antes señalado, obligándome a dar cumplimiento a la normatividad que le sea aplicable, aceptando el carácter personal e intransferible del mismo y comprometiéndome a darle un uso responsable y conforme a los Lineamientos o Reglas de Operación del Programa disponible en: https://www.conavi.gob.mx/images/documentos/normateca/2022/LINEAMIENTOS%20PEV%202022-MOD.pdf Autorizo a que la institución bancaria me identifique mediante NIP, número de trámite u orden de pago. Es de mi conocimiento que, en su caso, puedo consultar el contrato de apertura a través de la página de internet de la institución bancaria correspondiente y acepto los términos y condiciones del mismo.")); //NOMBRE DEL COMISIONADO

            izquierdas.AddCell(cinco);
            izquierdas.AddCell(txtcinco);
            doc.Add(izquierdas);




            Table firma2 = new Table(1, false);
            firma2.SetBorder(Border.NO_BORDER);
            firma2.SetWidth(500);
            firma2.SetFixedPosition(1, 370, 60, 180);
            Cell areatxt2 = new Cell(1, 1)
                .SetWidth(250)
               .SetHeight(80); //AreaParaFirma
            Cell txtDatoServ = new Cell(1, 1)
              .SetTextAlignment(TextAlignment.JUSTIFIED)
              .SetFont(fonte)
              .SetFontSize(5)
              .SetBorder(Border.NO_BORDER)
             .Add(new Paragraph("ACEPTO SER PARTE DEL PROGRAMA Y, EN EL MISMO ACTO, RECIBO EL MANUAL DE AUTOCONSTRUCCIÓN Y EL TRÍPTICO SOBRE EL PROGRAMA")); //NOMBRE DEL COMISIONADO

            Cell nombreservido2 = new Cell(2, 1)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFont(Bold)

                .SetFontSize(7)
                .SetFontColor(new DeviceRgb(152, 56, 39))
                .SetHeight(10)
                .SetWidth(130)
                .SetVerticalAlignment((VerticalAlignment.MIDDLE))
                .Add(new Paragraph("NOMBRE Y FIRMA DE LA PERSONA BENEFICIARIA")); //NombreServidorPublico
                                                                                  //
            firma2.AddCell(txtDatoServ);
            firma2.AddCell(areatxt2);
            firma2.AddCell(nombreservido2);
            firma2.SetNextRenderer(new TableBorderRenderer(firma2));

            doc.Add(firma2);
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
                .SetMarginTop(20)
                .SetMarginLeft(25)
                .ScaleToFit(540, 520)
                .SetTextAlignment(TextAlignment.CENTER);
                canvas.Add(img);
                Color goldColor = new DeviceRgb(218, 165, 32);
                SolidLine line = new SolidLine(3f);
                line.SetColor(goldColor);
                LineSeparator ls = new LineSeparator(line);
                ls.SetMarginTop(755);
                canvas.Add(ls);
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
    }
}
