using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State when customer is sat on a chair, waiting for food
/// </summary>
[System.Serializable]
public class CustomerSatState : State
{
    [Header("How many seconds the customer is going to wait at the table before leaving")]
    [SerializeField] float waitingTime = 10;

    NavMeshAgent navMeshAgent;
    Customer customer;
    Coroutine leaveTableAfterWaitingTime;

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

        //start timer to leave table unsatisfied
        leaveTableAfterWaitingTime = StateMachine.StartCoroutine(LeaveTableAfterWaitingTime());
    }

    protected override void OnExit()
    {
        base.OnExit();

        //be sure to stop coroutine if leaving this state for other reasons
        if (leaveTableAfterWaitingTime != null)
            StateMachine.StopCoroutine(leaveTableAfterWaitingTime);
    }

    private IEnumerator LeaveTableAfterWaitingTime()
    {
        //after few seconds, leave table unsatisfied
        float timer = Time.time + waitingTime;
        while (Time.time < timer)
        {
            customer.SetTimerBeforeLeave(timer - Time.time);
            yield return null;
        }

        leaveTableAfterWaitingTime = null;
        customer.Leave(false);
    }
}
