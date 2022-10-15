using Core.DataPersistence;
using Core.Repositories;
using Infrastructure.Entity;
using Infrastructure.InputSystem;
using Infrastructure.Repositories;
using Zenject;

namespace Infrastructure.Dependency
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DataPersistenceManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerInputControl>().FromComponentInHierarchy().AsSingle();

            Container.Bind<DbContextBuilder>().AsSingle();
            Container.Bind<IStatsConfigRepository>().To<StatsConfigRepository>().AsSingle();
            Container.Bind<IItemConfigRepository>().To<ItemConfigRepository>().AsSingle();
        }
    }
}