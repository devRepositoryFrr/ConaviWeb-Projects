using ConaviWeb.Data.Repositories;
using ConaviWeb.Model;
using ConaviWeb.Model.Request;
using ConaviWeb.Model.Response;
using ConaviWeb.Tools;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ConaviWeb.Services
{
    public class ProcessCancelService : IProcessCancelService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IProcessSignRepository _processSignRepository;
        private readonly ISecurityRepository _securityRepository;
        private readonly ISourceFileRepository _sourceFileRepository;
        public ProcessCancelService(IWebHostEnvironment environment, IProcessSignRepository processSignRepository, ISecurityRepository securityRepository, ISourceFileRepository sourceFileRepository)
        {
            _environment = environment;
            _processSignRepository = processSignRepository;
            _securityRepository = securityRepository;
            _sourceFileRepository = sourceFileRepository;
        }

        public async Task<bool> ProcessFileSatAsync(User user, DataSignRequest dataSignRequest, IEnumerable<FileResponse> files)
        {
            var fileKey = Array.Empty<byte>();
            var fileCert = Array.Empty<byte>();
            bool certificadoValido, llavePrivadaValida, keyCerMatched, vigencia;
            bool success = false;
            DatosCertificado.datosgeneralescertificado certificadoleido = new DatosCertificado.datosgeneralescertificado();
            DatosCadenaOriginal datosfea = new DatosCadenaOriginal();


            if (dataSignRequest.KeySat.Length > 0)
            {
                using var ms = new MemoryStream();
                string ext = System.IO.Path.GetExtension(dataSignRequest.KeySat.FileName);

                dataSignRequest.KeySat.CopyTo(ms);
                fileKey = ms.ToArray();
                //string s = Convert.ToBase64String(fileKey);
            }
            if (dataSignRequest.CerSat.Length > 0)
            {
                using var ms = new MemoryStream();
                string ext = System.IO.Path.GetExtension(dataSignRequest.CerSat.FileName);

                dataSignRequest.CerSat.CopyTo(ms);
                fileCert = ms.ToArray();
                //string s = Convert.ToBase64String(fileKey);
            }


            var archivoKey = OBC_Utilities.MatchKeyPwd(dataSignRequest.PassFirmante.ToCharArray(), fileKey);
            llavePrivadaValida = archivoKey != null;
            if (llavePrivadaValida)
            {

                X509Certificate archivoCer = OBC_Utilities.LoadCertificate(fileCert);
                keyCerMatched = OBC_Utilities.MatchKeyCer(archivoKey, archivoCer);

                certificadoleido = ObtenCertificado(archivoCer, user.Id, user.RFC);//vigencia

                certificadoValido = certificadoleido.Valido;

                vigencia = certificadoleido.Vigente;

                if (keyCerMatched && certificadoValido && vigencia)
                {
                    foreach (var file in files)
                    {
                        var pathFile = System.IO.Path.Combine(_environment.WebRootPath, file.FilePath, file.FileName);
                        byte[] fileDoc = System.IO.File.ReadAllBytes(pathFile);
                        success = await FirmaDocumentoAsync(pathFile, fileDoc, fileCert, archivoKey, file.Id, user, file.IdPartition);
                    }


                }
            }
            return success;
        }
        private async Task<bool> FirmaDocumentoAsync(string pathDoc, byte[] fileDoc, byte[] fileCert, AsymmetricKeyParameter archivoKey, int idArchivoPadre, User user, int idPartition)
        {


            DateTime inicio = DateTime.Now;
            bool success = true;
            try
            {

                if (archivoKey != null)  //&& (superiorseleccionado)  
                {
                    var partition = await _securityRepository.GetPartition(idPartition);
                    string shorthPathXML = "";
                    string currentXML = "";
                    string XMLName = "";
                    DatosCadenaOriginal datosfea = new();
                    string cadenaOriginal = "";
                    bool estatus;
                    cadenaOriginal = "|" + XMLOriginalv2(pathDoc, fileDoc, fileCert, user, partition, out datosfea, out estatus, out shorthPathXML, out currentXML, out XMLName) + "||";
                    datosfea.CadenaOriginal = cadenaOriginal;
                    string pathXML = System.IO.Path.Combine(currentXML, XMLName);
                    // Firma de sello y actualización de XML
                    if (datosfea.CadenaOriginal != "" && estatus == true)
                    {
                        datosfea.Sello = GeneraSello(datosfea.CadenaOriginal, archivoKey, fileCert);
                        ActualizaXMLv2(datosfea.Sello, pathXML);

                        //Genera QR sello
                        QRCodeGenerator qrGenerator = new();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(datosfea.CadenaOriginal, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrcode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
                        string QrName = System.IO.Path.GetFileNameWithoutExtension(pathXML) + ".jpg";
                        string routeQR = System.IO.Path.Combine(_environment.WebRootPath, "doc", "EFirma", QrName);
                        qrCodeImage.Save(routeQR);//write your path where you want to store the qr-code image.
                        //Genera reporte pdf con sello
                        success = await EditPDFAsync(pathDoc, routeQR, idArchivoPadre, datosfea, user, shorthPathXML, XMLName, partition);

                    }


                    datosfea = null;


                    //mensaje = "Se realizó la firma de documentación con éxito.\nDocumentos firmados: " + docsfirmados + "\nTiempo transcurrido: " + tiempo + mensaje;
                    //ProgresoFirma(false, true, mensaje, sello);
                    //}
                    //else
                    //{
                    //    //ProgresoFirma(false, false, "Ocurrió un error y no se pudo firmar el documento, intente nuevamente", "");
                    //}

                    // mostrarAlerta("(" + lst_beneficiarios.Count + ") Beneficiarios firmados correctamente. ", AlertaVO.type_success);
                }
                else
                {
                    // mostrarAlerta("Seleccione algun estatus válido.", AlertaVO.type_warning);
                }
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }

        private string XMLOriginalv2(string pathDoc, byte[] fileDoc, byte[] fileCert, User user, Partition partition, out DatosCadenaOriginal datosfea, out bool estatus, out string shorthPathXML, out string currentXML, out string XMLName)
        {
            //.NET Core no permite resolver URI externos para XML
            AppContext.SetSwitch("Switch.System.Xml.AllowDefaultResolver", true);

            try
            {

                //string datosPersona = string.Join(";", string.Join(" ", persona.persona.nombre, persona.persona.aPaterno, persona.persona.aMaterno).Trim(), persona.persona.RFC, persona.persona.numEmpleado, persona.persona.area, persona.persona.nombrePuesto);

                DatosCertificado.datosgeneralescertificado datoscertificado = new DatosCertificado.datosgeneralescertificado();
                Org.BouncyCastle.X509.X509Certificate archivoCer = OBC_Utilities.LoadCertificate(fileCert);
                datoscertificado = ObtenCertificado(archivoCer, user.Id, user.RFC);
                DatosCadenaOriginal datosCadenaOriginal = new DatosCadenaOriginal();
                DateTime dateTime = DateTime.Now;
                try
                {

                    datosCadenaOriginal.Tema = "Firma Electrónica";
                    //_datosco.Correcto = datosco.Correcto;
                    datosCadenaOriginal.Folio = dateTime.ToString("ddMMyyHHmmssffff");
                    datosCadenaOriginal.Movimiento = "Cancelación";
                    datosCadenaOriginal.HashArchivo = ProccessFileTools.GetHashDocument(fileDoc);
                    datosCadenaOriginal.NombreFirmante = user.Name + " " + user.LName + " " + user.SLName;
                    //_datosco.idFirmante = benefVO.id;
                    //_datosco.usuarioFirmante = benefVO.id;
                    datosCadenaOriginal.NumEmpleadoFirmante = user.EmployeeNumber;
                    datosCadenaOriginal.PuestoFirmante = user.Position;
                    datosCadenaOriginal.AreaFirmante = user.Area.ToString();
                    datosCadenaOriginal.RfcFirmante = datoscertificado.SujetoRFC;
                    //_datosco.sello = datosco.sello;
                    //_datosco.sistema = datosco.sistema;
                    datosCadenaOriginal.TimeStampOCSP = datoscertificado.TsValidacion;
                    datosCadenaOriginal.TimeStampSign = datoscertificado.TsValidacion;
                    datosCadenaOriginal.AlgoritmoFirma = datoscertificado.AlgoritmoFirma;
                    datosCadenaOriginal.CertificateNumber = datoscertificado.NumeroSerie;
                    //_datosco.id_archivo = benefVO.id_archivo;
                    var shorthPath = System.IO.Path.Combine("doc", "EFirma", "XMLCancelado", dateTime.Year.ToString(), dateTime.Month.ToString(), partition.Text);
                    string currentPath = System.IO.Path.Combine(_environment.WebRootPath, shorthPath);
                    //string currentPath = pathRoot + dateTime.Year + "\\" + dateTime.Month;
                    if (!Directory.Exists(currentPath))
                        ProccessFileTools.CreateDirectory(currentPath);
                    string pathXslt = System.IO.Path.Combine(_environment.WebRootPath, "doc", "EFirma", "Xslt", "fea_convenio.xslt");
                    string xmlName = System.IO.Path.GetFileNameWithoutExtension(pathDoc) + "_" + dateTime.ToString("ddMMyyHHmmss") + ".xml";
                    string pathXml = System.IO.Path.Combine(currentPath, xmlName);
                    XmlSerializerNamespaces xmlNameSpace = new XmlSerializerNamespaces();
                    XmlTextWriter xmlTextWriter = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8); //Definir metodología de nombramiento de archivo
                    xmlTextWriter.Formatting = Formatting.Indented;
                    XmlSerializer xs = new XmlSerializer(typeof(DatosCadenaOriginal));
                    xs.Serialize(xmlTextWriter, datosCadenaOriginal, xmlNameSpace);

                    xmlTextWriter.Close();

                    //Cargar el XML generado
                    StreamReader leerXML = new StreamReader(pathXml);
                    XPathDocument XMLgenerado = new XPathDocument(leerXML);


                    StreamReader leerXSLT = new StreamReader(pathXslt);

                    XPathDocument xslt = new XPathDocument(leerXSLT);
                    XslCompiledTransform transformacionXslt = new XslCompiledTransform();
                    transformacionXslt.Load(xslt);

                    StringWriter str = new StringWriter();
                    XmlTextWriter myWriter = new XmlTextWriter(str);

                    //Aplicando transformacion
                    transformacionXslt.Transform(XMLgenerado, null, myWriter);

                    //Resultado
                    datosCadenaOriginal.IdArchivo = 5;
                    datosfea = datosCadenaOriginal;
                    estatus = true;
                    currentXML = currentPath;
                    shorthPathXML = shorthPath;
                    XMLName = xmlName;
                    return str.ToString();



                }
                catch (Exception errf)
                {
                    //MessageBox.Show("Ha ocurrido un error y no es posible mostrar la información.\nPor favor contacte al Departamento de Análisis y Programación.\n\n" + errf.Message);
                    Console.WriteLine(errf.Message);
                    DatosCadenaOriginal feaerror = new DatosCadenaOriginal();
                    //feaerror.Correcto = false;
                    //feaerror.Mensaje = errf.Message;// feaerror.solicitante;
                    datosfea = feaerror;
                    estatus = false;
                    shorthPathXML = "";
                    currentXML = "";
                    XMLName = "";
                    return "";
                }
            }
            catch (Exception exc)
            {
                DatosCadenaOriginal feaerror = new DatosCadenaOriginal();
                datosfea = feaerror;
                estatus = false;
                shorthPathXML = "";
                currentXML = "";
                XMLName = "";
                return "";
            }
        }




        public async Task<bool> EditPDFAsync(string pathDoc, string routeQR, int idArchivoPadre, DatosCadenaOriginal datosfea, User user, string shorthPathXML, string XMLName, Partition partition)
        {
            DateTime dateTime = DateTime.Now;
            var shortPath = System.IO.Path.Combine("doc", "EFirma", "Cancelado", dateTime.Year.ToString(), dateTime.Month.ToString(), partition.Text);
            var partitionPath = System.IO.Path.Combine("doc", "EFirma", "Cancelado", dateTime.Year.ToString(), dateTime.Month.ToString());
            SigningFile signingFile = new();
            signingFile.SignatureDate = datosfea.TimeStampSign;
            signingFile.Folio = datosfea.Folio;
            signingFile.OriginalString = datosfea.CadenaOriginal;
            signingFile.SignatureStamp = datosfea.Sello;
            signingFile.CertSeries = datosfea.CertificateNumber;
            signingFile.Algorithm = datosfea.AlgoritmoFirma;
            signingFile.Hash = datosfea.HashArchivo;
            signingFile.FilePath = shortPath;
            var filePath = System.IO.Path.Combine(_environment.WebRootPath, shortPath);
            if (!Directory.Exists(filePath))
                ProccessFileTools.CreateDirectory(filePath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(pathDoc);
            signingFile.FileName = fileName + "_" + dateTime.ToString("ddMMyyHHmmss") + ".pdf";
            string pdfresult = System.IO.Path.Combine(filePath, signingFile.FileName);
            // Initialize PDF document

            PdfReader reader = new PdfReader(System.IO.File.OpenRead(pathDoc));
            PdfDocument pdf = new PdfDocument(reader, new PdfWriter(pdfresult));

            // Initialize document
            Document document = new Document(pdf);
            bool success;
            try
            {
                // Add content
                pdf.SetDefaultPageSize(PageSize.LETTER);
                pdf.AddNewPage();
                document.Add(new AreaBreak(AreaBreakType.LAST_PAGE));


                PdfFont font_title = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);
                PdfFont font_content = PdfFontFactory.CreateFont(StandardFonts.COURIER);
                string tipoFirmante = user.Area.ToString();
                Text title1 = new Text("Cadena original").SetFont(font_title).SetFontSize(8);
                Text content1 = new Text(signingFile.OriginalString).SetFont(font_content).SetFontSize(6);
                Text title2 = new Text("Firma electrónica " + tipoFirmante).SetFont(font_title).SetFontSize(8);
                Text content2 = new Text(signingFile.SignatureStamp).SetFont(font_content).SetFontSize(6);
                Paragraph p1 = new Paragraph().Add(title1).SetTextAlignment(TextAlignment.JUSTIFIED);
                Paragraph p2 = new Paragraph().Add(content1).SetTextAlignment(TextAlignment.JUSTIFIED);
                Paragraph p3 = new Paragraph().Add(title2).SetTextAlignment(TextAlignment.JUSTIFIED);
                Paragraph p4 = new Paragraph().Add(content2).SetTextAlignment(TextAlignment.JUSTIFIED);

                // Upload image
                ImageData imageData = ImageDataFactory.Create(routeQR);
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).ScaleAbsolute(60, 60);

                // Table Cadena y QR
                Table table = new Table(2);
                table.SetWidth(500);
                table.SetTextAlignment(TextAlignment.JUSTIFIED);
                table.AddCell(new Cell().Add(p2).SetBorder(Border.NO_BORDER));
                table.AddCell(new Cell().Add(image).SetBorder(Border.NO_BORDER));

                // This adds the image to the page
                document.Add(p1).Add(table).Add(p3).Add(p4.SetWidth(430));
                //Close document
                document.Close();
                //Delete QR
                if (File.Exists(routeQR))
                {
                    File.Delete(routeQR);
                }
                success = await _processSignRepository.InsertCancelFile(signingFile, user, idArchivoPadre, shorthPathXML, XMLName, partition);
                if (partition.PathPartition == null)
                {
                    partition.PathPartition = partitionPath;
                    await _sourceFileRepository.UpdateParition(partition);
                }

            }
            catch(Exception e) { success = false; }
            return success;
        }

        public DatosCertificado.datosgeneralescertificado ObtenCertificado(X509Certificate archivoCer, int idUsuario, string rfcRequest)
        {
            try
            {
                DatosCertificado.datosgeneralescertificado datos_cer = new DatosCertificado.datosgeneralescertificado();
                datos_cer.LeeCertificado(archivoCer, out datos_cer);
                datos_cer.Vigente = datos_cer.vigencia(rfcRequest, datos_cer.SujetoRFC, datos_cer.FechaExpira, datos_cer.FechaInicio);

                if (datos_cer.Vigente)
                {

                    OBC_Ocsp.resultadoocsp consultaocsp = datos_cer.AutenticidadCertificado(archivoCer.SerialNumber, archivoCer.NotBefore, idUsuario, datos_cer.NumeroSerie);
                    datos_cer.Autentico = consultaocsp.status.ToString() == "Vigente";
                    datos_cer.TsValidacion = consultaocsp.TimeStampQuery;
                }
                datos_cer.Valido = datos_cer.Vigente && datos_cer.Autentico;

                return datos_cer;
            }
            catch (Exception er)
            {
                DatosCertificado.datosgeneralescertificado datos_cer = new DatosCertificado.datosgeneralescertificado();
                datos_cer.Vigente = false;
                datos_cer.Valido = false;
                datos_cer.Observaciones = er.Message;
                return datos_cer;
            }
        }

        private string GeneraSello(string CadenaOriginal, AsymmetricKeyParameter archivoKey, byte[] fileCert)
        {
            #region Firma de documentos
            byte[] CO = Encoding.UTF8.GetBytes(CadenaOriginal);

            X509Certificate archivoCer = OBC_Utilities.LoadCertificate(fileCert);

            X509CertificateEntry certEntry = new X509CertificateEntry(archivoCer);

            //Pendiente! Rutina para obtener el algoritmo firma, si no se encuentra se deberá registrar previamente

            ISigner sig = SignerUtilities.GetSigner(archivoCer.SigAlgName);
            sig.Init(true, archivoKey);
            sig.BlockUpdate(CO, 0, CO.Length); //CO es la cadena original a firmar
            byte[] signature = sig.GenerateSignature();
            string sello = Convert.ToBase64String(signature);

            #region Validación de sello
            //byte[] selloBytes = Convert.FromBase64String(sello);
            //sig.Reset();
            //sig = SignerUtilities.GetSigner(archivoCer.SigAlgName);
            //sig.Init(false, archivoCer.GetPublicKey());
            //sig.BlockUpdate(CO, 0, CO.Length);
            //bool valido = true;
            //valido = sig.VerifySignature(selloBytes);
            #endregion
            return sello;
            #endregion
        }

        private void ActualizaXMLv2(string sello, string pathXML)
        {

            string pathXslt = System.IO.Path.Combine(_environment.WebRootPath, "doc", "EFirma", "Xslt", "fea_convenio.xslt");
            XmlDocument doc = new XmlDocument();

            StreamReader leerXML = new StreamReader(pathXML);
            doc.LoadXml(leerXML.ReadToEnd());
            leerXML.Close();
            XmlNode root = doc.DocumentElement;

            //Generación del nodo Sello
            XmlElement elem = doc.CreateElement("sello");
            elem.InnerText = sello;

            //Se agrega el nodo al documento XML
            root.AppendChild(elem);
            //doc.Save(fbdRepositorio.SelectedPath + "\\ASI_A05F103_" + id + "-" + consultatipo_ + ".xml");
            doc.Save(pathXML);


            //Cargar el XML generado
            StreamReader leerXMLV2 = new StreamReader(pathXML);
            XPathDocument XMLgenerado = new XPathDocument(leerXMLV2);

            StreamReader leerXSLT = new StreamReader(pathXslt);

            XPathDocument xslt = new XPathDocument(leerXSLT);
            XslCompiledTransform transformacionXslt = new XslCompiledTransform();
            transformacionXslt.Load(xslt);

            StringWriter str = new StringWriter();
            XmlTextWriter myWriter = new XmlTextWriter(str);

            //Aplicando transformacion
            transformacionXslt.Transform(XMLgenerado, null, myWriter);



        }
    }
}
