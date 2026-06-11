using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ERP.Utilities
{
    public class CryptoServices
    {
        /// <summary>
        /// Encrypts a string using the DES algorithm
        /// </summary>
        /// <param name="sInputstring">string to be encrypted</param>
        /// <param name="sKey">key used to encrypt</param>
        /// <returns>Encrypted String</returns>
        public string EncryptString(string sInputstring, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            ICryptoTransform desencrypt = DES.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptostream = new CryptoStream(memoryStream, desencrypt, CryptoStreamMode.Write);

            byte[] bytearrayinput = Encoding.UTF8.GetBytes(sInputstring);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.FlushFinalBlock();
            byte[] bytes = memoryStream.ToArray();
            cryptostream.Close();
            memoryStream.Close();
            cryptostream.Dispose();
            memoryStream.Dispose();
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        /// Decrypts the encrypted string
        /// </summary>
        /// <param name="sInputString">Encrypted String</param>
        /// <param name="sKey">Key to decrypt</param>
        /// <returns></returns>
        public string DecryptString(string sInputString, string sKey)
        {
            string text = string.Empty;
            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

                byte[] inputByteArray = Convert.FromBase64String(sInputString.Replace(" ", "+")); 
                MemoryStream memoryStream = new MemoryStream(inputByteArray);
                ICryptoTransform desdecrypt = DES.CreateDecryptor();

                CryptoStream cryptostream = new CryptoStream(memoryStream, desdecrypt, CryptoStreamMode.Read);

                byte[] textbyte = new byte[inputByteArray.Length];
                int decryptedByteCount = cryptostream.Read(textbyte, 0, inputByteArray.Length);
                memoryStream.Close();
                cryptostream.Close();
                memoryStream.Dispose();
                cryptostream.Dispose();
                return text = Encoding.UTF8.GetString(textbyte, 0, decryptedByteCount);

            }
            catch
            {
                return text;
            }
        }
    }
       
}
