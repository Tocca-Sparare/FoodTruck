using Fusion;
using redd096;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// This is attached to Player prefab, to show dead model
/// </summary>
public class DeadStatePlayerFeedback : NetworkBehaviour
{
    [SerializeField] GameObject playerModel;
    [SerializeField] GameObject deadGraphics;
    [Space]
    [SerializeField] Material playerMat;
    [SerializeField] TMP_Text countdownText;
    [SerializeField] MeshRenderer deadModel;

    PlayerStateMachine player;

    private void Awake()
    {
        //get ref
        player = GetComponentInParent<PlayerStateMachine>();
        if (player == null) Debug.LogError($"Missing PlayerStateMachine on {name}", gameObject);

        //set death model material
        deadModel.sharedMaterial = playerMat;

        //add events
        if (player)
        {
            player.OnDead += OnDead;
            player.OnUpdateDeadState += OnUpdateDeadState;
            player.OnRespawn += OnRespawn;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (player)
        {
            player.OnDead -= OnDead;
            player.OnUpdateDeadState -= OnUpdateDeadState;
            player.OnRespawn -= OnRespawn;
        }
    }

    void OnDead(float deadDuration)
    {
        //sync online
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
        {
            RPC_OnDead(deadDuration);
        }

        //show dead graphics
        playerModel.SetActive(false);
        deadGraphics.SetActive(true);
    }

    void OnUpdateDeadState(float remainingTime)
    {
        countdownText.text = Mathf.CeilToInt(remainingTime).ToString();
        countdownText.gameObject.SetActive(true);
    }

    void OnRespawn()
    {
        //sync online
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
        {
            RPC_OnRespawn();
        }

        //show normal graphics
        playerModel.SetActive(true);
        deadGraphics.SetActive(false);

        //hide timer
        countdownText.gameObject.SetActive(false);
    }

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    public void RPC_OnDead(float deadDuration, RpcInfo info = default)
    {
        //show dead graphics
        playerModel.SetActive(false);
        deadGraphics.SetActive(true);

        //update timer in coroutine, to avoid send thousand of rpcs
        StartCoroutine(TimerCoroutine(deadDuration));
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    public void RPC_OnRespawn(RpcInfo info = default)
    {
        //show normal graphics
        playerModel.SetActive(true);
        deadGraphics.SetActive(false);

        //hide timer
        countdownText.gameObject.SetActive(false);
    }

    IEnumerator TimerCoroutine(float deadDuration)
    {
        float timer = Time.time + deadDuration;

        //update timer in local
        while (countdownText.gameObject.activeSelf && timer > Time.time)
        {
            countdownText.text = Mathf.CeilToInt(timer - Time.time).ToString();
            countdownText.gameObject.SetActive(true);
            yield return null;
        }
    }

    #endregion
}
