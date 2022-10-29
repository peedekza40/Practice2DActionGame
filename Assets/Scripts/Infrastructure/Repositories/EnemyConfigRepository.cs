using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using SqlCipher4Unity3D;

namespace Infrastructure.Repositories
{
    public class EnemyConfigRepository : IEnemyConfigRepository 
    {
        public EnemyConfig GetById(EnemyId type)
        {
            return Connection.Table<EnemyConfig>().FirstOrDefault(x => x.Id == (int)type);
        }

        private SQLiteConnection Connection;

        public EnemyConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    }
}