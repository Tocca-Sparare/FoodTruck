using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer meshRenderer;

    NavMeshAgent navMeshAgent;
    Chair targetChair;
    bool isExiting = false;
    Vector3 exitPoint;
    Food demandingFood;

    public Food DemandingFood => demandingFood;
    
    //events
    public System.Action OnSit;
    public System.Action OnStandUp;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void FixedUpdate()
    {
        if (!isExiting && targetChair && Vector3.Distance(targetChair.transform.position, transform.position) < 0.3f)
            Sit();

        if (isExiting && Vector3.Distance(exitPoint, transform.position) < 0.3f)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    public void Init(Food requestedIngredient, Table targetTable, Vector3 exitPoint)
    {
        SetRequestedIngredient(requestedIngredient);
        SetTargetTable(targetTable);
        this.exitPoint = exitPoint;
    }

    private void SetRequestedIngredient(Food ingredient)
    {
        demandingFood = ingredient;
        meshRenderer.material = ingredient.material;
    }

    private void SetTargetTable(Table table)
    {
        targetChair = table.GetRandomEmptyChair();
        targetChair.CustomerSit(this);
        navMeshAgent.destination = targetChair.transform.position;
    }

    public void Sit()
    {
        transform.SetParent(targetChair.transform);
        transform.SetPositionAndRotation(targetChair.transform.position, targetChair.transform.rotation);
        navMeshAgent.enabled = false;
        OnSit?.Invoke();
    }

    public void Exit()
    {
        isExiting = true;
        navMeshAgent.enabled = true;
        navMeshAgent.destination = exitPoint;
        targetChair.CustomerStandUp();
        OnStandUp?.Invoke();
    }
}
