using System;

namespace ConaviWeb.Model
{
    public class SigningFile
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime SignatureDate { get; set; }
        public string Folio { get; set; }
        public string OriginalString { get; set; }
        public string SignatureStamp { get; set; }
        public string CertSeries { get; set; }
        public string Algorithm { get; set; }
        public string Hash { get; set; }
        public int IdUser { get; set; }
        public int NuSign { get; set; }
        public int IdStatus { get; set; }
        public int IdFileParent { get; set; }
    }
}
