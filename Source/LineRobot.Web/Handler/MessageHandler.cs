using Line;
using LineRobot.Domain;
using LineRobot.Domain.Interface;
using LineRobot.Repository;
using LineRobot.Service;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LineRobot.Web.Handler
{
    public class MessageHandler : IHandler
    {
        private readonly HandleRepository handleRepository;
        private readonly CryptographyService cryptographyService;
        private readonly IHttpClientFactory httpClientFactory;
        public LineEventType LineEventType => LineEventType.Message;

        public MessageHandler(
            HandleRepository handleRepository,
            CryptographyService cryptographyService,
            IHttpClientFactory httpClientFactory)
        {
            this.handleRepository = handleRepository;
            this.cryptographyService = cryptographyService;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Handle(ILineBot lineBot, ILineEvent lineEvent)
        {
            if (string.IsNullOrEmpty(lineEvent.Message.Text))
                return;

            if (lineEvent.ReplyToken == "00000000000000000000000000000000" || lineEvent.ReplyToken == "ffffffffffffffffffffffffffffffff")
                return;

            var eventSourceId = EventSource.GetEventSourceId(lineEvent);
            if (lineEvent.Message.Text.Replace(" ", "").Equals("GetEventSourceId", StringComparison.InvariantCultureIgnoreCase))
            {
                await lineBot.Reply(lineEvent.ReplyToken, new TextMessage(eventSourceId));
            }
            else
            {
                var splits = lineEvent.Message.Text.Split(' ');
                if (splits.Length > 1)
                {
                    var keyWord = splits[0];
                    var handles = this.handleRepository.FetchBy(eventSourceId, keyWord);
                    foreach (var handle in handles)
                    {
                        var message = splits[1];
                        
                        var encryptValue = this.cryptographyService.Encrypt(handle.PublicKey, JsonSerializer.Serialize(new { date = DateTime.Now, eventSourceId, message }));

                        try
                        {
                            var client = httpClientFactory.CreateClient();

                            var requestMessage = new HttpRequestMessage(HttpMethod.Post, new Uri(handle.Url));
                            requestMessage.Content = new StringContent(JsonSerializer.Serialize(new { encryptValue }), Encoding.UTF8, "application/json");

                            var response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

                            if (response.IsSuccessStatusCode)
                            {
                                using var stream = response.Content.ReadAsStreamAsync().Result;
                                using (var streamReader = new StreamReader(stream))
                                {
                                    var value = streamReader.ReadToEnd();
                                    var document = JsonSerializer.Deserialize<Document>(value, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                                    await lineBot.Push(document.EventSourceId, new TextMessage(document.Value));
                                }
                            }
                            else
                            {
                                await lineBot.Push(eventSourceId, new TextMessage(
                                 $"Message：{lineEvent.Message.Text} \n" +
                                 $"IsSuccessStatusCode：{response.IsSuccessStatusCode} \n" +
                                 $"StatusCode：{response.StatusCode}"));
                            }
                        }
                        catch (Exception exception)
                        {
                            await lineBot.Push(eventSourceId, new TextMessage(
                                 $"Message：{lineEvent.Message.Text} \n" +
                                 $"ErrorMessage：{exception.ToString()} \n"));
                        }
                    }
                }
            }
        }
    }
}