using redd096.Attributes;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SceneInstance][SerializeField] string onExitScene;
    [SceneInstance][SerializeField] string onClientExitScene;

    AutoPossess autoPosses;

    private void Awake()
    {
        autoPosses = FindObjectOfType<AutoPossess>();
    }

    private void Update()
    {
        foreach (var playerController in autoPosses.playerControllers)
        {
            if (playerController == null)
                continue;

            var manager = playerController.GetComponent<InputManager>();

            if (manager.PauseWasPressedThisFrame)
            {
                if (pauseMenu.activeSelf)
                    CloseMenu();
                else
                    OpenMenu(manager);
            }
        }
    }

    private void OpenMenu(InputManager manager)
    {
        if (!NetworkManager.IsOnline)
            Time.timeScale = 0;

        pauseMenu.SetActive(true);
    }

    void CloseMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);

        if (!NetworkManager.IsOnline)
            Time.timeScale = 1;
    }

    public void OnResumeButtonPressed()
    {
        CloseMenu();
    }

    public void OnExitButtonPressed()
    {
        CloseMenu();
        if (NetworkManager.IsOnline && !NetworkManager.instance.Runner.IsServer)
        {
            NetworkManager.instance.LeaveGame();
            SceneLoader.LoadScene(onClientExitScene);
        }
        else
        {
            SceneLoader.LoadScene(onExitScene);
        }
    }

    public void OnOptionsButtonPressed()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void OnBackButtonPressed()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
