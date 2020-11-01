using Line;

namespace LineRobot.Domain
{
    /// <summary>
    /// Event source.
    /// </summary>
    public class EventSource : AbstractDomain
    {
        /// <summary>
        /// Gets or sets the type of the event source.
        /// </summary>
        /// <value>The type of the event source.</value>
        public EventSourceType EventSourceType { get; set; }

        /// <summary>
        /// Gets or sets the event source identifier.
        /// </summary>
        /// <value>The event source identifier.</value>
        public string EventSourceId { get; set; }

        /// <summary>
        /// Gets or sets the name of the event source.
        /// </summary>
        /// <value>The name of the event source.</value>
        public string EventSourceName { get; set; }

        /// <summary>
        /// Gets the event source identifier.
        /// </summary>
        /// <returns>The event source identifier.</returns>
        /// <param name="lineEvent">Line event.</param>
        public static string GetEventSourceId(ILineEvent lineEvent)
        {
            var result = string.Empty;
            switch (lineEvent.Source.SourceType)
            {
                case EventSourceType.User:
                    result = lineEvent.Source.User.Id;
                    break;
                case EventSourceType.Group:
                    result = lineEvent.Source.Group.Id;
                    break;
                case EventSourceType.Room:
                    result = lineEvent.Source.Room.Id;
                    break;
            }
            return result;
        }
    }
}