
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

        table.Chairs = table.GetComponentsInChildren<Chair>().ToList();
        table.Chairs.ForEach(c => c.OnCustomerSat += OnCustomerSat);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        table.OnHit += OnTableHit;
    }

    protected override void OnExit()
    {
        base.OnExit();
        
        table.OnHit -= OnTableHit;
    }

    public void OnCustomerSat()
    {
        table.CustomersOnTableCount++;

        if (table.CustomersOnTableCount == table.IncomingCustomersCount)
        {
            if (table.IsDirty)
            {
                StateMachine.SetState(GetStateMachine<Table>().DirtyState);
            }
            else
            {
                StateMachine.SetState(GetStateMachine<Table>().OrderReadyState);
            }
        }
    }

    void OnTableHit(Food food)
    {
        //TODO food non viene usato per colorare le macchie 
        StateMachine.SetState(table.DirtyState);
    }
}
