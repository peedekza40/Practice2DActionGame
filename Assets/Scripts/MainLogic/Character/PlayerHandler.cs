using Character.Animators;
using Character.Combat;
using Character.Inventory;
using Character.Status;
using Collecting;
using Core.Constants;
using Infrastructure.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Zenject;
using static PlayerInputAction;

namespace Character
{
    public class PlayerHandler : MonoBehaviour, IPlayerActions
    {
        [Header("Assets")]
        public PlayerAnimatorController AnimatorController;
        public PlayerStatus Status;
        public PlayerCombat Combat;
        public PlayerController Controller;
        public Gold Gold;
        public InventoryManagement Inventory;

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
        }

        private void OnTriggerEnter2D(Collider2D other) 
        {
            CollectItem(other);
        }
        
        private void OnTriggerStay2D(Collider2D other) 
        {
            CollectItem(other);
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
        }
        #endregion
    }
}

