using System.Linq;
using Character.Interfaces;
using Character.Inventory;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.InputSystem;
using UnityEngine;
using Zenject;

namespace Character
{
    public class CameraMovement : MonoBehaviour, IDataPersistence
    {
        public GameObject Target;
        public float FollowSpeed = 5f;
        public Vector3 Offset = new Vector3(0, 0.6f, -6.5f);
        public Vector3 OffsetOnOpenInventory = new Vector3(0.6f, -0.5f, -3.5f);
        public Vector3 OffsetOnOpenStatus = new Vector3(0.6f, -0.5f, -3.5f);

        #region Dependencies
        [Inject]
        private PlayerInputControl playerInputControl;
        #endregion

        // Update is called once per frame
        void Update()
        {
            Vector3 newPos = new Vector3(Target.transform.position.x + Offset.x, Target.transform.position.y + Offset.y, Offset.z);

            if(playerInputControl.UIPersistences.Any(x => x.Number == UINumber.Inventory && x.IsOpen))
            {
                newPos = new Vector3(Target.transform.position.x + OffsetOnOpenInventory.x, Target.transform.position.y + OffsetOnOpenInventory.y, OffsetOnOpenInventory.z);
            }
            else if(playerInputControl.UIPersistences.Any(x => x.Number == UINumber.Statistic && x.IsOpen))
            {
                newPos = new Vector3(Target.transform.position.x + OffsetOnOpenStatus.x, Target.transform.position.y + OffsetOnOpenStatus.y, OffsetOnOpenStatus.z);
            }

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
