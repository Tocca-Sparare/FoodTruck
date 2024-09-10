using UnityEngine;

/// <summary>
/// This is the default state for the player. Where can move, interact, etc...
/// </summary>
[System.Serializable]
public class PlayerNormalState : State
{
    PlayerPawn player;
    MovementComponent movementComponent;
    InputManager inputManager;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerPawn on {GetType().Name}", StateMachine);
        if (movementComponent == null && TryGetStateMachineUnityComponent(out movementComponent) == false)
            Debug.LogError($"Missing MovementComponent on {GetType().Name}", StateMachine);
        if (player.CurrentController == null || player.CurrentController.TryGetComponent(out inputManager) == false)
            Debug.LogError($"Missing inputManager on {GetType().Name}", StateMachine);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //set move direction
        if (movementComponent && inputManager)
        {
            movementComponent.MoveByInput3D(inputManager.Move);
        }
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        //move
        if (movementComponent != null)
            movementComponent.UpdatePosition();
    }
}
