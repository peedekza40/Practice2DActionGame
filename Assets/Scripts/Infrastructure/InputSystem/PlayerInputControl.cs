using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Infrastructure.InputSystem
{
    public class PlayerInputControl : MonoBehaviour
    {
        public Vector2 Move => MoveInput.ReadValue<Vector2>();

        public List<IUIPersistence> UIPersistences;
        private List<MouseEvent> UIMouseEvents { get; set; } = new List<MouseEvent>();

        public PlayerInputAction PlayerInput;
        public InputAction MoveInput { get; private set; }
        public InputAction JumpInput { get; private set; }
        public InputAction AttackInput { get; private set; }
        public InputAction BlockInput { get; private set; }
        public InputAction ToggleInventoryInput { get; private set; }
        public InputAction TogglePauseMenuInput { get; private set; }
        public InputAction InteractInput { get; private set; }

        private void Awake() 
        {
            PlayerInput = new PlayerInputAction();
            MoveInput = PlayerInput.Player.Move;
            JumpInput = PlayerInput.Player.Jump;
            AttackInput = PlayerInput.Player.Attack;
            ToggleInventoryInput = PlayerInput.Player.ToggleInventory;
            TogglePauseMenuInput = PlayerInput.Player.TogglePauseMenu;
            BlockInput = PlayerInput.Player.Block;
            InteractInput = PlayerInput.Player.Interact;
        }

        private void OnEnable() 
        {
            MoveInput.Enable();
            JumpInput.Enable();
            AttackInput.Enable();
            BlockInput.Enable();
            ToggleInventoryInput.Enable();
            InteractInput.Enable();
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() 
        {
            MoveInput.Disable();
            JumpInput.Disable();
            AttackInput.Disable();
            BlockInput.Disable();
            ToggleInventoryInput.Disable();
            InteractInput.Disable();

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UIPersistences = FindAllUIPersistenceObjects();
            UIMouseEvents = UIPersistences.Select(x => x.MouseEvent).ToList();
        }

        private void Update() 
        {
            //disabled combat
            if(UIMouseEvents.Any(x => x.IsHover))
            {
                AttackInput.Disable();
                BlockInput.Disable();
            }
            else
            {
                AttackInput.Enable();
                BlockInput.Enable();
            }

            //disabled movement && inventory && statistic
            bool pauseMenuIsOpen = UIPersistences.Any(x => x.Number == UINumber.PauseMenu && x.IsOpen);
            bool deathMenuIsOpen = UIPersistences.Any(x => x.Number == UINumber.DeathMenu && x.IsOpen);
            bool statisticIsOpen = UIPersistences.Any(x => x.Number == UINumber.Statistic && x.IsOpen);
            if(deathMenuIsOpen || pauseMenuIsOpen || statisticIsOpen)
            {
                MoveInput.Disable();
                JumpInput.Disable();
                ToggleInventoryInput.Disable();
            }
            else
            {
                MoveInput.Enable();
                JumpInput.Enable();
                ToggleInventoryInput.Enable();
                TogglePauseMenuInput.Enable();
            }

            //set cursor visible
            if(UIPersistences.Any(x => x.IsOpen))
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }
        }

        private List<IUIPersistence> FindAllUIPersistenceObjects()
        {
            IEnumerable<IUIPersistence> uiPersistences = FindObjectsOfType<MonoBehaviour>(true).OfType<IUIPersistence>();
            return new List<IUIPersistence>(uiPersistences);
        }

    }

}
