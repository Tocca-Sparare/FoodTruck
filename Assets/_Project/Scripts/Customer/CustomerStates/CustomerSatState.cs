using UnityEngine.AI;

[System.Serializable]
public class CustomerSatState : State
{
    NavMeshAgent navMeshAgent;

    protected override void OnInit()
    {
        base.OnInit();

        navMeshAgent = GetStateMachineUnityComponent<NavMeshAgent>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Chair targetChair = GetStateMachine<Customer>().CurrentChair;
        transformState.SetParent(targetChair.transform);
        transformState.SetPositionAndRotation(targetChair.transform.position, targetChair.transform.rotation);
        navMeshAgent.enabled = false;
    }
}
