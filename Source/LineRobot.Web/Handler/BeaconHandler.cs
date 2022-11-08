using Line;
using LineRobot.Domain;
using LineRobot.Domain.Interface;
using System.Threading.Tasks;

namespace LineRobot.Web.Handler
{
    public class BeaconHandler : IHandler
    {
        public LineEventType LineEventType => LineEventType.Beacon;

        public BeaconHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lineBot"></param>
        /// <param name="lineEvent"></param>
        /// <returns></returns>
        public async Task Handle(ILineBot lineBot, ILineEvent lineEvent)
        {
            if (lineEvent.Beacon.BeaconType == BeaconType.Unknown)
                return;

            var eventSourceId = EventSource.GetEventSourceId(lineEvent);
            if (string.IsNullOrEmpty(eventSourceId))
                return;
        }
    }
}