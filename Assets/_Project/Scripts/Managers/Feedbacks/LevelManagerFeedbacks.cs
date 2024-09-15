using Fusion;
using redd096;
using UnityEngine;

/// <summary>
/// GRaphics and audio for LevelManager
/// </summary>
public class LevelManagerFeedbacks : NetworkBehaviour
{
    [SerializeField] float remainingTimeForAlarm = 5;
    [SerializeField] AudioClass almostFinishLevelAudio;

    LevelManager levelManager;
    bool playedAlarm;

    private void Awake()
    {
        if (levelManager == null && TryGetComponent(out levelManager) == false)
            Debug.LogError($"Missing levelManager on {name}");

        //add events
        if (levelManager)
        {
            levelManager.OnUpdateGameTimer += OnUpdateGameTimer;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (levelManager)
        {
            levelManager.OnUpdateGameTimer -= OnUpdateGameTimer;
        }
    }

    private void OnUpdateGameTimer(float remainingTime)
    {
        //when almost finished level
        if (remainingTime < remainingTimeForAlarm)
        {
            //do only once
            if (playedAlarm)
                return;
            playedAlarm = true;

            if (NetworkManager.IsOnline)
                RPC_OnUpdateAlmostFinishLevel();
            else
                SoundManager.instance.Play(almostFinishLevelAudio);
        }
    }

    #region online


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnUpdateAlmostFinishLevel(RpcInfo info = default)
    {
        SoundManager.instance.Play(almostFinishLevelAudio);
    }

    #endregion
}
