using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    [ProtoInclude(1, typeof(SaveConfigMessage))]
    [ProtoInclude(2, typeof(SIgnInMessage))]
    [ProtoInclude(3, typeof(AuthorizeMessage))]
    [ProtoInclude(4, typeof(GetListOfConfigByPeriodMessage))]
    [ProtoInclude(5, typeof(LogOutMessage))]
    public abstract class ClientMessage:Message
    {
        [ProtoMember(1)]
        public string SessionKey { get; set; }
    }
}