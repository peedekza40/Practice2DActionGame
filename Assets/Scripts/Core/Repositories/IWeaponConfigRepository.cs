using Core.Constants;
using Infrastructure.Entities;

namespace Core.Repositories
{
    public interface IWeaponConfigRepository
    {
        WeaponConfig GetByItemType(ItemType type);
    } 
}


