using redd096.Attributes;
using redd096.UIControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This is the main script in MainMenu
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string singlePlayer;
    [SceneInstance][SerializeField] string localMultiplayer;
    [SceneInstance][SerializeField] string onlineMultiplayer;
    //[SerializeField] ModalWindowManager popupOnQuitGame;
    [Space]
    [SerializeField] OptimizeEventSystem eventSystemController;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;

    [Header("Auto set events")]
    [SerializeField] Button singlePlayerButton;
    [SerializeField] Button localMultiplayerButton;
    [SerializeField] Button onlineMultiplayerButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button backFromOptionsButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        if (singlePlayerButton) singlePlayerButton.onClick.AddListener(OnClickSinglePlayer);
        if (localMultiplayerButton) localMultiplayerButton.onClick.AddListener(OnClickMultiplayerLocal);
        if (onlineMultiplayerButton) onlineMultiplayerButton.onClick.AddListener(OnClickMultiplayerOnline);
        if (optionsButton) optionsButton.onClick.AddListener(OnClickOptions);
        if (backFromOptionsButton) backFromOptionsButton.onClick.AddListener(OnClickBackToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(OnClickQuit);
    }

    private void OnDestroy()
    {
        if (singlePlayerButton) singlePlayerButton.onClick.RemoveListener(OnClickSinglePlayer);
        if (localMultiplayerButton) localMultiplayerButton.onClick.RemoveListener(OnClickMultiplayerLocal);
        if (onlineMultiplayerButton) onlineMultiplayerButton.onClick.RemoveListener(OnClickMultiplayerOnline);
        if (optionsButton) optionsButton.onClick.RemoveListener(OnClickOptions);
        if (backFromOptionsButton) backFromOptionsButton.onClick.RemoveListener(OnClickBackToMainMenu);
        if (quitButton) quitButton.onClick.RemoveListener(OnClickQuit);
    }

    public void OnClickSinglePlayer()
    {
        SceneManager.LoadScene(singlePlayer);
    }

    public void OnClickMultiplayerLocal()
    {
        SceneManager.LoadScene(localMultiplayer);
    }

    public void OnClickMultiplayerOnline()
    {
        SceneManager.LoadScene(onlineMultiplayer);
    }

    public void OnClickOptions()
    {
        eventSystemController.ChangeMenu(optionsMenu);
    }

    public void OnClickBackToMainMenu()
    {
        eventSystemController.ChangeMenu(mainMenu);
    }

    public void OnClickQuit()
    {
        mainMenu.SetActive(false);
        //popupOnQuitGame.Open();
        OnClickConfirmOnPopup();
    }

    public void OnClickCancelOnPopup()
    {
        mainMenu.SetActive(true);
    }

    public void OnClickConfirmOnPopup()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
