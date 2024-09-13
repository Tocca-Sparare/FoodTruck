using System.Collections;
using UnityEngine;

[System.Serializable]
public class TableDirtyState : State
{
    [SerializeField] float cleaningTime;
    [SerializeField] float waitingTime;

    Table table;
    float remaningCleaningTime;
    Coroutine waitingCoroutine;

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

        table.OnDirtyTable?.Invoke(table.LastFood);

        remaningCleaningTime = cleaningTime;

        table.OnCleaning += OnCleaning;
        table.Chairs.ForEach(c => c.OnCustomerSat += OnCustomerSat);
    }

    protected override void OnExit()
    {
        base.OnExit();

        table.OnCleaning -= OnCleaning;
        table.Chairs.ForEach(c => c.OnCustomerSat -= OnCustomerSat);

        if (waitingCoroutine != null)
            StateMachine.StopCoroutine(waitingCoroutine);
    }

    private void OnCleaning(float deltaTime)
    {
        remaningCleaningTime -= deltaTime;
        table.OnUpdateClean?.Invoke(remaningCleaningTime * 100 / cleaningTime);

        if (remaningCleaningTime <= 0)
        {
            remaningCleaningTime = 0;
            table.LastFood = null;
            table.OnTableClean?.Invoke();
            if (table.CustomersOnTableCount > 0)
            {
                StateMachine.SetState(table.OrderReadyState);
            }
            else
            {
                StateMachine.SetState(table.NormalState);
            }
        }
    }

    void OnCustomerSat()
    {
        table.CustomersOnTableCount++;

        if (table.CustomersOnTableCount == table.IncomingCustomersCount)
        {
            waitingCoroutine = StateMachine.StartCoroutine(LeaveAfterWaitingTime());
        }
    }

    IEnumerator LeaveAfterWaitingTime()
    {
        yield return new WaitForSeconds(waitingTime);
        table.Chairs.ForEach(c => c.CustomerSat?.Leave(EOrderSatisfaction.Unsatisfied));
        table.OnOrderNotSatisfied?.Invoke();
    }
}
