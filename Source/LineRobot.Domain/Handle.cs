using System.Runtime.Serialization;

namespace LineRobot.Domain
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Handle : AbstractDomain
    {
        /// <summary>
        /// 來源代號
        /// </summary>
        [DataMember]
        public string EventSourceId { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// 關鍵字
        /// </summary>
        [DataMember]
        public string KeyWord { get; set; }

        /// <summary>
        /// 公鑰
        /// </summary>
        [DataMember]
        public string PublicKey { get; set; }

        /// <summary>
        /// 轉呼叫網址
        /// </summary>
        [DataMember]
        public string Url { get; set; }
    }
}