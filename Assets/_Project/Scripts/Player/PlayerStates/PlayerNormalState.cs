using UnityEngine;

/// <summary>
/// This is the default state for the player. Where can move, interact, etc...
/// </summary>
[System.Serializable]
public class PlayerNormalState : State
{
    PlayerPawn player;
    InputManager inputManager;
    MovementComponent movementComponent;
    InteractComponent interactComponent;

    Vector3 movementDirection;

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
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (movementComponent)
        {
            //set move direction
            if (inputManager)
                movementComponent.MoveByInput3D(inputManager.Move);

            //and update direction for interactables (ignore vector3.zero)
            if (movementComponent.MoveDirectionInput != Vector3.zero)
                movementDirection = movementComponent.MoveDirectionInput;
        }

        //try interact
        if (interactComponent)
        {
            //interactComponent.ScanInteractablesInDirection(movementDirection);
            interactComponent.ScanInteractables();

            if (inputManager && inputManager.InteractWasPressedThisFrame)
                interactComponent.TryInteract();
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
