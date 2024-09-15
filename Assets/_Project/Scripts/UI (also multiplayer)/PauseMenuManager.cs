using redd096.Attributes;
using redd096.UIControl;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SceneInstance][SerializeField] string onLocalExitScene;
    [SceneInstance][SerializeField] string onServerExitScene;
    [SceneInstance][SerializeField] string onClientExitScene;

    AutoPossess autoPosses;
    OptimizeEventSystem optimizeEventSystem;
    bool isPaused;

    private void Awake()
    {
        autoPosses = FindObjectOfType<AutoPossess>();
        optimizeEventSystem = FindObjectOfType<OptimizeEventSystem>();
    }

    private void Update()
    {
        //check every player controller
        foreach (var playerController in autoPosses.playerControllers)
        {
            if (playerController == null)
                continue;

            var manager = playerController.GetComponent<InputManager>();
            if (manager)
            {
                //check if press Resume or Pause
                if (isPaused && manager.ResumeWasPressedThisFrame)
                    CloseMenu();
                else if (isPaused == false && manager.PauseWasPressedThisFrame)
                    OpenMenu();
            }
        }
    }

    private void OpenMenu()
    {
        isPaused = true;

        //in local set timescale 0
        if (!NetworkManager.IsOnline)
            Time.timeScale = 0;

        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    void CloseMenu()
    {
        //if in options menu, just move back to pause menu
        if (optionsMenu.activeInHierarchy)
        {
            OnBackButtonPressed();
            return;
        }

        isPaused = false;

        //in local reset timescale
        if (!NetworkManager.IsOnline)
            Time.timeScale = 1;

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    /// <summary>
    /// Close both pause and options menu immediatly
    /// </summary>
    public void ForceCloseMenu()
    {
        isPaused = false;

        //in local reset timescale
        if (!NetworkManager.IsOnline)
            Time.timeScale = 1;

        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    #region ui button events

    public void OnResumeButtonPressed()
    {
        CloseMenu();
    }

    public void OnExitButtonPressed()
    {
        if (!NetworkManager.IsOnline)
            Time.timeScale = 1;

        if (NetworkManager.IsOnline)
        {
            //if server, back to lobby online or select level - if client, leave game and back to create room
            if (NetworkManager.instance.Runner.IsServer)
                SceneLoader.LoadScene(onServerExitScene);
            else
                NetworkManager.instance.LeaveGame(() => SceneLoader.LoadScene(onClientExitScene));
        }
        else
        {
            //in local just move back to lobby local or select level
            SceneLoader.LoadScene(onLocalExitScene);
        }
    }

    public void OnOptionsButtonPressed()
    {
        optimizeEventSystem.ChangeMenu(optionsMenu);
    }

    public void OnBackButtonPressed()
    {
        optimizeEventSystem.ChangeMenu(pauseMenu);
    }

    #endregion
}
