using System;
using System.Collections.Generic;
using System.Linq;
using Character.Animators;
using Character.Combat;
using Character.Inventory;
using Character.Status;
using Collecting;
using Core.Constants;
using Core.DataPersistence;
using Core.DataPersistence.Data;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;
using static PlayerInputAction;

namespace Character
{
    public class PlayerHandler : MonoBehaviour, IPlayerActions, IDataPersistence
    {
        [Header("Assets")]
        public PlayerAnimatorController AnimatorController;
        public PlayerAttribute Attribute;
        public PlayerStatus Status;
        public PlayerCombat Combat;
        public PlayerController Controller;
        public Gold Gold;
        public InventoryManagement Inventory;

        [Header("Collect Items")]
        public LayerMask ItemLayer;
        public float CollectingRange = 0.5f;
        public Transform CenterPoint;

        public UnityAction InteractAction;

        public string LastCheckPointID { get; private set; }
        public List<string> CheckedPointIDs { get; private set; } = new List<string>();
        public List<string> OpenedChestIDs { get; private set; } = new List<string>();

        #region Dependencies
        [Inject]
        private PlayerInputControl playerInputControl;
        #endregion

        private void Awake() 
        {
            Inventory.UseItemAction += UseItem;    
        }

        private void OnEnable() 
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
           playerInputControl.PlayerInput.Player.SetCallbacks(this);
        }

        private void Update() 
        {
            AnimatorController.SetAssetsAnimation(
                Combat.CurrentWeapon != null, 
                Status.CurrentBoot?.HaveWeaponSpritePath);

            //collect items
            var items = Physics2D.OverlapCircleAll(CenterPoint.position, CollectingRange, ItemLayer).ToList();
            foreach (var item in items) 
            {
                CollectItem(item);
            }
        }

        private void CollectItem(Collider2D other)
        {
            ItemWorld itemWorld = other.GetComponent<ItemWorld>();
            if(itemWorld != null && itemWorld.IsCanCollect && !Inventory.IsFull(itemWorld.GetItem()))
            {
                Inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        private void UseItem(ItemModel item)
        {
            switch(item.Type)
            {
                case ItemType.HeathPotion : 
                    Status.AddCurrentHP(20);
                    break;
                case ItemType.ManaPotion : 
                    break;
            }
        }

        public void AddCheckedPointID(string checkPointID)
        {
            LastCheckPointID = checkPointID;
            CheckedPointIDs.Add(LastCheckPointID);
        }

        public void AddOpenedChestID(string chestID)
        {
            OpenedChestIDs.Add(chestID);
        }


        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(CenterPoint.position, CollectingRange);
        }

        #region IPlayerActions
        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Controller.StartJump();
            }
            else if(context.canceled)
            {
                Controller.FinishJump();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Combat.SetMeleeState();
            }
        }

        public void OnBlock(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Combat.StartBlockState();
            }
            else if(context.canceled)
            {
                Combat.FinishBlockState();
            }
        }

        public void OnToggleInventory(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                Inventory.ToggleInventory();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                InteractAction?.Invoke();
            }
        }
        #endregion

        public void LoadData(GameDataModel data)
        {
            LastCheckPointID = data.PlayerData.LastCheckPointID;
            CheckedPointIDs = data.CheckedPointIDs;
            OpenedChestIDs = data.OpenedChestIDs;
        }

        public void SaveData(GameDataModel data)
        {
            data.PlayerData.LastCheckPointID = LastCheckPointID;
            data.CheckedPointIDs = CheckedPointIDs;
            data.OpenedChestIDs = OpenedChestIDs;
        }
    }
}

