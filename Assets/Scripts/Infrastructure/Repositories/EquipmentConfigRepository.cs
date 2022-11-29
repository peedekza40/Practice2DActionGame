using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using SqlCipher4Unity3D;

namespace Infrastructure.Repositories
{
    public class EquipmentConfigRepository : IEquipmentConfigRepository
    {
        public EquipmentConfig GetByItemType(ItemType type)
        {
            return Connection.Table<EquipmentConfig>().FirstOrDefault(x => x.ItemId == (int)type);
        }

        private SQLiteConnection Connection;

        public EquipmentConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    } 
}


