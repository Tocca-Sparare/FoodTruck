using UnityEngine;

/// <summary>
/// This is the statemachine for every PlayerPawn in game
/// </summary>
public class PlayerStateMachine : BasicStateMachine
{
    [SerializeField] Transform transportedObjectContainer;
    public PlayerNormalState NormalState;
    public PlayerUsingCannonState UsingCannonState;
    public PlayerCleaningState PlayerCleaningState;
    public PlayerTrasnportingObjectsState TrasnportingObjectsState;

    public TransportableObject TransportedObject { get; private set; }
    public CannonInteractable Cannon { get; private set; }
    public TableInteractable Table { get; private set; }
    public Transform TransportedObjectContainer => transportedObjectContainer;

    public void SetTrasportingObject(TransportableObject obj)
    {
        TransportedObject = obj;
    }
    
    public void SetCannon(CannonInteractable cannon)
    {
        Cannon = cannon;
    }

    public void SetTable(TableInteractable table)
    {
        Table = table;
    }
}
