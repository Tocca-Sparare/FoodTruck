using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    private List<Chair> chairs = new();

    public bool IsAvaiable => chairs.All(c => c.IsEmpty);
    public List<Chair> EmptyChairs => chairs.Where(c => c.IsEmpty).ToList();


    void Awake()
    {
        chairs = GetComponentsInChildren<Chair>().ToList();
    }

    public Chair GetRandomEmptyChair()
    {
        var randomIndex = Random.Range(0, EmptyChairs.Count);
        return EmptyChairs[randomIndex];
    }

    public void Free()
    {
        foreach (var customer in GetComponentsInChildren<Customer>())
            customer.Exit();
    }
}
