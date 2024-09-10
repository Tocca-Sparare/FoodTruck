using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] Transform physicalTablePosition;
    public Vector3 PhysicalTablePosition => physicalTablePosition.position;

    private List<Chair> chairs = new();

    public bool IsAvailable => chairs.Any(c => c.IsEmpty);
    public List<Chair> EmptyChairs => chairs.Where(c => c.IsEmpty).ToList();


    void Awake()
    {
        chairs = GetComponentsInChildren<Chair>().ToList();
    }

    public Chair GetRandomEmptyChair()
    {
        var randomIndex = Random.Range(0, EmptyChairs.Count);
        if (EmptyChairs.Count() == 0)
            return null;
        return EmptyChairs[randomIndex];
    }

    /// <summary>
    /// Find a customer with this demanding food and make it stand up
    /// </summary>
    /// <param name="food"></param>
    public void OnHitTable(Food food)
    {
        Debug.Log("TODO - find customer with lower timer and same food");
        Chair chair = chairs.FirstOrDefault(x => x.customerSat && x.customerSat.DemandingFood.FoodName == food.FoodName);
        if (chair)
        {
            chair.customerSat.Exit();
        }
    }
}
