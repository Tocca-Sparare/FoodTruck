using UnityEngine;

public class Chair : MonoBehaviour
{
    public bool IsEmpty => customerSat == null;

    public Customer customerSat;

    public void CustomerSit(Customer customer)
    {
        customerSat = customer;
    }

    public void CustomerStandUp()
    {
        customerSat = null;
    }
}
