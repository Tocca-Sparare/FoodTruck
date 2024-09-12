
/// <summary>
/// This is the statemachine for every PlayerPawn in game
/// </summary>
public class PlayerStateMachine : BasicStateMachine
{
    public PlayerNormalState NormalState;
    public PlayerUsingCannonState UsingCannonState;
    public PlayerDriveState DriveState;

    public CannonInteractable Cannon { get; private set; }
    public FoodTruckDriveInteractable FoodTruckDriver { get; private set; }

    /// <summary>
    /// Set cannon to use in PlayerUsingCannonState
    /// </summary>
    /// <param name="cannon"></param>
    public void SetCannon(CannonInteractable cannon)
    {
        Cannon = cannon;
    }

    /// <summary>
    /// Set FoodTruckDriver to use in PlayerDriveState
    /// </summary>
    /// <param name="foodTruckDriver"></param>
    public void SetFoodTruckDriver(FoodTruckDriveInteractable foodTruckDriver)
    {
        FoodTruckDriver = foodTruckDriver;
    }
}
