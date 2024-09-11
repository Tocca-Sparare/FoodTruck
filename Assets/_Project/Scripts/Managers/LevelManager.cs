using UnityEngine;

/// <summary>
/// Manager to know when the level finish
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] int initialCountdown;
    [SerializeField] float levelDuration;

    CustomerSpawner customerSpawner;
    UIManager uiManager;
    float stateTimer;

    public enum ELevelState { InitialCountdown, Playing, Finish }
    private ELevelState levelState;

    private void Awake()
    {
        //get refs
        customerSpawner = FindObjectOfType<CustomerSpawner>();
        if (customerSpawner == null) Debug.LogError($"Missing customerSpawner on {name}", gameObject);
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) Debug.LogError($"Missing uiManager on {name}", gameObject);

        OnStartCountdown();
    }

    private void Update()
    {
        //update initial countdown
        if (levelState == ELevelState.InitialCountdown)
        {
            uiManager.UpdateInitialCountdown(Mathf.CeilToInt(stateTimer - Time.time));
        }
        //or game timer
        else if (levelState == ELevelState.Playing)
        {
            uiManager.UpdateGameTimer(Mathf.CeilToInt(stateTimer - Time.time));
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

        //show timers with correct value
        uiManager.UpdateInitialCountdown(Mathf.CeilToInt(initialCountdown));
        uiManager.UpdateGameTimer(Mathf.CeilToInt(levelDuration));
    }

    void OnStartPlaying()
    {
        //set state and timer
        levelState = ELevelState.Playing;
        stateTimer = Time.time + levelDuration;

        //set every player in normal state
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetState(playerSM.NormalState);

        //hide countdown
        uiManager.StopInitialCountdown();

        //start spawner
        customerSpawner.Init();
    }

    void OnFinishLevel()
    {
        //set state
        levelState = ELevelState.Finish;

        //stop every player
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetNullState();

        //show end menu
        uiManager.ShowEndMenu();
    }
}
