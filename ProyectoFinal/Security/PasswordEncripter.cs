using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProyectoFinal.Security
{
    public class PasswordEncripter:IPasswordEncripter
    {
        public string Encript(string value, List<byte[]> hashes)
        {
            if (hashes.Count < 2)
                throw new ArgumentException("Se deben especificar 2 hash, para Key y IV");
            using (var rijndael = Rijndael.Create())
            {
                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(hashes[0], hashes[1]), CryptoStreamMode.Write))
                {
                    byte[] plainMessageBytes = UTF8Encoding.UTF8.GetBytes(value);
                    cryptoStream.Write(plainMessageBytes, 0, plainMessageBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherMessageBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherMessageBytes);
                }
            }
        }
    }
}