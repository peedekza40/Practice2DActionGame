using System.Collections.Generic;
using Core.Repositories;
using Infrastructure.Entities;
using SqlCipher4Unity3D;
using Zenject;

namespace Infrastructure.Repositories
{
    public class StatsConfigRepository : IStatsConfigRepository
    {
        public List<StatsConfig> Get()
        {
            return Connection.Table<StatsConfig>().ToList();
        }

        private SQLiteConnection Connection;
        
        public StatsConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    }
}
