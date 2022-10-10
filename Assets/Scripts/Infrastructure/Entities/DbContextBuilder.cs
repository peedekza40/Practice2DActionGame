using Mono.Data.Sqlite;

namespace Entity
{
    public class DbContextBuilder
    {
        private string DbName = "URI=file:PlatformRPG.db";

        public SqliteConnection GetConnection()
        {
            return new SqliteConnection(DbName);
        }
    }
}

