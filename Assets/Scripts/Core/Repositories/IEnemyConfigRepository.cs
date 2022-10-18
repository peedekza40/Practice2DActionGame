using Core.Constants;
using Infrastructure.Entity;

namespace Core.Repositories
{
    public interface IEnemyConfigRepository
    {
        EnemyConfig GetByType(EnemyType type);
    }
}
