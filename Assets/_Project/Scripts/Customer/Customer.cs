using System.Collections;
using UnityEngine;

public class Customer : BasicStateMachine
{
    [Header("How many seconds the customer is going to wait at the table before leaving")]
    [SerializeField] float waitingTime = 10;
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] CustomerNormalState normalState;
    [SerializeField] CustomerSatState satState;
    [SerializeField] CustomerLeavingState leavingState;

    Food demandingFood;
    Chair currentChair;

    public Food DemandingFood => demandingFood;
    public Chair CurrentChair => currentChair;
    public bool IsSat => CurrentState == satState;
    public Vector3 ExitPoint { get; private set; }

    //events
    public System.Action OnSit;
    public System.Action OnStandUp;
    public System.Action OnSatisfied;

    public void Init(Food requestedIngredient, Table targetTable, Vector3 exitPoint)
    {
        ExitPoint = exitPoint;

        SetRequestedIngredient(requestedIngredient);
        SetTargetTable(targetTable);
        SetState(normalState);
    }

    private void SetRequestedIngredient(Food ingredient)
    {
        demandingFood = ingredient;
        meshRenderer.sharedMaterial = ingredient.material;
    }

    private void SetTargetTable(Table table)
    {
        currentChair = table.GetRandomEmptyChair();
        currentChair.CustomerSit(this);
    }

    public void Sit()
    {
        StartCoroutine(LeaveTableAfterWaitingTime());
        SetState(satState);
        OnSit?.Invoke();
    }

    public void Leave(bool satisfied)
    {
        SetState(leavingState);
        currentChair.CustomerStandUp();

        if (satisfied)
            OnSatisfied?.Invoke();
        OnStandUp?.Invoke();
    }

    IEnumerator LeaveTableAfterWaitingTime()
    {
        yield return new WaitForSeconds(waitingTime);
        if (IsSat)
            Leave(false);
    }
}
