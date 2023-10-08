using Core.Configs;
using Core.DataPersistence;
using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.InputSystem;
using Infrastructure.Repositories;
using Zenject;

namespace Infrastructure.Dependency
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAppSettingsContext>().To<AppSettingsContext>().FromComponentInHierarchy().AsTransient();
            Container.Bind<DataPersistenceManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerInputControl>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GeneralFunction>().FromComponentInHierarchy().AsSingle();

            Container.Bind<DbContextBuilder>().AsSingle();
            Container.Bind<IStatsConfigRepository>().To<StatsConfigRepository>().AsSingle();
            Container.Bind<IItemConfigRepository>().To<ItemConfigRepository>().AsSingle();
            Container.Bind<IEnemyConfigRepository>().To<EnemyConfigRepository>().AsSingle();
            Container.Bind<IEquipmentConfigRepository>().To<EquipmentConfigRepository>().AsSingle();
        }
    }
}