using Core.Constants;
using Core.Repositories;
using Infrastructure.Entity;
using SqlCipher4Unity3D;

namespace Infrastructure.Repositories
{
    public class EnemyConfigRepository : IEnemyConfigRepository 
    {
        public EnemyConfig GetByType(EnemyType type)
        {
            return Connection.Table<EnemyConfig>().FirstOrDefault(x => x.TypeId == (int)type);
        }

        private SQLiteConnection Connection;

        public EnemyConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    }
}