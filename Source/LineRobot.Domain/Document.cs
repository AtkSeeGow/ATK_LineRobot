using System.Runtime.Serialization;

namespace LineRobot.Domain
{
    /// <summary>
    /// 文件
    /// </summary>
    [DataContract]
    public class Document : AbstractDomain
    {
        /// <summary>
        /// 來源代號
        /// </summary>
        [DataMember]
        public string EventSourceId { get; set; }

        /// <summary>
        /// 文件名稱
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 文件值
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}