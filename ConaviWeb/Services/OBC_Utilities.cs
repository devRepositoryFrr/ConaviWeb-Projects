using System;
using System.Collections.Generic;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using System.Collections;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Math;

namespace ConaviWeb.Services
{
    public class OBC_Utilities
    {
        public static X509Certificate LoadCertificate(string filename)
        {
            X509CertificateParser certParser = new X509CertificateParser();
            FileStream fs = new FileStream(filename, FileMode.Open);

            X509Certificate cert = certParser.ReadCertificate(fs);
            fs.Close();

            return cert;
        }

        public static X509Certificate LoadCertificate(byte[] filecer)
        {
            X509CertificateParser certParser = new X509CertificateParser();
            X509Certificate cert = certParser.ReadCertificate(filecer);
            return cert;
        }

        public static List<string> GetAuthorityInformationAccessOcspUrl(X509Certificate cert)
        {
            List<string> ocspUrls = new List<string>();

            try
            {
                Asn1Object obj = GetExtensionValue(cert, X509Extensions.AuthorityInfoAccess.Id);

                if (obj == null)
                {
                    return null;
                }
                // AuthorityInformationAccess aia = AuthorityInformationAccess.GetInstance(obj);
                //Búsqueda manual
                Asn1Sequence s = (Asn1Sequence)obj;
                IEnumerator elements = s.GetEnumerator();

                while (elements.MoveNext())
                {
                    Asn1Sequence element = (Asn1Sequence)elements.Current;
                    DerObjectIdentifier oid = (DerObjectIdentifier)element[0];

                    if (oid.Id.Equals("1.3.6.1.5.5.7.48.1"))
                    {
                        Asn1TaggedObject taggedObject = (Asn1TaggedObject)element[1];
                        GeneralName gn = (GeneralName)GeneralName.GetInstance(taggedObject);
                        ocspUrls.Add(((DerIA5String)DerIA5String.GetInstance(gn.Name)).GetString());
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("No se pudo obtener la información de acceso.", e);
            }

            return ocspUrls;
        }

        protected static Asn1Object GetExtensionValue(X509Certificate cert, string oid)
        {
            if (cert == null)
            {
                return null;
            }

            byte[] bytes = cert.GetExtensionValue(new DerObjectIdentifier(oid)).GetOctets();

            if (bytes == null)
            {
                return null;
            }

            Asn1InputStream aIn = new Asn1InputStream(bytes);

            return aIn.ReadObject();
        }

        public static bool MatchKeyCer(AsymmetricKeyParameter archivo_key, X509Certificate archivo_cer)
        {
            try
            {
                X509CertificateEntry certEntry = new X509CertificateEntry(archivo_cer);

                //Certificado
                Org.BouncyCastle.Crypto.AsymmetricKeyParameter cer_usr = certEntry.Certificate.GetPublicKey();
                Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters ll_cer_usr = (Org.BouncyCastle.Crypto.Parameters.RsaKeyParameters)cer_usr;

                BigInteger ex_cer_usr = ll_cer_usr.Exponent;
                BigInteger mod_cer_usr = ll_cer_usr.Modulus;

                //Llave privada
                Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters ll_key_usr = (Org.BouncyCastle.Crypto.Parameters.RsaPrivateCrtKeyParameters)archivo_key;

                BigInteger ex_pub_key_usr = ll_key_usr.PublicExponent;
                BigInteger mod_key_usr = ll_key_usr.Modulus;

                return (ex_cer_usr.CompareTo(ex_pub_key_usr) == 0) && (mod_cer_usr.CompareTo(mod_key_usr) == 0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static Org.BouncyCastle.Crypto.AsymmetricKeyParameter MatchKeyPwd(char[] password, byte[] byte_key)
        {
            try
            {
                return Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(password, byte_key);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
