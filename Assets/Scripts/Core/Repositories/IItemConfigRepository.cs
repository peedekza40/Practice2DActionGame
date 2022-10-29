using Core.Constants;
using Infrastructure.Entities;

namespace Core.Repositories
{
    public interface IItemConfigRepository
    {
        ItemConfig GetById(ItemId type);
    }
}