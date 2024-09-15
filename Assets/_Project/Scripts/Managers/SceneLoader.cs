using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Use this to load scene in local or online
/// </summary>
public static class SceneLoader
{
    /// <summary>
    /// Load scene by name
    /// </summary>
    public static void LoadScene(string scene)
    {
        Time.timeScale = 1;

        if (TransitionManager.instance)
        {
            TransitionManager.instance.LoadScene(scene);
        }
        else
        {
            //change scene online or normally
            if (NetworkManager.IsOnline)
                SceneLoaderOnline.LoadScene(scene);
            else
                SceneManager.LoadScene(scene);
        }
    }

    /// <summary>
    /// Exit game (works also in editor)
    /// </summary>
    public static void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
