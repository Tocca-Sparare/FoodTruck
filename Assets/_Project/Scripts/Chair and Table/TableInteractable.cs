using UnityEngine;

public class TableInteractable : MonoBehaviour, IInteractable
{
    InteractComponent interactingPlayer;
    Table table;

    void Awake()
    {
        table = GetComponent<Table>();

        if (table == null)
            Debug.LogError($"Missing Table component in {gameObject.name}", gameObject);
    }

    public bool CanInteract()
        => interactingPlayer == null;

    public void Interact(InteractComponent interactor)
    {
        interactingPlayer = interactor;

        PlayerStateMachine playerStateMachine = interactor.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
        {
            playerStateMachine.SetTable(this);
            playerStateMachine.SetState(playerStateMachine.PlayerCleaningState);
        }
    }

    public void Dismiss()
    {
        interactingPlayer = null;
    }

    public void Clean(float deltaTime)
    {
       table.DoCleaning(deltaTime);
    }
}
