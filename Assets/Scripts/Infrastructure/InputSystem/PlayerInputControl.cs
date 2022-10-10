using System.Collections.Generic;
using System.Linq;
using Core.Constants;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.InputSystem
{
    public class PlayerInputControl : MonoBehaviour
    {
        public Vector2 Move => MoveInput.ReadValue<Vector2>();

        public List<MouseEvent> UIMouseEvents;
        public List<IUIPersistence> UIPersistences;

        private PlayerInputAction PlayerInput;
        public InputAction MoveInput { get; private set; }
        public InputAction JumpInput { get; private set; }
        public InputAction AttackInput { get; private set; }
        public InputAction BlockInput { get; private set; }
        public InputAction ToggleInventoryInput { get; private set; }

        private void Awake() 
        {
            PlayerInput = new PlayerInputAction();
            UIPersistences = FindAllUIPersistenceObjects();
        }

        private void OnEnable() 
        {
            MoveInput = PlayerInput.Player.Move;
            MoveInput.Enable();

            JumpInput = PlayerInput.Player.Jump;
            JumpInput.Enable();

            AttackInput = PlayerInput.Player.Attack;
            AttackInput.Enable();

            BlockInput = PlayerInput.Player.Block;
            BlockInput.Enable();

            ToggleInventoryInput = PlayerInput.Player.ToggleInventory;
            ToggleInventoryInput.Enable();
        }

        private void OnDisable() 
        {
            MoveInput.Disable();
            JumpInput.Disable();
            AttackInput.Disable();
            BlockInput.Disable();
            ToggleInventoryInput.Disable();
        }

        private void Update() 
        {
            //disabled attack
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

            //disabled movement & inventory
            if(UIPersistences.Any(x => x.Number == UINumber.PauseMenu && x.IsOpen))
            {
                MoveInput.Disable();
                ToggleInventoryInput.Disable();
            }
            else
            {
                MoveInput.Enable();
                ToggleInventoryInput.Enable();
            }


            //set cursur visible
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
            IEnumerable<IUIPersistence> uiPersistences = FindObjectsOfType<MonoBehaviour>().OfType<IUIPersistence>();

            return new List<IUIPersistence>(uiPersistences);
        }

    }

}
