using Core.Constants;
using Infrastructure.Entities;

namespace Core.Repositories
{
    public interface IEnemyConfigRepository
    {
        EnemyConfig GetById(EnemyId type);
    }
}
