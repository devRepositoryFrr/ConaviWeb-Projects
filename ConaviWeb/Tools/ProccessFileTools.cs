using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ConaviWeb.Tools
{
    public class ProccessFileTools
    {
        public static string GetHashDocument(byte[] file)
        {
            SHA512 shaM = new SHA512Managed();
            string hashFile = BitConverter.ToString(shaM.ComputeHash(file));

            return hashFile;
        }
        public static string GetHashDocument(IFormFile file)
        {
            byte[] fileBytes;
            SHA512 shaM = new SHA512Managed();
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                fileBytes = ms.ToArray();
            }
            //fileByte = shaM.ComputeHash(file.OpenReadStream());
            string hashFile = BitConverter.ToString(shaM.ComputeHash(fileBytes));

            return hashFile;
        }
        public static void CreateDirectory(string path)
        {

            try
            {
                // Try to create the directory if not exist.
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch { }

        }
        public static async Task SaveFileAsync(IFormFile file, string path)
        {
            try
            {
                using var stream = System.IO.File.Create(path);
                await file.CopyToAsync(stream);
            }
            catch { }

        }
    }
}
