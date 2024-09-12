
/// <summary>
/// Use this state to skip one frame before change state
/// </summary>
public class PlayerTransitionState : State
{
    State nextState;
    bool skipOneFrame;

    public PlayerTransitionState(State nextState) : base() 
    { 
        this.nextState = nextState;
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        skipOneFrame = true;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //skip one frame
        if (skipOneFrame)
        {
            skipOneFrame = false;
            return;
        }

        //then set state
        StateMachine.SetState(nextState);
    }
}
