using UnityEngine;

[System.Serializable]
public class PlayerCleaningState : State
{
    [Header("Stop cleaning if move away")]
    [SerializeField] float stopCleaningDistance = 3;
    [SerializeField] LayerMaskClass layerTables;

    PlayerPawn player;
    InputManager inputManager;
    MovementComponent movementComponent;
    PlayerStateMachine playerStateMachine;
    TableInteractable tableInteractable;
    float lastCleaningTime;

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
        playerStateMachine = GetStateMachine<PlayerStateMachine>();
        if (playerStateMachine == null)
            Debug.LogError($"Statemachine isn't PlayerStateMachine on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        tableInteractable = playerStateMachine.Table;
        lastCleaningTime = Time.time;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (inputManager == null || tableInteractable == null)
            return;

        if (movementComponent)
        {
            //set move direction
            if (inputManager)
                movementComponent.MoveByInput3D(inputManager.Move);
        }

        //continue to clean table
        tableInteractable.Clean(Time.time - lastCleaningTime);
        lastCleaningTime = Time.time;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        //move
        if (movementComponent != null)
            movementComponent.UpdatePosition();

        //if move too far or looking away, stop cleaning
        if (CheckIfLookingAtTable() == false)
        {
            tableInteractable.Dismiss();
        }
    }

    bool CheckIfLookingAtTable()
    {
        //update direction for interactables (ignore vector3.zero)
        if (movementComponent.MoveDirectionInput != Vector3.zero)
            movementDirection = movementComponent.MoveDirectionInput;
        else
            return true;    //if move direction is always 0, then player is still in front of the table from when he pressed interact

        //check in front of player (with max distance)
        if (PhysicsHelper.Raycast(transformState.position, movementDirection, out RaycastHit hit, stopCleaningDistance, layerTables.Layer))
        {
            //if there is still our table
            TableInteractable test = hit.collider.GetComponentInParent<TableInteractable>();
            if (test == tableInteractable)
            {
                return true;
            }
        }

        return false;
    }
}
