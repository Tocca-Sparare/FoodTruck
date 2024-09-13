
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TableNormalState : State
{
    Table table;

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

        table.IncomingCustomersCount = 0;
        table.CustomersOnTableCount = 0;
        
        table.OnHit += OnTableHit;
        table.Chairs.ForEach(c => c.OnCustomerSat += table.OnCustomerSat);
    }

    protected override void OnExit()
    {
        base.OnExit();
        
        table.OnHit -= OnTableHit;
        table.Chairs.ForEach(c => c.OnCustomerSat -= table.OnCustomerSat);
    }

    void OnTableHit(Food food)
    {
        table.LastFood = food;
        StateMachine.SetState(table.DirtyState);
    }
}
