using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Customer in scene. This is a statemachine with states to move to the chair, eat and leave
/// </summary>
public class Customer : BasicStateMachine
{
    [Header("How many seconds the customer is going to wait at the table before leaving")]
    [SerializeField] float waitingTime = 10;
    [SerializeField] CustomerNormalState normalState;
    [SerializeField] CustomerSatState satState;
    [SerializeField] CustomerLeavingState leavingState;

    Food demandingFood;
    Chair currentChair;
    Vector3 exitPoint;
    float remainingTimeBeforeLeave;

    public Food DemandingFood => demandingFood;
    public Chair CurrentChair => currentChair;
    public Vector3 ExitPoint => exitPoint;
    public float WaitingTime => waitingTime;
    public float RemainingTimeBeforeLeave => remainingTimeBeforeLeave;

    //events
    public System.Action OnInit;
    public System.Action OnSit;
    public System.Action OnStandUp;
    public System.Action OnSatisfied;
    public System.Action OnUnsatisfied;

    public void Init(Food requestedFood, Table targetTable, Vector3 exitPoint)
    {
        //set vars
        SetRequestedFood(requestedFood);
        SetTargetTable(targetTable);
        this.exitPoint = exitPoint;

        //set state to move to the chair
        SetState(normalState);

        OnInit?.Invoke();
    }

    private void SetRequestedFood(Food ingredient)
    {
        //set food
        demandingFood = ingredient;
    }

    private void SetTargetTable(Table table)
    {
        //find random available chair and select as target chair
        currentChair = table.GetRandomAvailableChair();
        currentChair.CustomerSelectTargetChair(this);
    }

    /// <summary>
    /// Sit at table
    /// </summary>
    public void Sit()
    {
        //sat at chair and set state
        currentChair.CustomerSit();
        SetState(satState);

        OnSit?.Invoke();
    }

    /// <summary>
    /// Leave table
    /// </summary>
    /// <param name="satisfied"></param>
    public void Leave(ECustomerSatisfaction satisfied)
    {
        //stand up and set state
        SetState(leavingState);
        currentChair.CustomerStandUp();

        if (satisfied == ECustomerSatisfaction.Satisfied)
            OnSatisfied?.Invoke();
        else if (satisfied == ECustomerSatisfaction.Unsatisfied)
            OnUnsatisfied?.Invoke();

        OnStandUp?.Invoke();
    }

    /// <summary>
    /// Set remaining time. At the end, the customer will leave unsatisfied, without give points
    /// </summary>
    /// <param name="remainingTime"></param>
    public void SetTimerBeforeLeave(float remainingTime)
    {
        remainingTimeBeforeLeave = remainingTime;
    }
}
