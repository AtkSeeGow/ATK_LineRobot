using System;
using System.Runtime.Serialization;
using LineRobot.Domain.Interface;

namespace LineRobot.Domain
{
    [DataContract]
    public partial class AbstractDomain : IIdentifier<Guid>
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
