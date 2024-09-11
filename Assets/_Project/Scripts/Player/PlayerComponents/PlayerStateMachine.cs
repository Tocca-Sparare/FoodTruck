
/// <summary>
/// This is the statemachine for every PlayerPawn in game
/// </summary>
public class PlayerStateMachine : BasicStateMachine
{
    public PlayerNormalState NormalState;
    public PlayerUsingCannonState UsingCannonState;

    public CannonInteractable Cannon { get; private set; }

    private void Start()
    {
        //set first state
        SetState(NormalState);
    }

    /// <summary>
    /// Set cannon to use in PlayerUsingCannonState
    /// </summary>
    /// <param name="cannon"></param>
    public void SetCannon(CannonInteractable cannon)
    {
        Cannon = cannon;
    }
}
