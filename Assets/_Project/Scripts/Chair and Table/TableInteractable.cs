using UnityEngine;

public class TableInteractable : MonoBehaviour, IInteractable
{
    InteractComponent interactingPlayer;
    Table table;

    public System.Action OnDismiss;

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
        PlayerStateMachine playerStateMachine = interactingPlayer.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
        {
            playerStateMachine.SetState(playerStateMachine.NormalState);
        }

        interactingPlayer = null;

        OnDismiss?.Invoke();
    }

    public void Clean(float deltaTime)
    {
        table.Clean(deltaTime);
    }
}
