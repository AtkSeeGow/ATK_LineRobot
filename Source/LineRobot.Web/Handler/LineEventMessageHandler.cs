using Line;
using LineRobot.Domain;
using LineRobot.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace LineRobot.Web.Handler
{
    public class LineEventMessageHandler : ILineEventHandler
    {
        public LineEventMessageHandler()
        {
        }

        public LineEventType LineEventType => LineEventType.Message;

        public async Task Handle(ILineBot lineBot, ILineEvent lineEvent)
        {
            if (string.IsNullOrEmpty(lineEvent.Message.Text))
                return;

            if (lineEvent.ReplyToken == "00000000000000000000000000000000" || lineEvent.ReplyToken == "ffffffffffffffffffffffffffffffff")
                return;

            if (lineEvent.Message.Text.Replace(" ", "").Equals("GetEventSourceId", StringComparison.InvariantCultureIgnoreCase))
            {
                var eventSourceId = EventSource.GetEventSourceId(lineEvent);
                await lineBot.Reply(lineEvent.ReplyToken, new TextMessage(eventSourceId));
            }
        }
    }
}