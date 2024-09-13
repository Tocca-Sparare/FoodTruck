using Fusion;
using redd096.Attributes;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the main script in Create Room Online
/// </summary>
public class OnlineCreateFindRoomUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string backButtonScene = "MainMenu";
    [SceneInstance][SerializeField] int lobbyOnlineScene;
    [Space]
    [SerializeField] TMP_InputField usernameInputField;
    [SerializeField] TMP_InputField roomCodeInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] Button[] buttonsToDisableWhenConnecting;

    [Header("Auto register to events")]
    [SerializeField] Button createRoomButton;
    [SerializeField] Button joinRoomButton;
    [SerializeField] Button backButton;

    const string errorJoin = "No Room Found";
    const string errorUsernameEmpty = "Not valid username";
    const string errorRoomCodeEmpty = "Not valid room code";

    string[] randomNames = new string[50] {"Tharion", "Eryndor", "Arintha", "Kaelin", "Eldrid",
                      "Draven", "Ryker", "Torin", "Lirien", "Galadrielle",
                      "Valoria", "Zephyr", "Sable", "Celestia", "Nyx",
                      "Calantha", "Sorin", "Isadora", "Auriel", "Thalia",
                      "Gwyneth", "Lorien", "Alaric", "Rowan", "Eira",
                      "Darian", "Seraphina", "Evander", "Lysandra", "Halcyon",
                      "Thorne", "Rhiannon", "Kairos", "Sabriel", "Cygnus",
                      "Caius", "Elara", "Orion", "Lyra", "Serenity",
                      "Daedalus", "Lyris", "Vesper", "Aeloria", "Cassius",
                      "Xanthe", "Thetis", "Zarek", "Niamh", "Elwyn" };

    private void Awake()
    {
        if (NetworkManager.instance == null)
        {
            Debug.LogError($"Missing network manager on {name}", gameObject);
        }

        //register to events
        if (NetworkManager.instance)
        {
            NetworkManager.instance.OnFailJoinRoom += OnFailJoinRoom;
            NetworkManager.instance.OnFailStartGame += OnFailStartGame;
        }

        //set random username
        usernameInputField.text = randomNames[Random.Range(0, randomNames.Length)];

        //Auto register to events
        if (createRoomButton) createRoomButton.onClick.AddListener(OnClickCreateRoom);
        if (joinRoomButton) joinRoomButton.onClick.AddListener(OnClickJoinRoom);
        if (backButton) backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDestroy()
    {
        //unregister from events
        if (NetworkManager.instance)
        {
            NetworkManager.instance.OnFailJoinRoom -= OnFailJoinRoom;
            NetworkManager.instance.OnFailStartGame -= OnFailStartGame;
        }

        //unregister events
        if (createRoomButton) createRoomButton.onClick.RemoveListener(OnClickCreateRoom);
        if (joinRoomButton) joinRoomButton.onClick.RemoveListener(OnClickJoinRoom);
        if (backButton) backButton.onClick.RemoveListener(OnClickBack);
    }

    public void OnClickCreateRoom()
    {
        //error if no username
        if (string.IsNullOrEmpty(usernameInputField.text))
        {
            errorText.text = errorUsernameEmpty;
            return;
        }

        errorText.text = "";

        //disable buttons, wait the game to start
        foreach (var b in buttonsToDisableWhenConnecting)
            b.interactable = false;

        //create room
        string roomCode = GenerateRandomRoomCode();
        NetworkManager.instance.StartGame(GameMode.Host, roomCode, usernameInputField.text, lobbyOnlineScene);
    }

    public void OnClickJoinRoom()
    {
        //error if no username
        if (string.IsNullOrEmpty(usernameInputField.text))
        {
            errorText.text = errorUsernameEmpty;
            return;
        }
        //error if no room code
        else if (string.IsNullOrEmpty(usernameInputField.text))
        {
            errorText.text = errorRoomCodeEmpty;
            return;
        }

        errorText.text = "";

        //disable buttons, wait the game to start
        foreach (var b in buttonsToDisableWhenConnecting)
            b.interactable = false;

        //try join room
        NetworkManager.instance.StartGame(GameMode.Client, roomCodeInputField.text.ToUpper(), usernameInputField.text, lobbyOnlineScene);
    }

    public void OnClickBack()
    {
        SceneLoader.LoadScene(backButtonScene);
    }

    #region events

    private void OnFailJoinRoom()
    {
        //show error message
        errorText.text = errorJoin;

        //re-enable buttons
        foreach (var b in buttonsToDisableWhenConnecting)
            b.interactable = true;
    }

    private void OnFailStartGame(string error)
    {
        //show error message
        errorText.text = error;

        //re-enable buttons
        foreach (var b in buttonsToDisableWhenConnecting)
            b.interactable = true;
    }

    #endregion

    #region generate random room code

    private string GenerateRandomRoomCode()
    {
        string result;

        while (true)
        {
            //generate random string
            result = RandomString(4);

            //check there aren't other sessions with same name
            if (NetworkManager.instance.Sessions == null)
                break;
            if (NetworkManager.instance.Sessions.Find((room) => room.Name == result) == false)
                break;
        }

        return result;
    }

    private string RandomString(int length)
    {
        System.Random rand = new System.Random();
        const string possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(possibleChars, length)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
    }

    #endregion
}
