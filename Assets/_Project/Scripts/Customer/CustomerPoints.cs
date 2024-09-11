using UnityEngine;

/// <summary>
/// Give or Remove points when customer is satisfied or unsatisfied
/// </summary>
public class CustomerPoints : MonoBehaviour
{
    [SerializeField] int pointsToRemoveWhenUnsatisfied;
    [SerializeField] int[] pointsOnSatisfied;

    Customer customer;
    PointsManager pointsManager;

    private void Awake()
    {
        //get refs
        pointsManager = FindObjectOfType<PointsManager>();
        if (customer == null && TryGetComponent(out customer) == false)
            Debug.LogError($"Missing customer on {name}", gameObject);

        //add events
        if (customer)
        {
            if (customer.HungryChangeSteps.Length + 1 != pointsOnSatisfied.Length)
                Debug.LogError($"Different count of hanger steps and customer points steps", gameObject);

            customer.OnSatisfied += OnSatisfied;
            customer.OnUnsatisfied += OnUnsatisfied;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (customer)
        {
            customer.OnSatisfied -= OnSatisfied;
            customer.OnUnsatisfied -= OnUnsatisfied;
        }
    }

    private void OnSatisfied()
    {

        if (pointsManager)
            pointsManager.AddPoints(pointsOnSatisfied[customer.HungerLevel]);
    }

    private void OnUnsatisfied()
    {
        //remove points when leave unsatisfied
        if (pointsManager)
            pointsManager.RemovePoints(pointsToRemoveWhenUnsatisfied);
    }
}
