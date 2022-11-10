using LineRobot.Domain;
using LineRobot.Repository;
using LineRobot.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;

namespace LineRobot.Web.Api
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : Controller
    {
        private readonly ILogger logger;
        private readonly DocumentRepository documentRepository;
        private readonly CryptographyService cryptographyService;

        public DocumentController(
            ILogger<DocumentController> logger,
            DocumentRepository documentRepository,
            CryptographyService cryptographyService)
        {
            this.logger = logger;
            this.documentRepository = documentRepository;
            this.cryptographyService = cryptographyService;
        }

        [HttpPost]
        public ValidResult<IEnumerable<Document>> FetchByName([FromBody]JsonElement data)
        {
            var validResult = new ValidResult<IEnumerable<Document>>();

            var value = this.cryptographyService.Decrypt(data.GetProperty(CryptographyService.PROPERTY_NAME).GetString());

            if (!value.IsValid)
            {
                ValidResult<Document>.Add(validResult.ErrorMessages, value.ErrorMessages);
                validResult.Result = new List<Document>();
                return validResult;
            }
            
            string name = value.Result.name;
            validResult.Result = this.documentRepository.FetchBy(name);

            return validResult;
        }
    }
}