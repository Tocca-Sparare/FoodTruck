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

    public bool IsAvailable => chairs.Any(c => c.IsAvailable);


    void Awake()
    {
        chairs = GetComponentsInChildren<Chair>().ToList();
    }

    public Chair GetRandomAvailableChair()
    {
        //find empty chairs
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

        if (chair)
        {
            chair.CustomerSat.Leave(true);
        }
        else
        {
            Debug.Log("TODO - set table dirty?");
        }
    }
}
