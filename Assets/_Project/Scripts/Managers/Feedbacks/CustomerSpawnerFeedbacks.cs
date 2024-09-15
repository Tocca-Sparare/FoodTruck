using Fusion;
using redd096;
using UnityEngine;

/// <summary>
/// GRaphics and audio for CustomerSpawner
/// </summary>
public class CustomerSpawnerFeedbacks : NetworkBehaviour
{
    [SerializeField] AudioClass alarmAudio;

    CustomerSpawner customerManager;

    private void Awake()
    {
        if (customerManager == null && TryGetComponent(out customerManager) == false)
            Debug.LogError($"Missing customerManager on {name}");

        //add events
        if (customerManager)
        {
            customerManager.OnSlowPhaseEndedCallback += OnSlowPhaseEndedCallback;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (customerManager)
        {
            customerManager.OnSlowPhaseEndedCallback -= OnSlowPhaseEndedCallback;
        }
    }

    private void OnSlowPhaseEndedCallback()
    {
        if (NetworkManager.IsOnline)
        {
            if (NetworkManager.instance.Runner.IsServer)
                RPC_AlarmAudio();
        }
        else
        {
            Debug.Log("play offline");
            SoundManager.instance.Play(alarmAudio);
        }
    }

    #region online


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_AlarmAudio(RpcInfo info = default)
    {
        Debug.Log("play");
        SoundManager.instance.Play(alarmAudio);
    }

    #endregion
}
