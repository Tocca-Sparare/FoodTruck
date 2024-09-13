using UnityEngine;

/// <summary>
/// Both in local and online multiplayer, this is instantiated for every player (it's attached to PlayerController). 
/// Use this to access to every player's information (name, index, etc...)
/// </summary>
[RequireComponent(typeof(InputManager))]
public class User : MonoBehaviour
{
    InputManager inputManager;
    public int PlayerIndex { get { if (inputManager == null) inputManager = GetComponent<InputManager>(); return inputManager.PlayerInput.playerIndex; } }

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
    }
}
