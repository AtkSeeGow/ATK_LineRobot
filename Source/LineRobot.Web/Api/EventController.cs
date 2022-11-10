using Line;
using LineRobot.Domain;
using LineRobot.Domain.Interface;
using LineRobot.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace LineRobot.Web.Api
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class EventController : Controller
    {
        private readonly ILogger<EventController> logger;
        private readonly ILineBot lineBot;
        private readonly IServiceProvider serviceProvider;
        private readonly CryptographyService cryptographyService;

        public EventController(
            ILogger<EventController> logger,
            ILineBot lineBot,
            IServiceProvider serviceProvider,
            CryptographyService cryptographyService)
        {
            this.logger = logger;
            this.lineBot = lineBot;
            this.serviceProvider = serviceProvider;
            this.cryptographyService = cryptographyService;
        }

        [HttpPost]
        public async void Handle()
        {
            var events = await lineBot.GetEvents(Request);
            foreach (var @event in events)
            {
                var eventHandlers = serviceProvider.GetServices<IHandler>().Where(item => item.LineEventType == @event.EventType);
                foreach (var eventHandler in eventHandlers)
                    await eventHandler.Handle(lineBot, @event);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost]
        public async Task<ValidResult<dynamic>> Push([FromBody]JsonElement data)
        {
            var result = new ValidResult<dynamic>();

            var value = this.cryptographyService.Decrypt(data.GetProperty(CryptographyService.PROPERTY_NAME).GetString());

            if (!value.IsValid)
                return value;

            string eventSourceId = value.Result.eventSourceId;
            string message = value.Result.message;

            await lineBot.Push(eventSourceId, new TextMessage() { Text = message });

            return result;
        }
    }
}