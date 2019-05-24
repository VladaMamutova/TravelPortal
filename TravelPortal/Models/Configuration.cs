using System;
using Npgsql;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    /// <summary>
    /// Класс Configuration представляет собой конфигурационные настройки
    /// для подключения к базе данных и содержит информацию о текущем клиенте БД.
    /// Является потокобезопасной реализацией паттерна Singleton.
    /// </summary>
    public class Configuration
    {
        const string DATABASE_NAME = "travelagencies";
        private readonly string _server;
        private readonly int _port;

        public User CurrentUser { get; private set; }

        private static Configuration _configuration;
        private static readonly object SyncRoot = new object();
        public string ConnectionString { get; private set; }

        private Configuration()
        {
            _server = "127.0.0.1";
            _port = 5432;
        }

        public static Configuration GetConfiguration()
        {
            if (_configuration == null)
            {
                lock (SyncRoot)
                {
                    _configuration = new Configuration();
                }
            }

            return _configuration;
        }

        public void SetUser(string userId, string password)
        {
            Roles currentRole = Roles.None;
            ConnectionString =
                $"Server = {_server}; User Id = {userId}; Database = {DATABASE_NAME}; " +
                $"Port = {_port}; Password = {password}";
            try
            {
                using (var connection =
                    new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    try
                    {
                        new NpgsqlCommand("set role " + Roles.Admin, connection)
                            .ExecuteNonQuery();
                        currentRole = Roles.Admin;
                    }
                    catch { }

                    try
                    {
                        new NpgsqlCommand("set role " + Roles.Supervisor,
                            connection).ExecuteNonQuery();
                        currentRole = Roles.Supervisor;
                    }
                    catch { }

                    try
                    {
                        new NpgsqlCommand("set role " + Roles.Employee,
                            connection).ExecuteNonQuery();
                        currentRole = Roles.Employee;
                    }
                    catch { }

                    if (currentRole == Roles.None)
                        throw new Exception(
                            "Не удалось авторизировать пользователя.");
                    CurrentUser = MainTables.GetCurrentUser(userId);
                    Queries.SetRole(currentRole, CurrentUser.GetAgencyId());
                }
            }
            catch
            {
                CurrentUser = null;
                ConnectionString = null;
                throw;
            }
        }

        public string GetConnectionStringForUser(string userId, string password)
        {
            return $"Server = {_server}; User Id = {userId}; Database = {DATABASE_NAME}; " +
                $"Port = {_port}; Password = {password}";
        }
    }
}