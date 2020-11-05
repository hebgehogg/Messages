using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    [ProtoInclude(3, typeof(SaveConfigMessage))]
    [ProtoInclude(4, typeof(LogInMessage))]
    [ProtoInclude(5, typeof(RegistrationMessage))]
    [ProtoInclude(6, typeof(GetListOfConfigByPeriodMessage))]
    [ProtoInclude(7, typeof(LogOutMessage))]
    public abstract class ClientMessage:Message
    {
        [ProtoMember(1)]
        public string SessionKey { get; set; }
    }
}