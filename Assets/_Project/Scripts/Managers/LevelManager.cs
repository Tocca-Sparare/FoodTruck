using Fusion;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manager to know when the level finish
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] int initialCountdown = 3;
    [SerializeField] float levelDuration = 90;
    [SerializeField] int[] pointsForStars = new int[] { 100, 200, 300 };

    public enum ELevelState { InitialCountdown, Playing, Finish }

    ELevelState levelState;
    float stateTimer;
    int starsUnlocked;

    public int InitialCountdown => initialCountdown;
    public float LevelDuration => levelDuration;
    public int StarsUnlocked => starsUnlocked;

    public System.Action<ELevelState> OnChangeLevelState;
    public System.Action<float> OnUpdateInitialCountdown;
    public System.Action<float> OnUpdateGameTimer;
    public System.Action OnUpdateStars;

    private void Start()
    {
        OnStartCountdown();
    }

    private void Update()
    {
        //update initial countdown
        if (levelState == ELevelState.InitialCountdown)
        {
            OnUpdateInitialCountdown?.Invoke(stateTimer - Time.time);
        }
        //or game timer
        else if (levelState == ELevelState.Playing)
        {
            OnUpdateGameTimer?.Invoke(stateTimer - Time.time);
        }

        //check when finish state
        if (Time.time > stateTimer)
        {
            if (levelState == ELevelState.InitialCountdown)
                OnStartPlaying();
            else if (levelState == ELevelState.Playing)
                OnFinishLevel();
        }
    }

    void OnStartCountdown()
    {
        //set state and timer
        levelState = ELevelState.InitialCountdown;
        stateTimer = Time.time + initialCountdown;

        OnChangeLevelState(levelState);
    }

    void OnStartPlaying()
    {
        //set state and timer
        levelState = ELevelState.Playing;
        stateTimer = Time.time + levelDuration;

        OnChangeLevelState(levelState);

        //set every player in normal state
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetState(playerSM.NormalState);

        //start spawner
        CustomerSpawner customerSpawner = FindObjectOfType<CustomerSpawner>();
        if (customerSpawner) 
            customerSpawner.Init();
        else if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
            Debug.LogError($"Missing customerSpawner on {name}", gameObject);        
    }

    void OnFinishLevel()
    {
        //set state
        levelState = ELevelState.Finish;

        //disable pause menu - will be showed the End menu
        PauseMenuManager pauseMenu = FindObjectOfType<PauseMenuManager>();
        if (pauseMenu)
        {
            pauseMenu.ForceCloseMenu();
            pauseMenu.enabled = false;
        }

        //stop every player
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
        {
            playerSM.SetNullState();
            Rigidbody rb = playerSM.GetComponent<Rigidbody>();
            if (rb)
                rb.velocity = Vector3.zero;
        }

        //only local or server
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
        {
            //save points
            PointsManager pointsManager = FindObjectOfType<PointsManager>();
            if (pointsManager)
            {
                //calculate stars with this points
                string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                int scoresForStar = pointsForStars.Where(x => pointsManager.CurrentPoints >= x).OrderBy(x => x).LastOrDefault();
                starsUnlocked = System.Array.IndexOf(pointsForStars, scoresForStar) + 1;    //+1 because we want 1 star when we reach target at index 0

                if (PlayerPrefs.GetInt(sceneName, 0) < starsUnlocked)
                    PlayerPrefs.SetInt(sceneName, starsUnlocked);
            }

            //server send RPC to clients to show stars
            if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            {
                RPC_SetStars(starsUnlocked);
            }
        }

        OnChangeLevelState(levelState);
    }

    /// <summary>
    /// This is called by UsePlayerInputManagerInEditor, to test rapidly the scene with more players
    /// </summary>
    public void AddPlayerInEditorForTestLocalMultiplayer()
    {
        //if game is already going, be sure to set state for every statemachine still null
        if (levelState == ELevelState.Playing)
        {
            PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
            foreach (var playerSM in playerStateMachines)
            {
                if (playerSM.CurrentState == null)
                    playerSM.SetState(playerSM.NormalState);
            }
        }
    }

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_SetStars(int stars, RpcInfo info = default)
    {
        starsUnlocked = stars;
        OnUpdateStars?.Invoke();
    }

    #endregion
}
