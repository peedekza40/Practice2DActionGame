using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
    public static PlayerInputControl Instance { get; private set; }

    public Vector2 Move => MoveInput.ReadValue<Vector2>();

    private PlayerInputAction PlayerInput;
    public InputAction MoveInput { get; private set; }
    public InputAction JumpInput { get; private set; }
    public InputAction AttackInput { get; private set; }
    public InputAction BlockInput { get; private set; }
    public InputAction ToggleInventoryInput { get; private set; }

    private void Awake() 
    {
        if(Instance != null)
        {
            Debug.LogError("Found more than one Player InputControl in the scene. Destroy the newest one.");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        PlayerInput = new PlayerInputAction();
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
}
