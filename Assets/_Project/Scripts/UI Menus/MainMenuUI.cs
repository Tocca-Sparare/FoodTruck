using redd096.Attributes;
using redd096.UIControl;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the main script in MainMenu
/// </summary>
public class MainMenuUI : MonoBehaviour
{
    [SceneInstance][SerializeField] string local;
    [SceneInstance][SerializeField] string online;
    //[SerializeField] ModalWindowManager popupOnQuitGame;
    [Space]
    [SerializeField] OptimizeEventSystem eventSystemController;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;

    [Header("Auto register to events")]
    [SerializeField] Button localButton;
    [SerializeField] Button onlineButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button backFromOptionsButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        //Auto register to events
        if (localButton) localButton.onClick.AddListener(OnClickSinglePlayer);
        if (onlineButton) onlineButton.onClick.AddListener(OnClickMultiplayerOnline);
        if (optionsButton) optionsButton.onClick.AddListener(OnClickOptions);
        if (backFromOptionsButton) backFromOptionsButton.onClick.AddListener(OnClickBackToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(OnClickQuit);
    }

    private void OnDestroy()
    {
        //unregister events
        if (localButton) localButton.onClick.RemoveListener(OnClickSinglePlayer);
        if (onlineButton) onlineButton.onClick.RemoveListener(OnClickMultiplayerOnline);
        if (optionsButton) optionsButton.onClick.RemoveListener(OnClickOptions);
        if (backFromOptionsButton) backFromOptionsButton.onClick.RemoveListener(OnClickBackToMainMenu);
        if (quitButton) quitButton.onClick.RemoveListener(OnClickQuit);
    }

    public void OnClickSinglePlayer()
    {
        SceneLoader.LoadScene(local);
    }

    public void OnClickMultiplayerOnline()
    {
        SceneLoader.LoadScene(online);
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
        SceneLoader.ExitGame();
    }
}
