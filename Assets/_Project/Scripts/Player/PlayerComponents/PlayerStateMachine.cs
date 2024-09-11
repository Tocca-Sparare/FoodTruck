
/// <summary>
/// This is the statemachine for every PlayerPawn in game
/// </summary>
public class PlayerStateMachine : BasicStateMachine
{
    public PlayerNormalState NormalState;
    public PlayerUsingCannonState UsingCannonState;

    private void Start()
    {
        //set first state
        SetState(NormalState);
    }
}
