using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LineRobot.Test
{
    [TestClass]
    public class CryptographyServiceTest
    {
        [TestMethod]
        public void Create()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("******");
            var publicKey = rsa.ToXmlString(false);
        }

        [TestMethod]
        public void Encrypt()
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString("******");
                var size = (rsaCryptoServiceProvider.KeySize / 8) - 11;
                var buffer = new byte[size];

                var valueBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new { date = DateTime.Now, eventSourceId = "******", message = "******", name = "******" }));
                using (MemoryStream inputStream = new MemoryStream(valueBytes), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, size);
                        if (readSize <= 0)
                            break;

                        var readBytes = new byte[readSize];
                        Array.Copy(buffer, 0, readBytes, 0, readSize);
                        
                        var encryptBytes = rsaCryptoServiceProvider.Encrypt(readBytes, false);
                        outputStream.Write(encryptBytes, 0, encryptBytes.Length);
                    }
                    var result = Convert.ToBase64String(outputStream.ToArray());
                }
            }
        }
    }
}
