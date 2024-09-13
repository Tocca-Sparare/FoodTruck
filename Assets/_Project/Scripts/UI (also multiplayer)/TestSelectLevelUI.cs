using redd096.Attributes;
using UnityEngine;
using UnityEngine.UI;

/// <summary>/// 
/// This is the main script in Test Select Level
/// </summary>
public class TestSelectLevelUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string backSceneServer;
    [SceneInstance][SerializeField] string backSceneClient;
    [SceneInstance][SerializeField] string backSceneOffline;
    [SceneInstance][SerializeField] string[] levelsScenes;
    [Space]
    [SerializeField] Button[] buttonsToDisableIfNotServer;

    [Header("Auto register to events")]
    [SerializeField] Button[] levelsButtons;
    [SerializeField] Button backButton;

    private void Awake()
    {
        //active buttons only offline or for server
        foreach (var b in buttonsToDisableIfNotServer)
            b.interactable = NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer;

        //Auto register to events
        for (int i = 0; i < levelsButtons.Length; i++)
        {
            int index = i;
            if (levelsButtons[i]) levelsButtons[i].onClick.AddListener(() => OnClickLevel(index));
        }
        if (backButton) backButton.onClick.AddListener(OnClickBack);
    }

    private void OnDestroy()
    {
        //unregister events
        if (backButton) backButton.onClick.RemoveListener(OnClickBack);
    }

    public void OnClickLevel(int index)
    {
        //only offline or server can call this button
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
            SceneLoader.LoadScene(levelsScenes[index]);
    }

    public void OnClickBack()
    {
        if (NetworkManager.IsOnline)
        {
            //server move back to lobby online
            if (NetworkManager.instance.Runner.IsServer)
            {
                SceneLoader.LoadScene(backSceneServer);
            }
            //client exit from lobby and move back to Create or Join room
            else
            {
                NetworkManager.instance.LeaveGame();
                SceneLoader.LoadScene(backSceneClient);
            }
        }
        else
        {
            //offline, move back to lobby local
            SceneLoader.LoadScene(backSceneOffline);
        }
    }
}
