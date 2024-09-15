using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerDeadState : State
{
    [SerializeField] int deadDuration;

    PlayerStateMachine player;
    Rigidbody rb;
    Collider[] colliders;
    float timer;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerStateMachine on {GetType().Name}", StateMachine);

        if (rb == null && TryGetStateMachineUnityComponent(out rb) == false)
            Debug.LogError($"Missing RigidBody on {GetType().Name}", StateMachine);

        colliders = transformState.GetComponentsInChildren<Collider>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        player.OnDead?.Invoke(deadDuration);

        //disable rigidbody and colliders
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        foreach (var collider in colliders)
            collider.enabled = false;

        //start timer
        timer = Time.time + deadDuration;
        StateMachine.StartCoroutine(RespawnPlayer());
    }

    protected override void OnExit()
    {
        base.OnExit();

        //re-enable rigidbody and colliders
        rb.useGravity = true;
        foreach (var collider in colliders)
            collider.enabled = true;
    }

    IEnumerator RespawnPlayer()
    {
        while (timer > Time.time)
        {
            yield return null;

            //continue reset rigidbody velocity to be sure player is still
            rb.velocity = Vector3.zero;
            player.OnUpdateDeadState?.Invoke(timer - Time.time);
        }

        //respawn
        player.RespawnPlayer();
    }
}
