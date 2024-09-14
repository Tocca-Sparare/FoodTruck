using UnityEngine;

/// <summary>
/// This is the state for the player when interact with a Cannon. Use this to rotate and shoot, or leave the cannon
/// </summary>
[System.Serializable]
public class PlayerUsingCannonState : State
{
    [SerializeField] bool resetWhenReleaseAnalogInput = false;
    [SerializeField] bool useRaycastForMouse;
    [SerializeField] LayerMaskClass raycastLayer;

    Camera cam;
    PlayerPawn player;
    InputManager inputManager;
    PlayerStateMachine playerStateMachine;
    CannonInteractable cannonInteractable;
    RotateCharacterFeedback rotateCharacter;
    Vector3 lastSavedAnalogDirection;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (cam == null) cam = Camera.main;
        if (cam == null) Debug.LogError($"Missing camera on {GetType().Name}", StateMachine);
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerPawn on {GetType().Name}", StateMachine);
        if (player.CurrentController == null || player.CurrentController.TryGetComponent(out inputManager) == false)
            Debug.LogError($"Missing inputManager on {GetType().Name}", StateMachine);
        playerStateMachine = GetStateMachine<PlayerStateMachine>();
        if (playerStateMachine == null) Debug.LogError($"Statemachine isn't PlayerStateMachine on {GetType().Name}", StateMachine);
        rotateCharacter = player.GetComponentInChildren<RotateCharacterFeedback>();
        if (rotateCharacter == null) Debug.LogError($"Missing RotateCharacterFeedback on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        //get cannon reference
        cannonInteractable = playerStateMachine.Cannon;
        if (cannonInteractable == null)
            Debug.LogError($"Missing cannonInteractable on {GetType().Name}", StateMachine);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (inputManager == null || cannonInteractable == null)
            return;

        //set direction using mouse position
        if (inputManager.IsUsingMouse)
        {
            if (cam)
            {
                CalculateAimPositionWithMouse();
            }
        }
        //or using analog (or keyboard direction)
        else
        {
            CalculateAimWithGamepad();
        }

        //shoot
        if (inputManager.ShootWasPressedThisFrame)
            cannonInteractable.Shoot();

        //leave cannon
        if (inputManager.InteractWasPressedThisFrame)
            cannonInteractable.Dismiss();
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        //follow cannon
        FollowCannonPositionAndRotation();
    }

    void CalculateAimPositionWithMouse()
    {
        //use raycast to find where is the mouse (if hit nothing, use direction)
        if (useRaycastForMouse)
        {
            Ray ray = cam.ScreenPointToRay(inputManager.Aim);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, raycastLayer.Layer))
            {
                cannonInteractable.AimAtPosition(hit.point);
                return;
            }
        }

        //calculate direction from player to mouse position
        cannonInteractable.AimInDirection(CalculateDirectionWithScreenPoints());
    }

    Vector3 CalculateDirectionWithScreenPoints()
    {
        //from owner screen position to mouse position
        Vector3 ownerPos = cam.WorldToScreenPoint(cannonInteractable.transform.position);
        Vector3 direction = (inputManager.Aim - (Vector2)ownerPos).normalized;
        return new Vector3(direction.x, 0, direction.y);
    }

    void CalculateAimWithGamepad()
    {
        //check if moving analog or reset input when released
        if (inputManager.Aim != Vector2.zero || resetWhenReleaseAnalogInput)
        {
            lastSavedAnalogDirection = new Vector3(inputManager.Aim.x, 0, inputManager.Aim.y);  //save input
        }

        cannonInteractable.AimInDirection(lastSavedAnalogDirection);
    }

    void FollowCannonPositionAndRotation()
    {
        if (cannonInteractable == null)
            return;

        //follow cannon position and rotation
        player.transform.position = cannonInteractable.PlayerTransform.position;
        Vector3 rotateDirection = cannonInteractable.transform.position - rotateCharacter.ObjectToRotate.position;
        rotateDirection.y = 0;
        if (rotateCharacter)
            rotateCharacter.ForceDirection(true, rotateDirection);
    }
}
