using UnityEngine;

/// <summary>
/// Manager to know when the level finish
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] int initialCountdown;
    [SerializeField] float levelDuration;

    public enum ELevelState { InitialCountdown, Playing, Finish }

    ELevelState levelState;
    float stateTimer;

    public int InitialCountdown => initialCountdown;
    public float LevelDuration => levelDuration;

    public System.Action<ELevelState> OnChangeLevelState;
    public System.Action<float> OnUpdateInitialCountdown;
    public System.Action<float> OnUpdateGameTimer;

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
        if (customerSpawner == null) Debug.LogError($"Missing customerSpawner on {name}", gameObject);
        customerSpawner.Init();
    }

    void OnFinishLevel()
    {
        //set state
        levelState = ELevelState.Finish;

        OnChangeLevelState(levelState);

        //stop every player
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetNullState();
    }
}
