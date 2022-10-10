using Core.DataPersistence.Data;

namespace Core.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameData data);

        void SaveData(ref GameData data);
    }
}

