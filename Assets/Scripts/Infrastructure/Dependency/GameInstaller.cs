using Core.Configs;
using Core.DataPersistence;
using Core.Repositories;
using Infrastructure.Entities;
using Infrastructure.InputSystem;
using Infrastructure.Repositories;
using UnityEngine;
using Zenject;

namespace Infrastructure.Dependency
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DataPersistenceManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerInputControl>().FromComponentInHierarchy().AsSingle();

            AppSettingsModel configure = new AppSettingsModel();
            Container.Bind<IAppSettingsContext>()
                .FromInstance(new AppSettingsContext(configure))
                .AsSingle()
                .NonLazy();
            Container.Bind<DbContextBuilder>()
                .AsSingle()
                .WithArguments(configure);
            Container.Bind<IStatsConfigRepository>().To<StatsConfigRepository>().AsSingle();
            Container.Bind<IItemConfigRepository>().To<ItemConfigRepository>().AsSingle();
            Container.Bind<IEnemyConfigRepository>().To<EnemyConfigRepository>().AsSingle();
        }
    }
}