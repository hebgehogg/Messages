using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using Messages.Server;
using ProtoBuf.WellKnownTypes;

namespace Utils
{
    public sealed class SessionKeyManager
    {
        private readonly List<Tuple<string, string, DateTime>> _sessionKeys;

        public Timer _sessionKeyTimer;

        public SessionKeyManager()
        {
            _sessionKeys = new List<Tuple<string, string, DateTime>>();
            SetTimer();
        }

        private void SetTimer()
        {
            _sessionKeyTimer = new Timer(500);
            _sessionKeyTimer.Elapsed += OnTimedEvent;
            _sessionKeyTimer.AutoReset = true;
            _sessionKeyTimer.Enabled = true;
        }
        
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            var deleteItems = _sessionKeys.Where(x => (DateTime.Now - x.Item3) > TimeSpan.FromMinutes(30)).ToArray();
            foreach (var a in deleteItems)
                _sessionKeys.Remove(a);
        }

        public string GenerateKey(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            string key = Guid.NewGuid().ToString();

            _sessionKeys.Add(Tuple.Create(login, key, DateTime.Now));
            return key;
        }
        
        public void DeleteSessionKey(string sessionKey)
        {
            var deleteItem = _sessionKeys.Single(x => x.Item2 == sessionKey);
            _sessionKeys.Remove(deleteItem);
        }
        
        public string GetLoginBySessionKey(string sessionKey)
        {
            return _sessionKeys.Single(x => x.Item2 == sessionKey).Item1;
        }
    }
}