using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A table in scene, with a list of chairs. Every customer can sit on a chair if available
/// </summary>
public class Table : MonoBehaviour
{
    [SerializeField] Transform physicalTablePosition;
    public Vector3 PhysicalTablePosition => physicalTablePosition.position;

    private List<Chair> chairs = new();
    private bool isDirty;

    public System.Action OnDirtyTable;

    public bool IsAvailable => isDirty == false && chairs.Any(c => c.IsAvailable);  //at least one chair available and table not dirty
    public bool IsDirty => isDirty;


    void Awake()
    {
        chairs = GetComponentsInChildren<Chair>().ToList();
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
        Chair chair = chairs.Where(c => c.IsCustomerSat && c.CustomerSat.DemandingFood.FoodName == food.FoodName)
            .OrderBy(c => c.CustomerSat.RemainingTimeBeforeLeave).FirstOrDefault(); //if there are more customers with same food, return who has lowest timer

        //customer leave satisfied
        if (chair)
        {
            chair.CustomerSat.Leave(ECustomerSatisfaction.Satisfied);
        }
        //else, if there aren't customers with this food, dirty the table
        else
        {
            DirtyTable();
        }
    }

    /// <summary>
    /// set this table dirty
    /// </summary>
    public void DirtyTable()
    {
        //everyone leave
        foreach (var c in chairs)
        {
            if (c.IsCustomerSat)
                c.CustomerSat.Leave(ECustomerSatisfaction.Unsatisfied);     //unsatisfied if already sat
            else if (c.IsAvailable == false)
                c.CustomerSat.Leave(ECustomerSatisfaction.Indifferent);     //else, indifferent (don't lose points)
        }

        //isDirty = true;

        OnDirtyTable?.Invoke();
    }

    /// <summary>
    /// Set table no more dirty
    /// </summary>
    public void CleanTable()
    {
        isDirty = false;
    }
}
