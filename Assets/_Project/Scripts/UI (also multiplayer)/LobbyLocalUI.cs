using redd096.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// This is the main script in Lobby Local
/// </summary>
public class LobbyLocalUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string backButtonScene;
    [SceneInstance][SerializeField] string selectLevelScene;
    [Space]
    [SerializeField] GameObject joinedPlayerPrefab;
    [SerializeField] Transform joinedPlayersContainer;
    [SerializeField] GameObject objectWhenNoPlayersInScene;
    [SerializeField] Button[] buttonsToDisableWhenNoPlayersInScene;

    [Header("Auto register to events")]
    [SerializeField] Button startGameButton;
    [SerializeField] Button backButton;

    private Dictionary<PlayerInput, GameObject> joinedPlayers = new Dictionary<PlayerInput, GameObject>();

    private void Awake()
    {
        if (PlayerInputManager.instance == null)
        {
            Debug.LogError($"Missing PlayerInputManager on {name}", gameObject);
            return;
        }

        //register to events
        if (PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft += OnPlayerLeft;
        }

        //disable select level button by default
        UpdateButtonsStatus();

        //set default players
        foreach (User u in FindObjectsOfType<User>())
            OnPlayerJoined(u.GetComponent<PlayerInput>());

        //Auto register to events
        if (startGameButton) startGameButton.onClick.AddListener(OnClickStartGame);
        if (backButton) backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDestroy()
    {
        //unregister from events
        if (PlayerInputManager.instance)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
            PlayerInputManager.instance.onPlayerLeft -= OnPlayerLeft;
        }

        //unregister events
        if (startGameButton) startGameButton.onClick.RemoveListener(OnClickStartGame);
        if (backButton) backButton.onClick.RemoveListener(OnClickBack);
    }

    public void OnClickStartGame()
    {
        //change scene
        SceneLoader.LoadScene(selectLevelScene);
    }

    public void OnClickBack()
    {
        //leave local multiplayer
        LeaveLocalMultiplayer();
        SceneLoader.LoadScene(backButtonScene);
    }

    #region events

    private void OnPlayerJoined(PlayerInput input)
    {
        //instantiate object in UI
        if (joinedPlayers.ContainsKey(input) == false)
        {
            User user = input.GetComponent<User>();
            GameObject go = Instantiate(joinedPlayerPrefab, joinedPlayersContainer);
            go.GetComponentInChildren<TMP_Text>(true).text = "Player " + (user.PlayerIndex + 1);    //+1 to show "Player 1" instead of "Player 0"
            go.GetComponentInChildren<Image>(true).color = NetworkManager.instance.ColorsForPlayers[user.PlayerIndex];
            go.SetActive(true);

            //and add to dictionary
            joinedPlayers.Add(input, go);

            //update buttons - with a little delay to avoid start game if player pressed Submit button to join
            if (objectWhenNoPlayersInScene) objectWhenNoPlayersInScene.SetActive(false);        //but disable immediatly the "object when no players in scene"
            foreach (var b in buttonsToDisableWhenNoPlayersInScene) b.interactable = false;
            Invoke(nameof(UpdateButtonsStatus), 0.1f);
        }
    }

    private void OnPlayerLeft(PlayerInput input)
    {
        //remove object from UI and dictionary
        if (joinedPlayers.ContainsKey(input))
        {
            GameObject go = joinedPlayers[input];
            joinedPlayers.Remove(input);
            Destroy(go);

            //update buttons
            UpdateButtonsStatus();
        }
    }

    #endregion

    #region private API

    void UpdateButtonsStatus()
    {
        bool thereArePlayersInScene = joinedPlayers.Count > 0;

        //object to activate only when there are NO players in scene
        if (objectWhenNoPlayersInScene) 
            objectWhenNoPlayersInScene.SetActive(thereArePlayersInScene == false);

        //enable buttons only if there are players in scene
        foreach (var b in buttonsToDisableWhenNoPlayersInScene)
            b.interactable = thereArePlayersInScene;
    }

    void LeaveLocalMultiplayer()
    {
        //remove connected players
        PlayerController[] playersInScene = FindObjectsOfType<PlayerController>();
        for (int i = playersInScene.Length - 1; i >= 0; i--)
            Destroy(playersInScene[i].gameObject);

        //destroy player input manager
        Destroy(PlayerInputManager.instance.gameObject);
    }

    #endregion
}
