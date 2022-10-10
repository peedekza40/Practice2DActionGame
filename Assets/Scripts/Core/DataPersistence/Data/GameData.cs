using UnityEngine;

namespace Core.DataPersistence.Data
{
    [System.Serializable]
    public class GameData
    {
        public float PlayerHP;
        public Vector3 PlayerPosition;
        public Vector3 Scale;
        
        public Vector3 CameraPosition;
        public string CurrentScene;

        public GameData()
        {
            this.PlayerHP = 0;
            this.PlayerPosition = Vector3.zero;
            this.CameraPosition = Vector3.zero;
        }
    }
}

