using LineRobot.Domain;
using LineRobot.Domain.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LineRobot.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class CryptographyService
    {
        public static readonly string PROPERTY_NAME = "encryptValue";

        private readonly ILogger logger;

        private readonly TokenOptions tokenOptions;

        public CryptographyService(
            ILogger<CryptographyService> logger,
            TokenOptions tokenOptions)
        {
            this.logger = logger;
            this.tokenOptions = tokenOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptValue"></param>
        /// <returns></returns>
        public ValidResult<dynamic> Decrypt(string encryptValue)
        {
            var validResult = new ValidResult<dynamic>();

            if (string.IsNullOrEmpty(encryptValue))
                validResult.ErrorMessages.Add(Guid.NewGuid().ToString(), "傳入的加密字串是空白");

            try
            {
                using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
                {
                    rsaCryptoServiceProvider.FromXmlString(this.tokenOptions.PrivateKey);

                    var value = this.decrypt(encryptValue);

                    dynamic data = JObject.Parse(value);
                    DateTime? date = data.date;

                    if (!date.HasValue)
                        validResult.ErrorMessages.Add(Guid.NewGuid().ToString(), "傳入的加密字串格式不符");

                    var timeSpan = DateTime.Now - date.Value;

                    if (validResult.IsValid && timeSpan.TotalMinutes < 5)
                    {
                        validResult.Result = data;
                        return validResult;
                    }

                    validResult.ErrorMessages.Add(Guid.NewGuid().ToString(), "傳入的加密字串已過期");
                }
            }
            catch(Exception exception)
            {
                this.logger.LogError(exception.ToString());
                validResult.ErrorMessages.Add(Guid.NewGuid().ToString(), exception.ToString());
            }

            return validResult;
        }

        public string Encrypt(string publicKey, string value)
        {
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider())
            {
                rsaCryptoServiceProvider.FromXmlString(publicKey);
                var size = (rsaCryptoServiceProvider.KeySize / 8) - 11;
                var buffer = new byte[size];

                using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(value)), outputStream = new MemoryStream())
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
                    return Convert.ToBase64String(outputStream.ToArray());
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptValue"></param>
        /// <returns></returns>
        private string decrypt(string encryptValue)
        {
            using (var rsaProvider = new RSACryptoServiceProvider())
            {
                rsaProvider.FromXmlString(this.tokenOptions.PrivateKey);

                int size = rsaProvider.KeySize / 8;
                var buffer = new byte[size];

                var encryptBytes = Convert.FromBase64String(encryptValue);
                using (MemoryStream inputStream = new MemoryStream(encryptBytes), outputStream = new MemoryStream())
                {
                    while (true)
                    {
                        int readSize = inputStream.Read(buffer, 0, size);
                        if (readSize <= 0)
                            break;

                        var readBytes = new byte[readSize];
                        Array.Copy(buffer, 0, readBytes, 0, readSize);

                        var decryptBytes = rsaProvider.Decrypt(readBytes, false);
                        outputStream.Write(decryptBytes, 0, decryptBytes.Length);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }
    }
}
