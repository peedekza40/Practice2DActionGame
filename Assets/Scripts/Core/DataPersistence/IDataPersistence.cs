using Core.DataPersistence.Data;

namespace Core.DataPersistence
{
    public interface IDataPersistence
    {
        void LoadData(GameDataModel data);

        void SaveData(GameDataModel data);
    }
}

