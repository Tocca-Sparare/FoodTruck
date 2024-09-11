using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class CustomerNormalState : State
{
    [SerializeField] float snapChairDistance = 0.3f;

    NavMeshAgent navMeshAgent;
    Chair currentChair;

    protected override void OnInit()
    {
        base.OnInit();

        navMeshAgent = GetStateMachineUnityComponent<NavMeshAgent>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        currentChair = GetStateMachine<Customer>().CurrentChair;
        navMeshAgent.destination = currentChair.transform.position;
    }

    protected override void OnFixedUpdate()
    {
        base.OnFixedUpdate();

        if (currentChair && Vector3.Distance(currentChair.transform.position, transformState.position) < snapChairDistance)
            GetStateMachine<Customer>().Sit();
    }
}
