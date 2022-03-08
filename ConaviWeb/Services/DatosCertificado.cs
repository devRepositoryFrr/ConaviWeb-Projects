using Org.BouncyCastle.X509;
using System;
using System.Data;
using System.Text;

namespace ConaviWeb.Services
{
    public class DatosCertificado
    {
        public class datosgeneralescertificado
        {
            public string Certificado { get; set; }

            public string Version { get; set; }

            public string NumeroSerie { get; set; }

            public string AlgoritmoFirma { get; set; }

            public string AlgoritmoHashFirma { get; set; }

            public string EmisorResponsable { get; set; }

            public string EmisorRFC { get; set; }

            public string EmisorCalle { get; set; }

            public string EmisorMunicipio { get; set; }

            public string EmisorEstado { get; set; }

            public string EmisorPais { get; set; }

            public string EmisorCP { get; set; }

            public string EmisorEmail { get; set; }

            public string EmisorAutoridad { get; set; }

            public string EmisorDomicilio { get; set; }

            public string FechaInicio { get; set; }

            public string FechaExpira { get; set; }

            public string SujetoRFC { get; set; }

            public string SujetoNombre { get; set; }

            public bool Vigente { get; set; }

            public bool Autentico { get; set; }

            public bool Valido { get; set; }

            public DateTime TsValidacion { get; set; }

            public string Observaciones { get; set; }


            public string obtendato(string texto, string inicio, string final)
            {
                int iniciocadena = texto.IndexOf(inicio) + inicio.Length;
                if (iniciocadena < 0) { texto.IndexOf(inicio.Replace(" ", "") + inicio.Length); };
                int fincadena = texto.IndexOf(final);
                if (fincadena < 0) { texto.IndexOf(final.Replace(" ", "")); };
                char[] cadena = new char[fincadena - iniciocadena];
                texto.CopyTo(iniciocadena, cadena, 0, fincadena - iniciocadena);
                return new string(cadena);
            }

            public bool vigencia(string RFCSujeto, string RFCCertificado, string FechaExpira, string FechaInicio)
            {
                if (RFCSujeto.ToUpper() == RFCCertificado && DateTime.Now <= Convert.ToDateTime(FechaExpira) && DateTime.Now >= Convert.ToDateTime(FechaInicio))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }


            public OBC_Ocsp.resultadoocsp AutenticidadCertificado(Org.BouncyCastle.Math.BigInteger certificado, DateTime fecha, int idUsuario, string numeroSerie)
            {
                string direccion = "";
                foreach (System.Net.NetworkInformation.NetworkInterface netinf in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    System.Net.NetworkInformation.IPInterfaceProperties properties = netinf.GetIPProperties();
                    foreach (System.Net.NetworkInformation.IPAddressInformation unicast in properties.UnicastAddresses)
                    {
                        direccion += unicast.Address.AddressFamily + ", " + unicast.Address.ToString() + ", " + netinf.GetPhysicalAddress().ToString() + ";";
                    }

                }
                try
                {
                    var _statuscertificado = ValidaCertificado(certificado, fecha, direccion, idUsuario, numeroSerie);
                    var statuscertificado = new OBC_Ocsp.resultadoocsp();
                    //statuscertificado.status = _statuscertificado.status.ToString();
                    statuscertificado.status = _statuscertificado.status;
                    statuscertificado.TimeStampQuery = _statuscertificado.TimeStampQuery;
                    return statuscertificado;
                }
                catch (Exception wer)
                {
                    Console.WriteLine(wer.Message);
                    throw new Exception("Error de consulta: No se puede verificar la autenticidad del certificado. Consulte al administrador del sistema");
                }
            }

            public OBC_Ocsp.resultadoocsp ValidaCertificado(Org.BouncyCastle.Math.BigInteger certificadocliente, DateTime fecha, string direcciones, int idUsuario, string numeroSerie)
            {
                int linea = 0;
                OBC_Ocsp ocspCli = new OBC_Ocsp();
                linea++; //1
                         //variable de respuesta
                var respuesta = new OBC_Ocsp.resultadoocsp();
                linea++; //2
                respuesta.status = OBC_Ocsp.CertificateStatus.Desconocido;
                linea++; //3
                respuesta.TimeStampQuery = DateTime.Now;
                linea++; //4
                respuesta.url = "";
                linea++; //5
                         //CER
                Org.BouncyCastle.X509.X509Certificate certificadoconfianza;
                linea++; //6
                Org.BouncyCastle.X509.X509Certificate emisor;
                linea++; //7
                string CER;
                linea++;
                DataTable dt = new DataTable();
                linea++;
                try
                {

                    CER = "AC" + numeroSerie.Substring(11, 1);

                    switch (CER)
                    {
                        case "AC3":
                            certificadoconfianza = OBC_Utilities.LoadCertificate(Properties.Resources.ocsp_ac3_sat);
                            linea++;
                            emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC3_SAT);
                            linea++;
                            respuesta = ocspCli.Query(certificadocliente, emisor, certificadoconfianza);
                            linea++;
                            break;

                        case "AC4":
                            certificadoconfianza = OBC_Utilities.LoadCertificate(Properties.Resources.ocsp_ac4_sat);
                            linea++;
                            emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC4_SAT);
                            linea++;
                            respuesta = ocspCli.Query(certificadocliente, emisor, certificadoconfianza);
                            linea++;
                            break;
                        case "AC5":
                            certificadoconfianza = OBC_Utilities.LoadCertificate(Properties.Resources.ocsp_ac5_sat);
                            linea++;
                            emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC5_SAT);
                            linea++;
                            respuesta = ocspCli.Query(certificadocliente, emisor, certificadoconfianza);
                            linea++;
                            break;
                        default:
                            certificadoconfianza = OBC_Utilities.LoadCertificate(Properties.Resources.ocsp_ac4_sat);
                            linea++;
                            emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC4_SAT);
                            linea++;
                            respuesta = ocspCli.Query(certificadocliente, emisor, certificadoconfianza);
                            linea++;
                            break;
                    }

