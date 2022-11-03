using Core.Constants;
using Core.Repositories;
using Infrastructure.Entities;
using SqlCipher4Unity3D;

namespace Infrastructure.Repositories
{
    public class WeaponConfigRepository : IWeaponConfigRepository
    {
        public WeaponConfig GetByItemType(ItemType type)
        {
            return Connection.Table<WeaponConfig>().FirstOrDefault(x => x.ItemId == (int)type);
        }

        private SQLiteConnection Connection;

        public WeaponConfigRepository(DbContextBuilder dbContextBuilder)
        {
            Connection = dbContextBuilder.Connection;
        }
    } 
}


