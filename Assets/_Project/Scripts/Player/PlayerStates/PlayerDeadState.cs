using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerDeadState : State
{
    [SerializeField] int deadDuration;

    PlayerPawn player;
    InputManager inputManager;
    MovementComponent movementComponent;
    InteractComponent interactComponent;
    PlayerStateMachine playerStateMachine;

    Vector3 moveDirection;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerPawn on {GetType().Name}", StateMachine);
        if (player.CurrentController == null || player.CurrentController.TryGetComponent(out inputManager) == false)
            Debug.LogError($"Missing inputManager on {GetType().Name}", StateMachine);
        if (movementComponent == null && TryGetStateMachineUnityComponent(out movementComponent) == false)
            Debug.LogError($"Missing MovementComponent on {GetType().Name}", StateMachine);
        if (interactComponent == null && TryGetStateMachineUnityComponent(out interactComponent) == false)
            Debug.LogError($"Missing interactComponent on {GetType().Name}", StateMachine);
        playerStateMachine = GetStateMachine<PlayerStateMachine>();
        if (playerStateMachine == null) Debug.LogError($"Missing PlayerStateMachine on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        player.Kill();

        StateMachine.StartCoroutine(RespawnPlayer());
    }

    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(deadDuration);

        player.Respawn();
    }

}
