using Character;
using Character.Inventory;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "Installers/GameSettingInstaller")]
public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
{
    public ItemAssets ItemAssets;
    public CharacterTemplates CharacterTemplates;

    public override void InstallBindings()
    {
        Container.BindInstance(ItemAssets).AsSingle();
        Container.BindInstance(CharacterTemplates).AsSingle();
    }
}