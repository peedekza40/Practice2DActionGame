using Core.DataPersistence;
using Infrastructure.Dependency;
using Infrastructure.Entity;
using Infrastructure.InputSystem;
using UnityEngine;

public class StartupContext : MonoBehaviour
{
    private static StartupContext Instance { get; set; }

    public DataPersistenceManager DataPersistenceManager = default;
    public PlayerInputControl PlayerInputControl = default;
    public ItemAssets ItemAssets = default;
    public CharacterTemplates CharacterTemplates = default;

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Startup in the scene. Destroy the newest one.");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Configure();
    }

    private void Configure()
    {
        //gameobject on scene mono behavior
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(DataPersistenceManager),
                Factory = () => DataPersistenceManager,
                IsSingleton = true
            });
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(PlayerInputControl),
                Factory = () => PlayerInputControl,
                IsSingleton = true
            });
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(ItemAssets),
                Factory = () => ItemAssets,
                IsSingleton = true
            });
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(CharacterTemplates),
                Factory = () => CharacterTemplates,
                IsSingleton = true
            });

        //plain class
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(DbContextBuilder),
                Factory = () => new DbContextBuilder(),
                IsSingleton = true
            });
        DependenciesContext.Dependencies
            .Add(new Dependency {
                Type = typeof(IStatsConfigRepository),
                Factory = () => new StatsConfigRepository(),
                IsSingleton = true
            });

        //prefab mono behavior
        // DependenciesContext.Dependencies.Add(new Dependency { Type = typeof(ExampleDependencyPlainClass), Factory = () => new ExampleDependencyPlainClass(), IsSingleton = false });
    }
}
