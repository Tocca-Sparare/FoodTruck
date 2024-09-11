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

    public Chair GetRandomEmptyChair()
    {
        //find empty chairs
        var emptyChairs = chairs.Where(c => c.IsAvailable).ToList();
        if (emptyChairs.Count == 0)
            return null;

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
        Debug.Log("TODO - find customer with lower timer and same food");
        Chair chair = chairs.FirstOrDefault(x => x.IsCustomerSat && x.CustomerSat.DemandingFood.FoodName == food.FoodName);
        if (chair)
        {
            chair.CustomerSat.Leave(true);
        }
        else
        {
            Debug.Log("TODO - sporcare il tavolo?");
        }
    }
}
