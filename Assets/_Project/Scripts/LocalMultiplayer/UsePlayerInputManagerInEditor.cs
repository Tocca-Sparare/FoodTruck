using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// In normal game, we have PlayerInputManager in the Lobby, without this script.
/// But in gameplay scenes, we need a PlayerInputManager with this script attached, just to test rapidly.
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class UsePlayerInputManagerInEditor : MonoBehaviour
{
    PlayerInputManager playerInputManager;
    LevelManager levelManager;

    bool isCorrectInstance;

    private void Awake()
    {
        //get refs
        playerInputManager = GetComponent<PlayerInputManager>();
        levelManager = FindObjectOfType<LevelManager>();

        //be sure to not spawn immediatly (because if we press Submit in gameplay scene, PlayerInputManager spawn a player also if it's not the correct instance)
        playerInputManager.DisableJoining();
    }

    //makes checks in Start to be sure instance is setted by PlayerInputManager
    private void Start()
    {
        if (playerInputManager == null)
        {
            Debug.LogError($"Missing PlayerInputManager on {name}", gameObject);
            return;
        }

        //if this isn't the correct input manager, destroy
        isCorrectInstance = IsCorrectPlayerInputManager();
        if (isCorrectInstance == false)
        {
            playerInputManager = null;  //remove to avoid unregister from events in OnDestroy event
            Destroy(gameObject);
            return;
        }

        //if this is the correct input manager, re-enable joining
        playerInputManager.EnableJoining();

        //register to events
        if (playerInputManager)
        {
            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }
    }

    private void OnDestroy()
    {
        //unregister from events
        if (playerInputManager)
        {
            playerInputManager.onPlayerJoined -= OnPlayerJoined;
            playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }
    }

    private void OnPlayerJoined(PlayerInput input)
    {
        //add player controller, and tell to LevelManager
        if (isCorrectInstance)
            levelManager.AddPlayerInEditorForTestLocalMultiplayer(input);
    }

    private void OnPlayerLeft(PlayerInput input)
    {
        //remove player controller
        if (isCorrectInstance)
            Destroy(input.gameObject);
    }

    /// <summary>
    /// The PlayerInputManager in the lobby doesn't have this script.
    /// So, if this is the PlayerInputManager instance and there aren't players in scene, then we started the game from the gameplay scene (so we are in editor)
    /// </summary>
    /// <returns></returns>
    bool IsCorrectPlayerInputManager()
    {
        //return true if this is the correct instance
        if (PlayerInputManager.instance != null && PlayerInputManager.instance == playerInputManager)
        {
            //and there aren't players in scene
            if (FindObjectsOfType<PlayerController>().Length <= 0)
                return true;
        }

        return false;
    }
}
