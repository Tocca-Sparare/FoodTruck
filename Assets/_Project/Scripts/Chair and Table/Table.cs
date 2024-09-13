using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A table in scene, with a list of chairs. Every customer can sit on a chair if available
/// </summary>
public class Table : BasicStateMachine
{
    public TableNormalState NormalState;
    public TableOrderReadyState OrderReadyState;
    public TableDirtyState DirtyState;


    [SerializeField] Transform physicalTablePosition;
    public List<Chair> Chairs { get; set; }
    public int IncomingCustomersCount { get; set; }
    public int CustomersOnTableCount { get; set; }
    public int HungerLevel { get; set; }

    //events
    public System.Action<Food> OnDirtyTable;
    public System.Action OnOrderReady;
    public System.Action<int> OnHungerLevelIncreased;
    public System.Action OnOrderSatisfied;
    public System.Action OnOrderNotSatisfied;
    public System.Action<Food> OnHit;
    public System.Action<float> OnCleaning;
    public System.Action<float> OnUpdateClean;
    public System.Action OnTableClean;

    public Vector3 PhysicalTablePosition => physicalTablePosition.position;
    public bool IsAvailable => !IsDirty && Chairs.All(c => c.IsAvailable);
    public bool IsDirty => CurrentState == DirtyState;


    void Awake()
    {
        SetState(NormalState);
    }

    public Chair GetRandomAvailableChair()
    {
        //find available chairs
        var emptyChairs = Chairs.Where(c => c.IsAvailable).ToList();
        if (emptyChairs.Count == 0)
        {
            Debug.LogError($"There aren't emptyChairs in {name}", gameObject);
            return null;
        }

        //return random one
        int randomIndex = Random.Range(0, emptyChairs.Count);
        return emptyChairs[randomIndex];
    }

    public void OnHitTable(Food food)
    {
        OnHit?.Invoke(food);
    }

    public void SetApproachingCustomersCount(int count)
    {
        SetState(NormalState);
        IncomingCustomersCount = count;
        CustomersOnTableCount = 0;
    }

    public void Clean(float deltaTime)
    {
        OnCleaning?.Invoke(deltaTime);
    }
    
    public void OnCustomerSat()
    {
        CustomersOnTableCount++;

        if (CustomersOnTableCount == IncomingCustomersCount)
        {
            if (!IsDirty)
            {
                SetState(OrderReadyState);
            }
        }
    }
}