                    //  InstrumentoDAO.instancia().insertarOCSP("", ipConsulta, certificadocliente.ToString(), respuesta.url, respuesta.status.ToString(), respuesta.TimeStampQuery.ToString("yyyy-MM-dd HH:mm:ss"), idArchivo);

                }
                catch (Exception ex)
                {
                    //respuesta.status = OBC_Ocsp.CertificateStatus.Error;
                    //respuesta.mensaje = "Error: " + ex.Message.ToString() + ". InnerException: " + ex.InnerException.ToString() + ". Línea: " + linea.ToString();
                }

                return respuesta;
            }
            //}


            public void LeeCertificado(X509Certificate usercertificado, out DatosCertificado.datosgeneralescertificado certificado)
            {
                DatosCertificado.datosgeneralescertificado datos = new DatosCertificado.datosgeneralescertificado();


                string emisor = usercertificado.IssuerDN.ToString();
                string sujeto = usercertificado.SubjectDN.ToString();

                #region Datos certificado
                datos.Certificado = usercertificado.GetPublicKey().ToString();
                byte[] byteArray = usercertificado.SerialNumber.ToByteArray();
                datos.NumeroSerie = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                datos.AlgoritmoFirma = usercertificado.SigAlgName.ToString();
                datos.AlgoritmoHashFirma = usercertificado.GetHashCode().ToString();
                datos.FechaInicio = usercertificado.NotBefore.ToString();
                datos.FechaExpira = usercertificado.NotAfter.ToString();
                #endregion

                #region Datos emisor
                foreach (Org.BouncyCastle.Asn1.DerObjectIdentifier llave in usercertificado.IssuerDN.GetOidList())
                {
                    var valor = usercertificado.IssuerDN.GetValueList(llave);
                    switch (llave.Id.ToString())
                    {
                        case "2.5.4.3": //CN    
                            break;
                        case "2.5.4.10": //O   
                            datos.EmisorAutoridad = valor[0].ToString();
                            break;
                        case "2.5.4.11": //OU    
                            break;
                        case "1.2.840.113549.1.9.1": //E    
                            datos.EmisorEmail = valor[0].ToString();
                            break;
                        case "2.5.4.9": //STREET    
                            datos.EmisorCalle = valor[0].ToString();
                            break;
                        case "2.5.4.17": //POSTALCODE    
                            datos.EmisorCP = valor[0].ToString();
                            break;
                        case "2.5.4.6": //C     
                            datos.EmisorPais = valor[0].ToString();
                            break;
                        case "2.5.4.8": //S    
                            datos.EmisorEstado = valor[0].ToString();
                            break;
                        case "2.5.4.7": //L    
                            datos.EmisorMunicipio = valor[0].ToString();
                            break;
                        case "2.5.4.45": //RFC    
                            datos.EmisorRFC = valor[0].ToString();
                            break;
                        case "1.2.840.113549.1.9.2": //RESPONSABLE    
                            datos.EmisorResponsable = valor[0].ToString();
                            break;
                    }
                }
                datos.EmisorDomicilio = datos.EmisorCalle + ", " + datos.EmisorMunicipio + ", " + datos.EmisorEstado + ", " + datos.EmisorPais + ", CP: " + datos.EmisorCP;
                #endregion

                #region Datos sujeto

                foreach (Org.BouncyCastle.Asn1.DerObjectIdentifier llave in usercertificado.SubjectDN.GetOidList())
                {
                    var valor = usercertificado.SubjectDN.GetValueList(llave);
                    switch (llave.Id.ToString())
                    {
                        case "2.5.4.3": //CN    
                            break;
                        case "2.5.4.41": //SUJETO 
                            datos.SujetoNombre = valor[0].ToString();
                            break;
                        case "2.5.4.10": //O    
                            break;
                        case "2.5.4.6": //C    
                            break;
                        case "1.2.840.113549.1.9.1": //E    
                            break;
                        case "2.5.4.45": //RFC    
                            datos.SujetoRFC = valor[0].ToString();
                            break;
                        case "2.5.4.5": //SERIALNUMBER    
                            break;
                    }
                }
                #endregion
                certificado = datos;
            }
        }
    }
}
