using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.IO;
using System.Net;

namespace ConaviWeb.Services
{
    public class OBC_Ocsp
    {
        public enum CertificateStatus { Vigente, Revocado, Desconocido, Error };

        public static readonly int BufferSize = 4096 * 8;

        private readonly int MaxClockSkew = 36000000;

        public class resultadoocsp
        {
            public CertificateStatus status { get; set; }
            public DateTime TimeStampQuery { get; set; }
            public string url { get; set; }
            public string mensaje { get; set; }
        }


        //trustcert= OBC_Utilities.LoadCertificate(Properties.Resources.ocsp_ac0_sat);
        //emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC0_SAT)
        //respuesta = ocspCli.Query(certificadocliente, emisor, certificadoconfianza);
        public resultadoocsp Query(BigInteger eeCert, X509Certificate issuerCert, X509Certificate trustCert)
        {
            /*
             * ¡No eliminar!
             * Como configuración especial se determina que todas las validaciones
             * se consulten al sitio https://cfdi.sat.gob.mx/edofiel porque así lo ha determinado el SAT
            List<string> urls = OBC_Utilities.GetAuthorityInformationAccessOcspUrl(trustCert);
            string url = "";
            if (urls.Count == 0)
            {
                throw new Exception("No se pudo obtener la dirección URL.");
            }
            else
            {
                url = urls[0];
            }
            */
            //string url = "https://cfdit.sat.gob.mx/edofiel"; //pruebas
            string url = "https://cfdi.sat.gob.mx/edofiel"; //instrucción específica

            OcspReq req = GenerateOcspRequest(issuerCert, eeCert);

            byte[] binaryResp = PostData(url, req.GetEncoded(), "application/ocsp-request", "application/ocsp-response");

            return ProcessOcspResponse(eeCert, issuerCert, trustCert, binaryResp, url);
        }

        //eeCert,==fileCert.FileBytes
        //emisor = OBC_Utilities.LoadCertificate(Properties.Resources.AC0_SAT)
        //public resultadoocsp Query(BigInteger eeCert, X509Certificate issuerCert)
        //{
        //    /*
        //     * ¡No eliminar!
        //     * Como configuración especial se determina que todas las validaciones
        //     * se consulten al sitio https://cfdi.sat.gob.mx/edofiel porque así lo ha determinado el SAT
        //    List<string> urls = OBC_Utilities.GetAuthorityInformationAccessOcspUrl(trustCert);
        //    string url = "";
        //    if (urls.Count == 0)
        //    {
        //        throw new Exception("No se pudo obtener la dirección URL.");
        //    }
        //    else
        //    {
        //        url = urls[0];
        //    }
        //    */
        //    string url = "https://cfdi.sat.gob.mx/edofiel"; //instrucción específica


        //    OcspReq req = GenerateOcspRequest(issuerCert, eeCert);

        //    byte[] binaryResp = PostData(url, req.GetEncoded(), "application/ocsp-request", "application/ocsp-response");

        //    //return ProcessOcspResponse(eeCert, issuerCert, trustCert, binaryResp, url);
        //    return ProcessOcspResponse(eeCert, issuerCert, binaryResp, url);
        //}

        //private resultadoocsp ProcessOcspResponse(BigInteger eeCert, X509Certificate issuerCert, byte[] binaryResp, string url)
        private resultadoocsp ProcessOcspResponse(BigInteger eeCert, X509Certificate issuerCert, X509Certificate trustCert, byte[] binaryResp, string url)
        {
            OcspResp r = new OcspResp(binaryResp);
            CertificateStatus cStatus = CertificateStatus.Desconocido;
            DateTime ts = DateTime.Now;
            //BasicOCSPResp basicResponse = (BasicOCSPResp)ocspResponse.getResponseObject();
            //return basicResponse.getEncoded();

            switch (r.Status)
            {
                case OcspRespStatus.Successful:
                    BasicOcspResp or = (BasicOcspResp)r.GetResponseObject();
                    ValidateResponse(or, trustCert);
                    if (or.Responses.Length == 1)
                    {
                        SingleResp resp = or.Responses[0];
                        ValidateCertificateId(issuerCert, eeCert, resp.GetCertID());
                        ValidateThisUpdate(resp);
                        ValidateNextUpdate(resp);
                        Object certificateStatus = resp.GetCertStatus();
                        if (certificateStatus == Org.BouncyCastle.Ocsp.CertificateStatus.Good)
                        {
                            cStatus = CertificateStatus.Vigente;
                            ts = or.ProducedAt.ToLocalTime();
                        }
                        else if (certificateStatus is Org.BouncyCastle.Ocsp.RevokedStatus)
                        {
                            cStatus = CertificateStatus.Revocado;
                            ts = or.ProducedAt.ToLocalTime();
                        }
                        else if (certificateStatus is Org.BouncyCastle.Ocsp.UnknownStatus)
                        {
                            cStatus = CertificateStatus.Desconocido;
                            ts = or.ProducedAt.ToLocalTime();
                        }
                    }
                    break;
            }
            resultadoocsp res = new resultadoocsp();
            res.status = cStatus;
            res.TimeStampQuery = ts.ToLocalTime();
            res.url = url;
            res.mensaje = "Respuesta recibida";
            return res;
        }

