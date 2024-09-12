using UnityEngine;

/// <summary>
/// A chair in scene, where customer can sit to order food
/// </summary>
public class Chair : MonoBehaviour
{
    public bool IsAvailable { get; private set; } = true;
    public bool IsCustomerSat {  get; private set; }
    public Customer CustomerSat { get; private set; }

    public System.Action OnCustomerSat;

    /// <summary>
    /// Set this chair NOT available
    /// </summary>
    /// <param name="customer"></param>
    public void CustomerSelectTargetChair(Customer customer)
    {
        CustomerSat = customer;
        IsAvailable = false;
    }

    /// <summary>
    /// Set this chair with CustomerSat
    /// </summary>
    public void CustomerSit()
    {
        IsCustomerSat = true;
        OnCustomerSat?.Invoke();
    }

    /// <summary>
    /// Set this chair available
    /// </summary>
    public void CustomerStandUp()
    {
        CustomerSat = null;
        IsAvailable = true;
        IsCustomerSat = false;
    }
}
