using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// State when customer stand up and go outside of the scene
/// </summary>
[System.Serializable]
public class CustomerLeavingState : State
{
    [SerializeField] float lifeTime = 10;

    NavMeshAgent navMeshAgent;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (navMeshAgent == null && TryGetStateMachineUnityComponent(out navMeshAgent) == false)
            Debug.LogError($"Missing navMeshAgent on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        //enable nav mesh and move to exit point
        navMeshAgent.enabled = true;
        navMeshAgent.destination = GetStateMachine<Customer>().ExitPoint;

        //start timer to destroy
        StateMachine.StartCoroutine(LifeTimeCoroutine());
    }

    private IEnumerator LifeTimeCoroutine()
    {
        //after few seconds, destroy
        yield return new WaitForSeconds(lifeTime);
        InstantiateHelper.Destroy(StateMachine.gameObject);
    }
}
