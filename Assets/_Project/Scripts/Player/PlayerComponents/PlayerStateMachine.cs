
/// <summary>
/// This is the statemachine for every PlayerPawn in game
/// </summary>
public class PlayerStateMachine : BasicStateMachine
{
    public PlayerNormalState NormalState;
    public PlayerUsingCannonState UsingCannonState;
    public PlayerCleaningState PlayerCleaningState;

    public CannonInteractable Cannon { get; private set; }
    public TableInteractable Table { get; private set; }

    
    public void SetCannon(CannonInteractable cannon)
    {
        Cannon = cannon;
    }

    public void SetTable(TableInteractable table)
    {
        Table = table;
    }
}
