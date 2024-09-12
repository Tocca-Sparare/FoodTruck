using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A table in scene, with a list of chairs. Every customer can sit on a chair if available
/// </summary>
public class Table : MonoBehaviour
{
    [SerializeField] Transform physicalTablePosition;
    [SerializeField] int waitingTime;
    [SerializeField]
    [Range(0, 100)]
    int[] warningDelays;

    private List<Chair> chairs = new();
    private bool isDirty;
    private int approachingCustomersCount;
    private int satCustomersCount;
    private Coroutine freeTableCoroutine;
    private int hungerLevel;

    //events
    public System.Action<Food> OnDirtyTable;
    public System.Action OnCleanTable;
    public System.Action OnOrderReady;
    public System.Action OnHungerLevelIncreased;

    public Vector3 PhysicalTablePosition => physicalTablePosition.position;
    public bool IsAvailable => !isDirty && chairs.All(c => c.IsAvailable);
    public bool IsDirty => isDirty;
    private bool IsAllCustomersSatisfied()
        => chairs.All(c => c.CustomerSat == null || c.CustomerSat.IsSatisfied);



    void Awake()
    {
        //get refs
        chairs = GetComponentsInChildren<Chair>().ToList();
        chairs.ForEach(c => c.OnCustomerSat += OnCustomerSat);
    }

    /// <summary>
    /// Find random available chair at this table
    /// </summary>
    /// <returns></returns>
    public Chair GetRandomAvailableChair()
    {
        //find available chairs
        var emptyChairs = chairs.Where(c => c.IsAvailable).ToList();
        if (emptyChairs.Count == 0)
        {
            Debug.LogError($"There aren't emptyChairs in {name}", gameObject);
            return null;
        }

        //return random one
        int randomIndex = Random.Range(0, emptyChairs.Count);
        return emptyChairs[randomIndex];
    }

    /// <summary>
    /// Find a customer with this demanding food and make it stand up
    /// </summary>
    /// <param name="food"></param>
    public void OnHitTable(Food food)
    {
        //find customers with this food sat at table
        Chair chair = chairs.Where(
            c => c.IsCustomerSat
            && !c.CustomerSat.IsSatisfied
            && c.CustomerSat.RequestedFood.FoodName == food.FoodName)
            .FirstOrDefault();

        if (chair)
            SetCustomerSatisfy(chair.CustomerSat);
        else
            DirtyTable(food);
    }

    /// <summary>
    /// set this table dirty
    /// </summary>
    public void DirtyTable(Food food)
    {
        //everyone leave
        foreach (var c in chairs)
        {
            if (c.IsCustomerSat)
                c.CustomerSat.Leave(ECustomerSatisfaction.Unsatisfied);     //unsatisfied if already sat
            else if (c.IsAvailable == false)
                c.CustomerSat.Leave(ECustomerSatisfaction.Indifferent);     //else, indifferent (don't lose points)
        }

        isDirty = true;
        OnDirtyTable?.Invoke(food);
    }

    /// <summary>
    /// Set table no more dirty
    /// </summary>
    public void CleanTable()
    {
        isDirty = false;
        OnCleanTable?.Invoke();
    }

    public void SetApproachingCustomersCount(int count)
    {
        approachingCustomersCount = count;
        satCustomersCount = 0;
    }

    void SetCustomerSatisfy(Customer customer)
    {
        customer.SatisfyRequest();
        if (IsAllCustomersSatisfied())
        {
            Free(ECustomerSatisfaction.Satisfied);
            StopCoroutine(freeTableCoroutine);
        }
    }

    void OnCustomerSat()
    {
        satCustomersCount++;

        if (satCustomersCount == approachingCustomersCount) // table is complete
        {
            OnOrderReady?.Invoke();
            freeTableCoroutine = StartCoroutine(FreeTableAfterWaitingTime());
            StartWarningsCoroutines();
        }
    }

    private void StartWarningsCoroutines()
    {
        foreach (var delay in warningDelays)
        {
            var delayInSeconds = waitingTime * delay / 100;
            StartCoroutine(IncreaseHungerLevelAfter(delayInSeconds));
        }
    }

    IEnumerator FreeTableAfterWaitingTime()
    {
        yield return new WaitForSeconds(waitingTime);
        Free(ECustomerSatisfaction.Unsatisfied);
    }

    IEnumerator IncreaseHungerLevelAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        hungerLevel++;
        OnHungerLevelIncreased?.Invoke();
    }

    void Free(ECustomerSatisfaction satisfaction)
        => chairs.ForEach(c => c.CustomerSat?.Leave(satisfaction));
}
