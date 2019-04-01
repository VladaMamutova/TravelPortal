namespace TravelPortal.Database
{
    public static class Configuration
    {
        const string DATABASE_NAME = "tralelagencies";
        public static string Server { get; set; }
        public static int Port { get; set; }
        public static string UserId { get; set; }
        public static string Password { get; set; }

        static Configuration()
        {
            Server = "127.0.0.1";
            Port = 5432;
            UserId = "postgres";
            Password = "1111";
        }

        public static void SetUser(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }

        public static string GetConnetionString()
        {
            return $"Server = {Server}; User Id = {UserId}; Database = {DATABASE_NAME}; " +
                   $"Port = {Port}; Password = {Password}";
        }
    }
}