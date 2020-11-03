using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Runtime.Intrinsics.X86;
using Messages;
using Messages.Server;
using Server.Exceptions;
using Utils;

namespace Server
{
    public sealed class SqLiteDataBaseProvider: IDatabaseProvider
    {
        private string _connectionString = @"Data Source=D:\Desktop\AeroSet\Education\DataBase\Server.db";
        private readonly SQLiteConnection _sqLiteConnection;
        private readonly SessionKeyManager _sessionKeyManager;

        public SqLiteDataBaseProvider(SessionKeyManager sessionKeyManager)
        {
            if (sessionKeyManager == null) throw new ArgumentNullException(nameof(sessionKeyManager));

            _sessionKeyManager = sessionKeyManager;
            _sqLiteConnection = new SQLiteConnection(_connectionString);
            _sqLiteConnection.Open();
        }
        
        public bool SaveConfig(string sessionKey, HardwareConfig config)
        {
            var cmd = _sqLiteConnection.CreateCommand();
            string userID = default;
            try
            {
                userID = $"SELECT UserID FROM Users WHERE login = '{_sessionKeyManager.GetLoginBySessionKey(sessionKey)}'";
                cmd.CommandText = userID;
                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            
            var existUser = $"INSERT INTO Configs VALUES(null,'{config.CpuCoreCount}', '{config.ClockFrequency}', '{config.RandomAccessMemory}','{config.DiskSpace}, '{userID}', '{DateTime.Now}')";
            cmd.CommandText = existUser;
            cmd.ExecuteNonQuery();
            return true;
        }

        public HardwareConfig[] GetListOfConfig(string sessionKey, DateTime from, DateTime to)
        {
            var cmd = _sqLiteConnection.CreateCommand();
            string userID = default;
            try
            {
                userID = $"SELECT UserID FROM Users WHERE login = '{_sessionKeyManager.GetLoginBySessionKey(sessionKey)}'";
                cmd.CommandText = userID;
                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            var getHardwareConfigs = $"SELECT CpuCoreCont, ClockFrequency, RandomAccessMemory, DiskSpace FROM Configs WHERE ConfigData BETWEEN '{from}' AND '{to}'";
            cmd.CommandText = getHardwareConfigs;
            
            var listOfConfigs = new List<HardwareConfig>();
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var hardwareConfigs = new HardwareConfig()
                    {
                        CpuCoreCount = reader.GetInt32(reader.GetOrdinal("CpuCoreCount")),
                        ClockFrequency = reader.GetInt32(reader.GetOrdinal("ClockFrequency")),
                        RandomAccessMemory = reader.GetDouble(reader.GetOrdinal("RandomAccessMemory")),
                        DiskSpace = reader.GetDouble(reader.GetOrdinal("DiskSpace")),
                    };
                    listOfConfigs.Add(hardwareConfigs);
                }
            }

            return listOfConfigs.ToArray();
        }

        public void Registration(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            var cmd = _sqLiteConnection.CreateCommand();
            var existUser = $"SELECT Login FROM Users WHERE Login = {login}";
            cmd.CommandText = existUser;

            if (cmd.ExecuteScalar()==login)
                throw new ExistingUserException();
            else
            {
                var createUser = $"INSERT INTO Users VALUES(null,'{login}')";
                cmd.CommandText = createUser;
                cmd.ExecuteNonQuery();
            }
        }

        public string SignIn(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            
            var cmd = _sqLiteConnection.CreateCommand();
            var existUser = $"SELECT Login FROM Users WHERE Login = {login}";
            cmd.CommandText = existUser;
            if (cmd.ExecuteScalar()==login)
                return _sessionKeyManager.GenerateKey(login);
            else
                throw new InvalidOperationException();
        }

        public bool LogOut(string sessionKey)
        {
            if (sessionKey == null) throw new ArgumentNullException(nameof(sessionKey));
            try
            {
                _sessionKeyManager.DeleteSessionKey(sessionKey);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
        
        public void Dispose()
        {
            _sqLiteConnection.Close();
        }
    }
}