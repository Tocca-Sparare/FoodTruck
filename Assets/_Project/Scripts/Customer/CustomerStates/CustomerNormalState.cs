using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State when customer is spawned and move to the chair
/// </summary>
[System.Serializable]
public class CustomerNormalState : State
{
    [SerializeField] float snapChairDistance = 0.3f;

    NavMeshAgent navMeshAgent;
    Customer customer;
    Chair currentChair;

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

        //set navmesh destination to chair position
        currentChair = customer.CurrentChair;
        navMeshAgent.destination = currentChair.transform.position;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        //update position and rotation
        transformState.rotation = Quaternion.LookRotation(navMeshAgent.nextPosition - transformState.position, Vector3.up);
        transformState.position = navMeshAgent.nextPosition;

        //when reach chair, sit
        if (currentChair && Vector3.Distance(currentChair.transform.position, transformState.position) < snapChairDistance)
            customer.Sit();
    }
}
