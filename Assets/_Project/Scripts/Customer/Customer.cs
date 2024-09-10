using UnityEngine;

public class Customer : BasicStateMachine
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] CustomerNormalState normalState;
    [SerializeField] CustomerSatState satState;
    [SerializeField] CustomerLeavingState leavingState;
    [SerializeField] string blackboardExitPoint = "exitPoint";

    Food demandingFood;
    Chair currentChair;

    public Food DemandingFood => demandingFood;
    public Chair CurrentChair => currentChair;
    public bool IsSat => CurrentState == satState;

    //events
    public System.Action OnSit;
    public System.Action OnStandUp;

    public void Init(Food requestedIngredient, Table targetTable, Vector3 exitPoint)
    {
        SetRequestedIngredient(requestedIngredient);
        SetTargetTable(targetTable);
        SetBlackboardElement(blackboardExitPoint, exitPoint);
        SetState(normalState);
    }

    private void SetRequestedIngredient(Food ingredient)
    {
        demandingFood = ingredient;
        meshRenderer.material = ingredient.material;
    }

    private void SetTargetTable(Table table)
    {
        currentChair = table.GetRandomEmptyChair();
        currentChair.CustomerSit(this);
    }

    public void Sit()
    {
        SetState(satState);
        OnSit?.Invoke();
    }

    public void Exit()
    {
        SetState(leavingState);
        OnStandUp?.Invoke();
        currentChair.CustomerStandUp();
    }
}
