using UnityEngine;

/// <summary>
/// State when transport objects. Can't interact with other objects, only release the transported one
/// </summary>
[System.Serializable]
public class PlayerTrasnportingObjectsState : State
{
    [SerializeField] float throwForce = 10f;
    [SerializeField] float verticalThrow = 0.5f;

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

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //move transported object
        if (playerStateMachine.TransportedObject)
        {
            playerStateMachine.TransportedObject.transform.position = playerStateMachine.TransportedObjectContainer.position;
            playerStateMachine.TransportedObject.transform.rotation = playerStateMachine.TransportedObjectContainer.rotation;
        }

        if (inputManager == null)
            return;

        //set move direction
        if (movementComponent)
        {            
            movementComponent.MoveByInput3D(inputManager.Move);

            //and update direction for interactables (ignore vector3.zero)
            if (movementComponent.MoveDirectionInput != Vector3.zero)
                moveDirection = movementComponent.MoveDirectionInput;
        }

        //when press interact, release object
        if (inputManager.InteractWasPressedThisFrame)
        {
            //if should interact with cannon, put bullet inside it
            if (interactComponent)
            {
                //interactComponent.ScanInteractablesInDirection(movementDirection);
                interactComponent.ScanInteractables();

                if (interactComponent.CurrentInteractable is CannonInteractable cannon)
                {
                    cannon.InsertBullet(playerStateMachine.TransportedObject.BulletPrefab);
                    InstantiateHelper.Destroy(playerStateMachine.TransportedObject.gameObject);
                }
            }

            if (playerStateMachine.TransportedObject)
                playerStateMachine.TransportedObject.Drop();

            return;
        }

        //when press shoot, throw object
        if (inputManager.ShootWasPressedThisFrame)
        {
            if (playerStateMachine.TransportedObject)
                playerStateMachine.TransportedObject.Throw(new Vector3(moveDirection.x, verticalThrow, moveDirection.z), throwForce);
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
