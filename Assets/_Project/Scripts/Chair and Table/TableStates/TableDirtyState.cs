using UnityEngine;

[System.Serializable]
public class TableDirtyState : State
{
    [SerializeField] float cleaningTime;

    Table table;
    float remaningCleaningTime;

    protected override void OnInit()
    {
        base.OnInit();

        table = GetStateMachine<Table>();
        if (table == null && TryGetStateMachineUnityComponent(out table) == false)
            Debug.LogError($"Missing Table on {GetType().Name}", StateMachine);

    }

    protected override void OnEnter()
    {
        base.OnEnter();

        table.OnCleaning += DoCleaning;

        remaningCleaningTime = cleaningTime;
        table.OnDirtyTable?.Invoke(null);

        foreach (var c in table.Chairs)
        {
            if (c.IsCustomerSat)
                c.CustomerSat.Leave(EOrderSatisfaction.Unsatisfied);     //unsatisfied if already sat
            else if (c.IsAvailable == false)
                c.CustomerSat.Leave(EOrderSatisfaction.Indifferent);     //else, indifferent (don't lose points)
        }
    }

    protected override void OnExit()
    {
        base.OnExit();

        table.OnCleaning -= DoCleaning;
    }

    public void DoCleaning(float deltaTime)
    {
        remaningCleaningTime -= deltaTime;
        // table.OnCleaning?.Invoke(remaningCleaningTime * 100 / cleaningTime); // TODO non si aggiorna il loading bar

        if (remaningCleaningTime < 0)
        {
            remaningCleaningTime = 0;
            table.OnCleanTable?.Invoke();
            StateMachine.SetState(table.NormalState);
        }
    }
}
