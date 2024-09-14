using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerDeadState : State
{
    [SerializeField] int deadDuration;
    [SerializeField] Material playerMat;
    [SerializeField] GameObject playerModel;
    [SerializeField] MeshRenderer deadModel;
    [SerializeField] GameObject deadGraphics;
    [SerializeField] TMP_Text countdownText;

    PlayerStateMachine player;
    float timer;
    Rigidbody rb;
    Collider[] colliders;

    Vector3 moveDirection;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerStateMachine on {GetType().Name}", StateMachine);

        if (rb == null && TryGetStateMachineUnityComponent(out rb) == false)
            Debug.LogError($"Missing RigidBody on {GetType().Name}", StateMachine);

        colliders = transformState.GetComponentsInChildren<Collider>();
        deadModel.sharedMaterial = playerMat;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        playerModel.SetActive(false); 
        deadGraphics.SetActive(true);
        countdownText.text = deadDuration.ToString();
        countdownText.gameObject.SetActive(true);
        rb.useGravity = false;

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        timer = Time.time + deadDuration;

        StateMachine.StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        while (timer > Time.time)
        {
            rb.velocity = Vector3.zero;
            yield return null;
            countdownText.text = Mathf.CeilToInt(timer - Time.time).ToString();
        }

        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        rb.useGravity = true;

        transformState.position = player.SpawnPosition;

        playerModel.SetActive(true);
        deadGraphics.SetActive(false);
        countdownText.gameObject.SetActive(false);
        StateMachine.SetState(player.NormalState);
    }

}
