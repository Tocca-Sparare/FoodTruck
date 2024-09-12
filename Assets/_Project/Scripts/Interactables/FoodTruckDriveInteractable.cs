using PathCreation;
using PathCreation.Examples;
using UnityEngine;

public class FoodTruckDriveInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] float speed = 3;
    [SerializeField] PathCreator pathToFollow;
    [SerializeField] PathFollower pathFollower;
    InteractComponent playerUsingThis;


    public void MoveForward()
    {
        pathFollower.speed = speed;
    }

    public void MoveBackward()
    {
        pathFollower.speed = -speed;
    }

    public void Stop()
    {
        pathFollower.speed = 0;
    }

    public void Dismiss()
    {
        //reset player state
        PlayerStateMachine playerStateMachine = playerUsingThis.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
            playerStateMachine.SetState(playerStateMachine.NormalState);

        //reset rotation
        RotateCharacterFeedback rotateCharacter = playerUsingThis.GetComponentInChildren<RotateCharacterFeedback>();
        if (rotateCharacter)
            rotateCharacter.ForceDirection(false);
    }

    public bool CanInteract(InteractComponent interactor)
    {
        return playerUsingThis == null;
    }

    public void Interact(InteractComponent interactor)
    {
        playerUsingThis = interactor;

        PlayerStateMachine playerStateMachine = interactor.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
        {
            playerStateMachine.SetFoodTruckDriver(this);
            playerStateMachine.SetState(playerStateMachine.DriveState);
        }
    }

}
