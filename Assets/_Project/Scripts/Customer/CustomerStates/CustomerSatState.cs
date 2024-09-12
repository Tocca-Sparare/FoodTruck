using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State when customer is sat on a chair, waiting for food
/// </summary>
[System.Serializable]
public class CustomerSatState : State
{
    NavMeshAgent navMeshAgent;
    Customer customer;
    Coroutine leaveTableAfterWaitingTime;
    List<Coroutine> hungerCoroutines = new List<Coroutine>();

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (navMeshAgent == null && TryGetStateMachineUnityComponent(out navMeshAgent) == false)
            Debug.LogError($"Missing navMeshAgent on {GetType().Name}", StateMachine);
        customer = GetStateMachine<Customer>();
        if (customer == null) Debug.LogError($"Statemachine isn't Customer on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        //disable navmesh
        navMeshAgent.enabled = false;

        //and snap to the chair
        Chair targetChair = customer.CurrentChair;
        transformState.SetParent(targetChair.transform);
        transformState.SetPositionAndRotation(targetChair.transform.position, targetChair.transform.rotation);
    }

    protected override void OnExit()
    {
        base.OnExit();

        //be sure to stop coroutine if leaving this state for other reasons
        if (leaveTableAfterWaitingTime != null)
            StateMachine.StopCoroutine(leaveTableAfterWaitingTime);

        foreach (var cor in hungerCoroutines)
            StateMachine.StopCoroutine(cor);
    }

    IEnumerator InvokeHungerChangeAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        customer.IncreaseHunger();
    }
}
