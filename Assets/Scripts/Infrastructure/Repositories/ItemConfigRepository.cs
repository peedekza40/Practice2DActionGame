using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using SqlCipher4Unity3D;

namespace Infrastructure.Repositories
{
    public class ItemConfigRepository : IItemConfigRepository 
    {
        public ItemConfig GetById(ItemId type)
        {
            return Connection.Table<ItemConfig>().FirstOrDefault(x => x.Id == (int)type);
        }

        private SQLiteConnection Connection;

        public ItemConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    }
}