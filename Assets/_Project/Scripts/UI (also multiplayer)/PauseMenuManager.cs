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
            var manager = playerController.GetComponent<InputManager>();

            Debug.Log($"{manager.PauseWasPressedThisFrame}");
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

        if (!NetworkManager.IsOnline)
            Time.timeScale = 1;
    }

    public void OnResumeButtonPressed()
    {
        Debug.Log("Resume");
        CloseMenu();
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("Exit");
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
        Debug.Log("Options");
        gameObject.SetActive(false);
        optionsMenu.SetActive(true);
    }
}
