using System;
using Messages;

namespace Server
{
    public interface IDatabaseProvider: IDisposable
    {
        public bool SaveConfig(string sessionKey, HardwareConfig config);
        
        public HardwareConfig[] GetListOfConfig(string sessionKey, DateTimeOffset from, DateTimeOffset to);
        
        public void Registration(string login);
        
        public string SignIn(string login);

        public bool LogOut(string sessionKey);
        
    }
}