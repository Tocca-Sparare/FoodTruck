using redd096.Attributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the main script in Lobby Online
/// </summary>
public class LobbyOnlineUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string backButtonScene;
    [SceneInstance][SerializeField] string selectLevelScene;
    [SerializeField] GameObject[] playerImages;
    [Space]
    [SerializeField] GameObject joinedPlayerPrefab;
    [SerializeField] Transform joinedPlayersContainer;
    [SerializeField] TMP_Text roomCodeText;
    [SerializeField] Button[] buttonsToDisableIfNotServer;

    [Header("Auto register to events")]
    [SerializeField] Button startGameButton;
    [SerializeField] Button backButton;

    private Dictionary<string, GameObject> joinedPlayers = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (NetworkManager.instance == null)
        {
            Debug.LogError($"Missing network manager on {name}", gameObject);
            return;
        }

        //update room code text
        roomCodeText.text = "Room Code: " + NetworkManager.instance.Runner.SessionInfo.Name;

        //active buttons only for server
        foreach (var b in buttonsToDisableIfNotServer)
            b.interactable = NetworkManager.instance.Runner.IsServer;

        //register to events
        if (NetworkManager.instance)
        {
            NetworkManager.instance.OnPlayerEnter += AddPlayer;
            NetworkManager.instance.OnPlayerRefreshName += RefreshPlayer;
            NetworkManager.instance.OnPlayerRefreshIndex += RefreshPlayer;
            NetworkManager.instance.OnPlayerExit += RemovePlayer;
        }

        //set default players
        foreach (UserOnline u in FindObjectsOfType<UserOnline>())
            AddPlayer(u);

        //Auto register to events
        if (startGameButton) startGameButton.onClick.AddListener(OnClickStartGame);
        if (backButton) backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDestroy()
    {
        //unregister from events
        if (NetworkManager.instance)
        {
            NetworkManager.instance.OnPlayerEnter -= AddPlayer;
            NetworkManager.instance.OnPlayerRefreshName -= RefreshPlayer;
            NetworkManager.instance.OnPlayerRefreshIndex -= RefreshPlayer;
            NetworkManager.instance.OnPlayerExit -= RemovePlayer;
        }

        //unregister events
        if (startGameButton) startGameButton.onClick.RemoveListener(OnClickStartGame);
        if (backButton) backButton.onClick.RemoveListener(OnClickBack);
    }

    public void OnClickStartGame()
    {
        //only server can call this button
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            SceneLoader.LoadScene(selectLevelScene);
    }

    public void OnClickBack()
    {
        //leave online
        NetworkManager.instance.LeaveGame(() => SceneLoader.LoadScene(backButtonScene));
    }

    #region events

    private void AddPlayer(UserOnline user)
    {
        string onlineID = user.Object.Id.ToString();

        //instantiate object in UI
        if (joinedPlayers.ContainsKey(onlineID) == false)
        {
            playerImages[joinedPlayers.Count].SetActive(true);
            GameObject go = Instantiate(joinedPlayerPrefab, joinedPlayersContainer);
            go.GetComponentInChildren<TMP_Text>(true).text = user.PlayerName;
            go.GetComponentInChildren<Image>(true).color = NetworkManager.instance.ColorsForPlayers[user.PlayerIndex];
            go.SetActive(true);

            //and add to dictionary
            joinedPlayers.Add(onlineID, go);
        }
    }

    private void RemovePlayer(UserOnline user)
    {
        string onlineID = user.Object.Id.ToString();

        //remove object from UI and dictionary
        if (joinedPlayers.ContainsKey(onlineID))
        {
            GameObject go = joinedPlayers[onlineID];
            joinedPlayers.Remove(onlineID);
            Destroy(go);
            playerImages[joinedPlayers.Count].SetActive(false);
        }
    }

    private void RefreshPlayer(UserOnline user)
    {
        string onlineID = user.Object.Id.ToString();

        if (joinedPlayers.ContainsKey(onlineID))
        {
            //update player username in UI
            GameObject go = joinedPlayers[onlineID];
            go.GetComponentInChildren<TMP_Text>(true).text = user.PlayerName;
            go.GetComponentInChildren<Image>(true).color = NetworkManager.instance.ColorsForPlayers[user.PlayerIndex];
        }
    }

    #endregion
}
