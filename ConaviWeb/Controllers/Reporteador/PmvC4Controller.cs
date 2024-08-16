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
               "2404026571",
"2404027488",
"2404027818",
"2404028634",
"2404034616",
"2404036085",
"2404036592",
"2404037051",
"2404037411",
"2404050965",
"2404054160",
"2404059900",
"2404061638",
"2404009534",
"2404028914",
"2404035350",
"2404036569",
"2404036570",
"2404036909",
"2404037407",
"2404041758",
"2404042005",
"2404042014",
"2404050526",
"2404051183",
"2404053333",
"2404004667",
"2404004046",
"2404004051",
"2404005577",
"2404036870",
"2404040607",
"2404040636",
"2404040850",
"2404040855",
"2404041499",
"2404045439",
"2404045554",
"2404047795",
"2404038949",
"2404040489",
"2404040614",
"2404040826",
"2404040837",
"2404040853",
"2404040860",
"2404041366",
"2404042994",
"2404043026",
"2404043405",
"2404045102",
"2404045103",
"2404045430",
"2404045434",
"2404045438",
"2404046461",
"2404013189",
"2404013299",
"2404013325",
"2404013356",
"2404013372",
"2404013373",
"2404013374",
"2404013375",
"2404013376",
"2404013404",
"2404013499",
"2404013500",
"2404013501",
"2404013505",
"2404013506",
"2404013676",
"2404013679",
"2404013685",
"2404013689",
"2404013752",
"2404013754",
"2404013755",
"2404013757",
"2404013758",
"2404013822",
"2404013825",
"2404002784",
"2404028551",
"2404043413",
"2404050695",
"2404067338",
"2404070197",
"2404070778",
"2404070812",
"2404073823",
"2404030217",
"2404073820",
"2404014826",
"2404040581",
"2404055132",
"2404004245",
"2404026823",
"2404035738",
"2404043507",
"2404002189",
"2404002304",
"2404002312",
"2404002353",
"2404002354",
"2404002364",
"2404003004",
"2404003022",
"2404003028",
"2404003403",
"2404003852",
"2404004084",
"2404004393",
"2404004398",
"2404005628",
"2404005629",
"2404006056",
"2404007993",
"2404007997",
"2404002917",
"2404003845",
"2404004099",
"2404005271",
"2404006127",
"2404006429",
"2404008919",
"2404010716",
"2404052646",
"2404006833",
"2404006892",
"2404007687",
"2404007802",
"2404007855",
"2404007860",
"2404016879",
"2404016892",
"2404016912",
"2404016913",
"2404016914",
"2404016915",
"2404017280",
"2404034990",
"2404034997",
"2404035392",
"2404041002",
"2404011502",
"2404025850",
"2404030032",
"2404030035",
"2404030040",
"2404030041",
"2404030122",
"2404030146",
"2404030148",
"2404030159",
"2404031228",
"2404031390",
"2404032226",
"2404033304",
"2404033315",
"2404033316",
"2404033317",
"2404033321",
"2404033326",
"2404033328",
"2404035227",
"2404035233",
"2404035237",
"2404035540",
"2404036253",
"2404036269",
"2404034741",
"2404038384",
"2404038388",
"2404038392",
"2404038396",
"2404038398",
"2404038400",
"2404038402",
"2404038403",
"2404038924",
"2404039860",
"2404041063",
"2404041086",
"2404041209",
"2404043502",
"2404043515",
"2404043539",
"2404043570",
"2404043875",
"2404043897",
"2404044016",
"2404044350",
"2404044358",
"2404044384",
"2404044419",
"2404045403",
"2404016283",
"2404016286",
"2404016447",
"2404024614",
"2404026548",
"2404029769",
"2404029943",
"2404029961",
"2404030422",
"2404030655",
"2404030657",
"2404035525",
"2404036483",
"2404039752",
"2404043406",
"2404045359",
"2404046990",
"2404049752",
"2404050436",
"2404052289",
"2404055214",
"2404055386",
"2404056063",
"2404061016",
"2404062612",
"2404062615",
"2404062620",
"2404063483",
"2404065826",
"2404065929",
"2404066150",
"2404070546",
"2404070974",
"2404074346",
"2404022078",
"2404029937",
"2404035222",
"2404039753",
"2404039909",
"2404039915",
"2404039917",
"2404040604",
"2404040605",
"2404046458",
"2404046758",
"2404046804",
"2404046981",
"2404051366",
"2404051432",
"2404052060",
"2404056473",
"2404058553",
"2404004417",
"2404005842",
"2404009074",
"2404010954",
"2404015589",
"2404015630",
"2404017135",
"2404019415",
"2404019652",
"2404020382",
"2404020399",
"2404020499",
"2404020502",
"2404020536",
"2404020894",
"2404023324",
"2404023355",
"2404023832",
"2404023900",
"2404024168",
"2404024262",
"2404025647",
"2404025650",
"2404025654",
"2404025660",
"2404025662",
"2404012955",
"2404012957",
"2404012958",
"2404012959",
"2404012996",
"2404020565",
"2404020794",
"2404045776",
"2404020038",
"2404020111",
"2404020150",
"2404020154",
"2404020156",
"2404020163",
"2404020308",
"2404020562",
"2404020756",
"2404025199",
"2404025996",
"2404028547",
"2404030523",
"2404030525",
"2404034121",
"2404036996",
"2404037049",
"2404037509",
"2404025368",
"2404039213",
"2404005181",
"2404006339",
"2404015478",
"2404015818",
"2404018664",
"2404019223",
"2404020676",
"2404023117",
"2404023122",
"2404024966",
"2404025351",
"2404025909",
"2404026293",
"2404026922",
"2404029258",
"2404029363",
"2404029388",
"2404031537",
"2404032735",
"2404032740",
"2404036533",
"2404040148",
"2404045583",
"2404046581",
"2404048199",
"2404054498",
"2404076245",
"2404001293",
"2404001307",
"2404001309",
"2404013484",
"2404020976",
"2404021027",
"2404021057",
"2404023780",
"2404033331",
"2404006922",
"2404042706",
"2404001288",
"2404001290",
"2404003173",
"2404003200",
"2404003426",
"2404008945",
"2404013485",
"2404013816",
"2404021181",
"2404023781",
"2404028497",
"2404065939",
"2404003992",
"2404004003",
"2404004135",
"2404004136",
"2404004140",
"2404004145",
"2404004149",
"2404004307",
"2404004315",
"2404004318",
"2404004364",
"2404004370",
"2404006843",
"2404006845",
"2404007214",
"2404007219",
"2404007262",
"2404007263",
"2404007265",
"2404007266",
"2404007268",
"2404007269",
"2404011097",
"2404019438",
"2404020614",
"2404036375",
"2404069424",
"2404003995",
"2404004000",
"2404004134",
"2404004178",
"2404004297",
"2404004300",
"2404004371",
"2404006844",
"2404007213",
"2404007215",
"2404007217",
"2404009352",
"2404009362",
"2404009410",
"2404010597",
"2404014144",
"2404016326",
"2404018335",
"2404028153",
"2404035812",
"2404039695",
"2404047768",
"2404048077",
"2404048081",
"2404048082",
"2404020506",
"2404024310",
"2404003651",
"2404012148",
"2404012589",
"2404023576",
"2404052042",
"2404016861",
"2404018805",
"2404018815",
"2404018822",
"2404019002",
"2404019042",
"2404019048",
"2404020031",
"2404020344",
"2404020371",
"2404028849",
"2404033152",
"2404033311",
"2404033670",
"2404033684",
"2404057345",
"2404057351",
"2404057353",
"2404058120",
"2404058171",
"2404009318",
"2404011796",
"2404011824",
"2404014018",
"2404017222",
"2404020350",
"2404040346",
"2404033413",
"2404034790",
"2404049157",
"2404064829",
"2404070971",
"2404071555",
"2404075455",
"2404006777",
"2404014685",
"2404006996",
"2404010509",
"2404010517",
"2404029410",
"2404031147",
"2404031429",
"2404032600",
"2404034541",
"2404035043",
"2404036568",
"2404037861",
"2404039074",
"2404039076",
"2404039359",
"2404039533",
"2404039684",
"2404060014",
"2404060015",
"2404061438",
"2404064568",
"2404064569",
"2404064571",
"2404067640",
"2404060238",
"2404044129",
"2404047655",
"2404047668",
"2404053929",
"2404055475",
"2404062904",
"2404067558",
"2404067571",
"2404068186",
"2404069810",
"2404069811",
"2404070079",
"2404071626",
"2404071684",
"2404071687",
"2404072873",
"2404073374",
"2404054176",
"2404024221",
"2404024690",
"2404026644",
"2404037068",
"2404052831",
"2404056574",
"2404057653",
"2404070135",
"2404070137",
"2404070615",
"2404071348",
"2404071350",
"2404071354",
"2404056417",
"2404056419",
"2404069460",
"2404070703",
"2404007390",
"2404007394",
"2404007401",
"2404029147",
"2404029149",
"2404029623",
"2404029625",
"2404029628",
"2404034765",
"2404007492",
"2404008537",
"2404023678",
"2404023682",
"2404023687",
"2404026608",
"2404026724",
"2404026725",
"2404032020",
"2404033975",
"2404034824",
"2404035101",
"2404036372",
"2404036497",
"2404041359",
"2404042449",
"2404045033",
"2404049431",
"2404050005",
"2404051003",
"2404051006",
"2404056666",
"2404056685",
"2404057648",
"2404057663",
"2404058422",
"2404005741",
"2404007152",
"2404007154",
"2404007155",
"2404007156",
"2404009384",
"2404009626",
"2404009629",
"2404010686",
"2404014990",
"2404019382",
"2404019538",
"2404019586",
"2404019591",
"2404019595",
"2404019602",
"2404019606",
"2404019610",
"2404019614",
"2404019617",
"2404019619",
"2404019622",
"2404019624",
"2404019626",
"2404019628",
"2404019629",
"2404019632",
"2404019636",
"2404019835",
"2404019837",
"2404019850",
"2404019855",
"2404019891",
"2404021872",
"2404022056",
"2404022105",
"2404022709",
"2404022710",
"2404022715",
"2404022720",
"2404022726",
"2404022733",
"2404022741",
"2404022747",
"2404022753",
"2404022756",
"2404022759",
"2404022761",
"2404022791",
"2404023262",
"2404028622",
"2404030047",
"2404001750",
"2404000553",
"2404000949",
"2404002483",
"2404002486",
"2404076861",
"2404077488",
"2404078728",
"2404000564",
"2404000970",
"2404002043",
"2404078279",
"2404078706",
"2404078732",
"2404078759",
"2404002056",
"2404076097",
"2404076159",
"2404076719",
"2404076771",
"2404000622",
"2404000028",
"2404001014",
"2404018238",
"2404024952",
"2404028451",
"2404029298",
"2404030410",
"2404031252",
"2404032358",
"2404034021",
"2404035581",
"2404036128",
"2404039599",
"2404039949",
"2404042109",
"2404045377",
"2404045381",
"2404045452",
"2404046366",
"2404046407",
"2404046450",
"2404047645",
"2404048286",
"2404053025",
"2404053029",
"2404055277",
"2404056134",
"2404059845"
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
            var pathPdf = System.IO.Path.Combine(_environment.WebRootPath, "doc", "PMVC4","JULIO_2", beneficiario.Estado);
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
