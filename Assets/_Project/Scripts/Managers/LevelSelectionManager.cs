using Fusion;
using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] LevelPoint[] levels;

    void Start()
    {
        EnableStateMachines();
        EnableUnlockedLevels();
    }

    void EnableStateMachines()
    {
        //set every statemachine to NormalState
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetState(playerSM.NormalState);
    }

    void EnableUnlockedLevels()
    {
        //only local or server
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
        {
            int[] unlockedStars = new int[levels.Length];

            //show stars for every level
            for (int i = 0; i < levels.Length; i++)
            {
                //get how many stars player unlocked for this level
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                unlockedStars[i] = PlayerPrefs.GetInt(sceneName, 0);

                levels[i].SetFullStars(unlockedStars[i]);
            }

            if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            {
                RPC_SetStars(unlockedStars);
            }
        }
    }

    /// <summary>
    /// This is called by UsePlayerInputManagerInEditor, to test rapidly the scene with more players
    /// </summary>
    public void AddPlayerInEditorForTestLocalMultiplayer()
    {
        //just be sure to have every statemachine to NormalState
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
        {
            if (playerSM.CurrentState == null)
                playerSM.SetState(playerSM.NormalState);
        }
    }

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetStars(int[] unlockedStars, RpcInfo info = default)
    {
        //show stars for every level
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SetFullStars(unlockedStars[i]);
        }
    }

    #endregion
}
