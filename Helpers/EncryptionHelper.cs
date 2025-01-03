using System.Security.Cryptography;
using System.Text;

namespace NetCoreApp.Helpers;

public static class EncryptionHelper
    {
        private static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ufnvsRUz");
        
        /// <summary>
        /// Encrypt a string.
        /// </summary>
        /// <param name="originalString">The original string.</param>
        /// <returns>The encrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the original string is null or empty.</exception>
        public static string Encrypt(string originalString)
        {
            DESCryptoServiceProvider cryptoProvider = null;
            MemoryStream memoryStream = null;

            cryptoProvider = new DESCryptoServiceProvider();
            memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(bytes, bytes),
                                                         CryptoStreamMode.Write);

            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();

            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        
        /// <summary>
        /// Decrypt a encrypted string.
        /// </summary>
        /// <param name="cryptedString">The encrypted string.</param>
        /// <returns>The decrypted string.</returns>
        /// <exception cref="ArgumentNullException">This exception will be thrown when the encrypted string is null or empty.</exception>
        public static string Decrypt(string cryptedString)
        {
            DESCryptoServiceProvider cryptoProvider = null;
            MemoryStream memoryStream = null;
            StreamReader reader = null;

            cryptoProvider = new DESCryptoServiceProvider();
            memoryStream = new MemoryStream(Convert.FromBase64String(cryptedString));
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(bytes, bytes),
                                                         CryptoStreamMode.Read);
            reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();
        }

    }