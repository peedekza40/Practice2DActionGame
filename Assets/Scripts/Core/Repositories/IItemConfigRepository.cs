using Core.Constants;
using Infrastructure.Entity;

namespace Core.Repositories
{
    public interface IItemConfigRepository
    {
        ItemConfig GetByType(ItemType type);
    }
}