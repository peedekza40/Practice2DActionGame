using Core.Constants;
using Infrastructure.Entities;

namespace Core.Repositories
{
    public interface IEquipmentConfigRepository
    {
        EquipmentConfig GetByItemType(ItemType type);
    } 
}


