using UnityEngine;

public class Customer : BasicStateMachine
{
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
