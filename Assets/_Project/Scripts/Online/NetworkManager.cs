using Fusion;
using Fusion.Sockets;
using redd096;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is the main script to manage online multiplayer
/// </summary>
public class NetworkManager : Singleton<NetworkManager>, INetworkRunnerCallbacks
{
    [SerializeField] NetworkPrefabRef _playerPrefab;
    [SerializeField] Color[] colorsForPlayers;

    public static bool IsOnline => instance != null && instance.Runner != null;
    public NetworkRunner Runner { get; set; }
    public string LocalPlayerName { get; set; }
    public List<SessionInfo> Sessions { get; set; }
    public Color[] ColorsForPlayers => colorsForPlayers;

    //events
    public System.Action<UserOnline> OnPlayerEnter, OnPlayerRefreshName, OnPlayerExit;
    public System.Action<NetworkInput> OnInputCallback;
    public System.Action OnFailJoinRoom;
    public System.Action<string> OnFailStartGame;

    //private
    private Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    #region custom

    public async void StartGame(GameMode mode, string sessionName, string username, int sceneIndex)
    {
        //save username from inputfield
        LocalPlayerName = username;

        // Create the Fusion runner and let it know that we will be providing user input
        Runner = gameObject.AddComponent<NetworkRunner>();
        Runner.ProvideInput = true;

        //join lobby to get sessions list
        await Runner.JoinSessionLobby(SessionLobby.ClientServer);
        await System.Threading.Tasks.Task.Yield();

        //if trying to join a room, but there isn't room with this name, call fail
        if (mode == GameMode.Client)
        {
            if (Sessions.Find((room) => room.Name == sessionName) == false)
            {
                LeaveGame();
                OnFailJoinRoom?.Invoke();
                return;
            }
        }

        try
        {
            // Create the NetworkSceneInfo from the scene name
            var scene = SceneRef.FromIndex(sceneIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            await Runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = sessionName,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
                PlayerCount = 4
            });
        }
        catch (System.Exception e)
        {
            LeaveGame();
            OnFailStartGame?.Invoke(e.Message);
        }
    }

    public void LeaveGame()
    {
        Runner.Disconnect(Runner.LocalPlayer);

        //destroy instantiated scripts
        foreach (MonoBehaviour script in GetComponents<MonoBehaviour>())
        {
            if (script != this)
                Destroy(script);
        }

        //clear vars
        spawnedCharacters.Clear();
    }

    #endregion

    #region photon

    public void OnConnectedToServer(NetworkRunner runner)
    {
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        OnInputCallback?.Invoke(input);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            spawnedCharacters.Add(player, networkPlayerObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // Find and remove the players avatar
        if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        Debug.Log("Update");
        Sessions = sessionList;
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }

    #endregion
}
