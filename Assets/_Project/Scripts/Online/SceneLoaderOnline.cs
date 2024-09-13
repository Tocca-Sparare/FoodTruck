using UnityEngine.SceneManagement;

/// <summary>
/// This is called to load scenes online. Normally is called by your local SceneLoader
/// </summary>
public static class SceneLoaderOnline
{
    /// <summary>
    /// Load scene by name. Only server can do this
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadScene(string scene)
    {
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            NetworkManager.instance.Runner.LoadScene(scene, new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics3D), true);
    }
}
