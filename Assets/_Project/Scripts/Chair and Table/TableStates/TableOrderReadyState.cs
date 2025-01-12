using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TableOrderReadyState : State
{
    TablesManager tablesManager;
    Table table;
    Coroutine freeTableCoroutine;
    List<Coroutine> hungerIncreseCoroutines = new List<Coroutine>();
    int satisfiedCustomersCount = 0;

    protected override void OnInit()
    {
        base.OnInit();

        table = GetStateMachine<Table>();
        if (table == null && TryGetStateMachineUnityComponent(out table) == false)
            Debug.LogError($"Missing Table on {GetType().Name}", StateMachine);
        tablesManager = Object.FindObjectOfType<TablesManager>();
        if (tablesManager == null) Debug.LogError($"Missing TableManager on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        table.OnOrderReady?.Invoke();

        table.OnHit += OnTableHit;

        table.HungerLevel = 0;
        satisfiedCustomersCount = 0;

        freeTableCoroutine = StateMachine.StartCoroutine(FreeTableAfterWaitingTime());
        StartHungerIncreaseCoroutines();
    }

    protected override void OnExit()
    {
        base.OnExit();

        table.OnHit -= OnTableHit;

        table.IncomingCustomersCount = 0;
        table.CustomersOnTableCount = 0;

        if (freeTableCoroutine != null)
            StateMachine.StopCoroutine(freeTableCoroutine);
        foreach (var coroutine in hungerIncreseCoroutines)
            StateMachine.StopCoroutine(coroutine);
    }

    private void StartHungerIncreaseCoroutines()
    {
        foreach (var delay in tablesManager.WarningDelays)
        {
            var delayInSeconds = tablesManager.WaitingTime * delay / 100;
            hungerIncreseCoroutines.Add(StateMachine.StartCoroutine(IncreaseHungerAfter(delayInSeconds)));
        }
    }

    IEnumerator FreeTableAfterWaitingTime()
    {
        yield return new WaitForSeconds(tablesManager.WaitingTime);
        Free(EOrderSatisfaction.Unsatisfied);
    }

    IEnumerator IncreaseHungerAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        table.HungerLevel++;
        table.OnHungerLevelIncreased?.Invoke(table.HungerLevel);
    }

    void Free(EOrderSatisfaction satisfaction)
    {
        table.Chairs.ForEach(c => c.CustomerSat?.Leave(satisfaction));

        if (satisfaction == EOrderSatisfaction.Satisfied)
        {
            table.OnOrderSatisfied?.Invoke();
            StateMachine.SetState(table.DirtyState);
        }
        else
        {
            table.OnOrderNotSatisfied?.Invoke();
            StateMachine.SetState(table.NormalState);
        }
    }

    void OnTableHit(Food food)
    {
        table.LastFood = food;

        Chair chair = table.Chairs.Where(
            c => c.IsCustomerSat
            && !c.CustomerSat.IsSatisfied
            && c.CustomerSat.RequestedFood.FoodName == food.FoodName)
            .FirstOrDefault();

        if (chair)
            SetCustomerSatisfy(chair.CustomerSat);
        else
        {
            table.Chairs.ForEach(c => c.CustomerSat?.Leave(EOrderSatisfaction.Unsatisfied));
            table.OnOrderNotSatisfied?.Invoke();
            StateMachine.SetState(table.DirtyState);
        }
    }

    void SetCustomerSatisfy(Customer customer)
    {
        customer.SatisfyRequest();
        satisfiedCustomersCount++;
        if (satisfiedCustomersCount == table.CustomersOnTableCount)
        {
            Free(EOrderSatisfaction.Satisfied);
        }
    }
}
