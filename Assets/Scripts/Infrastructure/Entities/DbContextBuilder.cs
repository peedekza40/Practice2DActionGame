using System.Data.Common;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Infrastructure.Entity
{
    public class DbContextBuilder
    {
        public SqliteConnection Connection;

        private string DbName = $"URI=file:{Application.dataPath}/Database/PlatformRPG.db";

        public DbContextBuilder()
        {
            Connection = new SqliteConnection(DbName);
        }
    }
}

