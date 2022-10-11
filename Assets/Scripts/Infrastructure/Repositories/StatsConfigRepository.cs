using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Repositories;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using Mono.Data.Sqlite;
using UnityEngine;

namespace Infrastructure.Repositories
{
    public class StatsConfigRepository : IStatsConfigRepository
    {
        public List<StatsConfig> Get()
        {
            return GetAll().ToList();
        }

        private IQueryable<StatsConfig> GetAll()
        {
            List<StatsConfig> statsConfigs = new List<StatsConfig>();
            Connection.Open();
            using(var command = Connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM StatsConfig;";
                using(IDataReader reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        StatsConfig statsConfig = new StatsConfig()
                        {
                            Id = reader.GetInt32(0),
                            Code = reader.GetString(1),
                            Name = reader.GetString(2),
                            MainIconPath = reader.GetString(3),
                            SubIconPath = reader.GetString(4)
                        };

                        statsConfigs.Add(statsConfig);
                    }
                }
            }
            Connection.Close();

            return statsConfigs.AsQueryable();
        }

        private SqliteConnection Connection;

        public StatsConfigRepository()
        {
            Connection = DependenciesContext.Dependencies.Get<DbContextBuilder>().Connection;
        }
    }
}
