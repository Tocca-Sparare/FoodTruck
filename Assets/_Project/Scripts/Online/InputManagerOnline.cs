using Fusion;
using UnityEngine;

/// <summary>
/// This is attached to PlayerController with InputManager. This is used to get inputs online
/// </summary>
[RequireComponent(typeof(InputManager))]
public class InputManagerOnline : NetworkBehaviour
{
    InputManager _inputManager;
    InputManager inputManager { get { if (_inputManager == null) _inputManager = GetComponent<InputManager>(); return _inputManager; } }

    NetworkInputData myInputs = new NetworkInputData();

    private void Awake()
    {
        //disable input manager online, now we use this to get inputs
        if (NetworkManager.IsOnline)
        {
            inputManager.enabled = false;
        }
        //viceversa in local, disable this and keep local input manager
        else
        {
            enabled = false;
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        //register to send input online
        if (Object.HasInputAuthority)
            NetworkManager.instance.OnInputCallback += OnInput;
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);

        //unregister events
        if (Object.HasInputAuthority)
            NetworkManager.instance.OnInputCallback -= OnInput;
    }

    private void OnInput(NetworkInput input)
    {
        //send inputs to server
        input.Set(myInputs);

        // Reset the input struct to start with a clean slate
        // when polling for the next tick
        myInputs = default;
    }

    private void Update()
    {
        //set inputs in local update (they'll be sent to server with OnInput callback)
        if (NetworkManager.IsOnline)
        {
            myInputs.Move = inputManager.FindAction("Move").ReadValue<Vector2>();
            if (inputManager.FindAction("Interact").WasPressedThisFrame()) myInputs.Buttons.Set(MyButtons.Interact, true);
            //myInputs.InteractIsPressed = inputManager.FindAction("Interact").IsPressed();
            if (inputManager.FindAction("Shoot").WasPressedThisFrame()) myInputs.Buttons.Set(MyButtons.Shoot, true);
            myInputs.Aim = inputManager.FindAction("Aim").ReadValue<Vector2>();
            myInputs.IsUsingMouse = inputManager.PlayerInput.currentControlScheme == inputManager.MouseSchemeName;
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        //receive inputs
        if (GetInput(out NetworkInputData input))
        {
            inputManager.Move = input.Move;
            inputManager.InteractWasPressedThisFrame = input.Buttons.IsSet(MyButtons.Interact);
            inputManager.ShootWasPressedThisFrame = input.Buttons.IsSet(MyButtons.Shoot);
            inputManager.Aim = input.Aim;
            inputManager.IsUsingMouse = input.IsUsingMouse;
        }
    }
}

public enum MyButtons
{
    Interact = 0,
    Shoot = 1,
}

public struct NetworkInputData : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector2 Move;
    public Vector2 Aim;
    public bool IsUsingMouse;
}