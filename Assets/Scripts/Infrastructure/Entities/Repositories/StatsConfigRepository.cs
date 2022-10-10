using System.Data;
using Infrastructure.Dependency;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Infrastructure.Entity
{
    public class StatsConfigRepository : IStatsConfigRepository
    {

        public void Get()
        {
            Connection.Open();
            using(var command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM StatsConfig;";
                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Debug.Log($"Code : {reader["Code"]}, Name : {reader["Name"]}");
                    }
                }
            }
            Connection.Close();
        }

        private SqliteConnection Connection;

        public StatsConfigRepository()
        {
            Connection = DependenciesContext.Dependencies.Get<DbContextBuilder>().Connection;
        }
    }
}
