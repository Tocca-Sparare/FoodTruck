using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool IsEmpty => customerSat == null;

    public Customer customerSat { get; set; }

    public void CustomerSit(Customer customer)
    {
        customerSat = customer;
    }

    public void CustomerStandUp()
    {
        customerSat = null;
    }
}
