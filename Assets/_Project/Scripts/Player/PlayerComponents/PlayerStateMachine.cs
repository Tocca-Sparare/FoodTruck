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
    public PlayerTransportingObjectsState TransportingObjectsState;
    public PlayerDeadState DeadState;
    public PlayerPauseState PauseState;

    public TransportableObject TransportedObject { get; private set; }
    public CannonInteractable Cannon { get; private set; }
    public TableInteractable Table { get; private set; }
    public Vector3 SpawnPosition { get; private set; }

    public Transform TransportedObjectContainer => transportedObjectContainer;

    public System.Action<float> OnDead;
    public System.Action<float> OnUpdateDeadState;
    public System.Action OnRespawn;

    private void Awake()
    {
        SpawnPosition = transform.position;
    }

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

    public void Drop()
    {
        if (TransportedObject)
            TransportedObject.Drop();
    }

    public void KillPlayer()
    {
        //if transporting an object, drop it
        if (CurrentState == TransportingObjectsState)
            Drop();

        //set in dead state
        SetState(DeadState);
    }

    public void RespawnPlayer()
    {
        //if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
        //    transform.GetComponent<Fusion.Addons.Physics.NetworkRigidbody3D>().Teleport(SpawnPosition);

        //reset position and set normal state
        transform.position = SpawnPosition;
        SetState(NormalState);

        OnRespawn?.Invoke();
    }
}
