using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Create a variable for every input and attach the component to your PlayerController. 
/// Then your player will use probably a statemachine to know when read these inputs
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public string MouseSchemeName = "MouseAndKeyboard";

    //inputs
    public Vector2 Move { get; set; }
    public bool InteractWasPressedThisFrame {  get; set; }
    public bool ShootWasPressedThisFrame { get; set; }
    public bool PauseWasPressedThisFrame { get; set; }
    public Vector2 Aim { get; set; }
    public bool IsUsingMouse { get; set; }

    #region playerInput

    PlayerInput _playerInput;
    public PlayerInput PlayerInput { get { if (_playerInput == null) _playerInput = GetComponent<PlayerInput>(); return _playerInput; } }

    /// <summary>
    /// Shortcut to call FindAction on PlayerInput
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public InputAction FindAction(string action)
    {
        return PlayerInput.actions.FindAction(action);
    }

    #endregion

    private void Update()
    {
        //read inputs
        Move = FindAction("Move").ReadValue<Vector2>();
        InteractWasPressedThisFrame = FindAction("Interact").WasPressedThisFrame();
        ShootWasPressedThisFrame = FindAction("Shoot").WasPressedThisFrame();
        PauseWasPressedThisFrame = FindAction("Pause").WasPressedThisFrame();
        Aim = FindAction("Aim").ReadValue<Vector2>();
        IsUsingMouse = PlayerInput.currentControlScheme == MouseSchemeName;
    }
}
