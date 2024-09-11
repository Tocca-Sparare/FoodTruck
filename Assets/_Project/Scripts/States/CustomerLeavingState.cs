using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class CustomerLeavingState : State
{
    [SerializeField] float lifeTime = 10;

    NavMeshAgent navMeshAgent;

    protected override void OnInit()
    {
        base.OnInit();

        navMeshAgent = GetStateMachineUnityComponent<NavMeshAgent>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        navMeshAgent.enabled = true;
        navMeshAgent.destination = GetStateMachine<Customer>().ExitPoint;
        StateMachine.StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        InstantiateHelper.Destroy(StateMachine.gameObject);
    }
}
