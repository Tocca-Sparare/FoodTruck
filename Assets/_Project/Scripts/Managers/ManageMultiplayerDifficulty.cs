using Fusion;
using UnityEngine;

/// <summary>
/// This is a manager in scene, used to manage difficulty of the level
/// </summary>
public class ManageMultiplayerDifficulty : NetworkBehaviour
{
    [SerializeField] FLevelSettings[] levelSettings;

    private void Awake()
    {
        int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;

        //local
        if (NetworkManager.IsOnline == false)
            SetDifficulty(numberOfPlayers);
    }

    public override void Spawned()
    {
        base.Spawned();

        //server
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
        {
            //find difficulty
            int numberOfPlayers = FindObjectsOfType<PlayerController>().Length;
            RPC_OnTableClean(numberOfPlayers);
        }
    }

    void SetDifficulty(int numberOfPlayers)
    {
        //get refs
        FLevelSettings settings = levelSettings[numberOfPlayers];
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        CustomerSpawner customerSpawner = FindObjectOfType<CustomerSpawner>();
        TablesManager tablesManager = FindObjectOfType<TablesManager>();

        //set everything
        if (levelManager)
        {
            levelManager.SetLevelSettings(settings.levelDuration, settings.pointsForStars);
        }
        if (customerSpawner)
        {
            customerSpawner.SetSpawnSettings(settings.minSlowDelay, settings.maxSlowDelay, settings.minSlowCustomers, settings.maxSlowCustomers,
                settings.minFastDelay, settings.maxFastDelay, settings.minFastCustomers, settings.maxFastCustomers,
                settings.durationSlowPhase, settings.totalDurationSpawn);
        }
        if (tablesManager)
        {
            tablesManager.SetTablesSettings(settings.CustomerWaitingTime, settings.CustomerWarningDelays);
        }

        Debug.Log($"SET DIFFICULTY FOR {numberOfPlayers} PLAYERS");
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnTableClean(int numberOfPlayers, RpcInfo info = default)
    {
        SetDifficulty(numberOfPlayers);
    }
}

[System.Serializable]
public struct FLevelSettings
{
    public int CustomerWaitingTime;
    [Range(0, 100)] public int[] CustomerWarningDelays;
    [Space]
    public float minSlowDelay;
    public float maxSlowDelay;
    public int minSlowCustomers;
    public int maxSlowCustomers;
    [Space]
    public float minFastDelay;
    public float maxFastDelay;
    public int minFastCustomers;
    public int maxFastCustomers;
    [Space]
    public float durationSlowPhase;
    public float totalDurationSpawn;
    [Space]
    public int levelDuration;
    public int[] pointsForStars;
}
