using Core.Constants;
using Core.Repositories;
using Infrastructure.Entity;
using SqlCipher4Unity3D;
using Zenject;

namespace Infrastructure.Repositories
{
    public class ItemConfigRepository : IItemConfigRepository 
    {
        public ItemConfig GetByType(ItemType type)
        {
            return Connection.Table<ItemConfig>().FirstOrDefault(x => x.TypeId == (int)type);
        }

        private SQLiteConnection Connection;

        public ItemConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    }
}