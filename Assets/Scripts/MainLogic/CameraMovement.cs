using Character.Interfaces;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using UnityEngine;

namespace Character
{
    public class CameraMovement : MonoBehaviour, IDataPersistence
    {
        public GameObject Target;
        public float FollowSpeed = 5f;
        public float OffsetY = 1.4f;
        public float OffsetZ = -6.5f;

        // Update is called once per frame
        void Update()
        {
            var newPositionZ = OffsetZ;
            var newPositionY = Target.transform.position.y + OffsetY;
            if(Target.transform.position.y >= 5.0f)
            {
                newPositionZ += -(Target.transform.position.y / 2);
                newPositionY -= OffsetY;
            }

            Vector3 newPos = new Vector3(Target.transform.position.x, Target.transform.position.y + OffsetY, newPositionZ);
            transform.position =  Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }

        public void LoadData(GameDataModel data)
        {
            transform.position = data.CameraPosition;
        }

        public void SaveData(GameDataModel data)
        {
            data.CameraPosition = transform.position;
        }
    }
}
