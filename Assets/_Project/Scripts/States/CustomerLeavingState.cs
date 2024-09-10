using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class CustomerLeavingState : State
{
    [SerializeField] float lifeTime = 10;
    [SerializeField] VarOrBlackboard<Vector3> blackboardExitPoint = new VarOrBlackboard<Vector3>("exitPoint");

    NavMeshAgent navMeshAgent;

    protected override void OnInit()
    {
        base.OnInit();

        navMeshAgent = GetStateMachineUnityComponent<NavMeshAgent>();
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        Vector3 exitPoint = GetValue(blackboardExitPoint);
        navMeshAgent.enabled = true;
        navMeshAgent.destination = exitPoint;
        StateMachine.StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(lifeTime);
        InstantiateHelper.Destroy(StateMachine.gameObject);
    }
}
