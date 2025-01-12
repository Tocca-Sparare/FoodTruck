using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Customer in scene. This is a statemachine with states to move to the chair, eat and leave
/// </summary>
public class Customer : BasicStateMachine
{
    [SerializeField] CustomerNormalState normalState;
    [SerializeField] CustomerSatState satState;
    [SerializeField] CustomerLeavingState leavingState;
    [SerializeField][Range(0, 100)] public int[] hungryChangeSteps;

    Food requestedFood;
    Chair currentChair;
    Vector3 exitPoint;
    int hungerLevel = 0;
    Table table;


    public Food RequestedFood => requestedFood;
    public Chair CurrentChair => currentChair;
    public Vector3 ExitPoint => exitPoint;
    public int[] HungryChangeSteps => hungryChangeSteps;
    public int HungerLevel => hungerLevel;
    public bool IsSatisfied => RequestedFood == null;
    public Table Table => table;

    //events
    public System.Action OnInit;
    public System.Action OnSit;
    public System.Action OnStandUp;
    public System.Action OnSatisfied;
    public System.Action OnUnsatisfied;
    public System.Action OnHungryIncreased;
    public System.Action OnSatisfyRequest;

    private void Awake()
    {
        //disable navmesh and auto set position, to avoid problems online (navmesh set position break network transform)
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent)
        {
            agent.enabled = false;
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }
    }

    public void Init(Food requestedFood, Table targetTable, Vector3 exitPoint)
    {
        //set vars
        SetRequestedFood(requestedFood);
        SetTargetTable(targetTable);
        this.exitPoint = exitPoint;
        this.table = targetTable;

        //set state to move to the chair
        StartCoroutine(SetStateAfterSecond());

        OnInit?.Invoke();
    }

    IEnumerator SetStateAfterSecond()
    {
        //wait few seconds to avoid problems online (nav mesh break spawn position if enabled in first frame)
        yield return new WaitForSeconds(0.1f);

        //re-enable navmesh
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent) 
            agent.enabled = true;

        //and set state
        SetState(new PlayerTransitionState(normalState));
    }

    private void SetRequestedFood(Food food)
    {
        //set food
        requestedFood = food;
    }

    private void SetTargetTable(Table table)
    {
        //find random available chair and select as target chair
        currentChair = table.GetRandomAvailableChair();
        currentChair.CustomerSelectTargetChair(this);
    }

    /// <summary>
    /// Sit at table
    /// </summary>
    public void Sit()
    {
        //sat at chair and set state
        currentChair.CustomerSit();
        SetState(satState);

        OnSit?.Invoke();
    }

    /// <summary>
    /// Leave table
    /// </summary>
    /// <param name="satisfied"></param>
    public void Leave(EOrderSatisfaction satisfied) // TODO da rimuovere lo stato qui
    {
        //stand up and set state
        SetState(leavingState);
        currentChair.CustomerStandUp();

        if (satisfied == EOrderSatisfaction.Satisfied)
        {
            OnSatisfied?.Invoke();
        }
        else if (satisfied == EOrderSatisfaction.Unsatisfied)
        {
            OnUnsatisfied?.Invoke();
        }

        OnStandUp?.Invoke();
    }

    public void SatisfyRequest()
    {
        requestedFood = null;
        OnSatisfyRequest?.Invoke();
    }

    public void IncreaseHungerLevel()
    {
        hungerLevel++;
        OnHungryIncreased?.Invoke();
    }
}
