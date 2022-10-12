using UnityEngine;

namespace Core.DataPersistence.Data
{
    [System.Serializable]
    public class GameDataModel
    {
        [Header("Player Data")]
        public PlayerDataModel PlayerData;
        
        [Header("Scene")]
        public Vector3 CameraPosition;
        public string CurrentScene;

        public GameDataModel()
        {
            this.PlayerData = new PlayerDataModel();
            this.CameraPosition = Vector3.zero;
        }
    }
}

