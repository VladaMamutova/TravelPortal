using System;
using Npgsql;

namespace TravelPortal.Database
{
    public static class Configuration
    {
        const string DATABASE_NAME = "travelagencies";
        private static readonly string Server;
        private static readonly int Port;
        private static string _userId;
        private static string _password;
        public static Roles Role;
        public enum Roles
        {
            None,
            Employee,
            Admin
        }

        static Configuration()
        {
            Server = "127.0.0.1";
            Port = 5432;
            //UserId = "postgres";
            //Password = "1111";
        }

        public static void SetUser(string userId, string password)
        {
            _userId = userId;
            _password = password;

            try
            {
                using (var connection =
                    new NpgsqlConnection(GetConnetionString()))
                {
                    connection.Open();
                    try
                    {
                        new NpgsqlCommand(
                                "set role " + Roles.Admin.ToString().ToLower(),
                                connection)
                            .ExecuteNonQuery();
                        Role = Roles.Admin;

                    }
                    catch { }

                    try
                    {
                        new NpgsqlCommand(
                                "set role " + Roles.Employee.ToString().ToLower(),
                                connection)
                            .ExecuteNonQuery();
                        Role = Roles.Employee;

                    }
                    catch { }

                    if (Role == Roles.None) throw new Exception("Не удалось авторизировать пользователя.");
                }
            }
            catch
            {
                _userId = null;
                _password = null;
                throw;
            }
        }

        public static string GetConnetionString()
        {
            return $"Server = {Server}; User Id = {_userId}; Database = {DATABASE_NAME}; " +
                   $"Port = {Port}; Password = {_password}";
        }
    }
}