        private void ValidateResponse(BasicOcspResp or, X509Certificate issuerCert)
        {
            ValidateResponseSignature(or, issuerCert.GetPublicKey());
            ValidateSignerAuthorization(issuerCert, or.GetCerts()[0]);
        }

        //3. The identity of the signer matches the intended recipient of the
        //request.
        //4. The signer is currently authorized to sign the response.
        private void ValidateSignerAuthorization(X509Certificate issuerCert, X509Certificate signerCert)
        {
            // This code just check if the signer certificate is the same that issued the ee certificate
            // See RFC 2560 for more information
            if (!(issuerCert.IssuerDN.Equivalent(signerCert.IssuerDN) && issuerCert.SerialNumber.Equals(signerCert.SerialNumber)))
            {
                throw new Exception("Invalid OCSP signer");
            }
        }

        //2. The signature on the response is valid;
        private void ValidateResponseSignature(BasicOcspResp or, Org.BouncyCastle.Crypto.AsymmetricKeyParameter asymmetricKeyParameter)
        {
            if (!or.Verify(asymmetricKeyParameter))
            {
                throw new Exception("Invalid OCSP signature");
            }

        }

        //6. When available, the time at or before which newer information will
        //be available about the status of the certificate (nextUpdate) is
        //greater than the current time.
        private void ValidateNextUpdate(SingleResp resp)
        {
            if (resp.NextUpdate != null && resp.NextUpdate.Value != null && resp.NextUpdate.Value.Ticks <= DateTime.Now.Ticks)
            {
                throw new Exception("Invalid next update.");
            }
        }

        //5. The time at which the status being indicated is known to be
        //correct (thisUpdate) is sufficiently recent.
        private void ValidateThisUpdate(SingleResp resp)
        {
            if (Math.Abs(resp.ThisUpdate.Ticks - DateTime.Now.Ticks) > MaxClockSkew)
            {
                //se deshabilitó esta validación toda vez que el reloj del SAT está en GMT0
                //throw new Exception("Max clock skew reached.");
            }
        }

        //1. The certificate identified in a received response corresponds to
        //that which was identified in the corresponding request;
        private void ValidateCertificateId(X509Certificate issuerCert, BigInteger eeCert, CertificateID certificateId)
        {
            CertificateID expectedId = new CertificateID(CertificateID.HashSha1, issuerCert, eeCert);

            if (!expectedId.SerialNumber.Equals(certificateId.SerialNumber))
            {
                throw new Exception("Invalid certificate ID in response");
            }

            if (!Org.BouncyCastle.Utilities.Arrays.AreEqual(expectedId.GetIssuerNameHash(), certificateId.GetIssuerNameHash()))
            {
                throw new Exception("Invalid certificate Issuer in response");
            }

        }

        private OcspReq GenerateOcspRequest(X509Certificate issuerCert, BigInteger serialNumber)
        {
            CertificateID id = new CertificateID(CertificateID.HashSha1, issuerCert, serialNumber);
            return GenerateOcspRequest(id);
        }

        private OcspReq GenerateOcspRequest(CertificateID id)
        {
            OcspReqGenerator ocspRequestGenerator = new OcspReqGenerator();

            ocspRequestGenerator.AddRequest(id);

            BigInteger nonce = BigInteger.ValueOf(new DateTime().Ticks);

            ArrayList oids = new ArrayList();
            Hashtable values = new Hashtable();

            oids.Add(OcspObjectIdentifiers.PkixOcsp);

            Asn1OctetString asn1 = new DerOctetString(new DerOctetString(new byte[] { 1, 3, 6, 1, 5, 5, 7, 48, 1, 1 }));

            values.Add(OcspObjectIdentifiers.PkixOcsp, new X509Extension(false, asn1));
            ocspRequestGenerator.SetRequestExtensions(new X509Extensions(oids, values));

            return ocspRequestGenerator.Generate();
        }

        public static byte[] PostData(string url, byte[] data, string contentType, string accept)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate {
                return true;
            };
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = data.Length;
            request.Accept = accept;

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream respStream = response.GetResponseStream();
            byte[] resp = ToByteArray(respStream);
            respStream.Close();

            return resp;
        }

        public static byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[BufferSize];
            MemoryStream ms = new MemoryStream();

            int read = 0;

            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }

            return ms.ToArray();
        }
    }
}
