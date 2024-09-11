using UnityEngine;
using System.Linq;

/// <summary>
/// Give or Remove points when customer is satisfied or unsatisfied
/// </summary>
public class CustomerPoints : MonoBehaviour
{
    [SerializeField] int pointsToRemoveWhenUnsatisfied;
    [SerializeField] FPoints[] pointsOnSatisfied;

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
        //find points with this remaining time
        float percentage = (customer.RemainingTimeBeforeLeave / customer.WaitingTime) * 100;
        FPoints addPoints = pointsOnSatisfied.Where(x => percentage > x.minimumTimePercentage)
            .OrderBy(x => x.minimumTimePercentage).LastOrDefault();         //return bigger one

        //and add
        if (pointsManager)
            pointsManager.AddPoints(addPoints.points);
    }

    private void OnUnsatisfied()
    {
        //remove points when leave unsatisfied
        if (pointsManager)
            pointsManager.RemovePoints(pointsToRemoveWhenUnsatisfied);
    }

    [System.Serializable]
    public struct FPoints
    {
        [Tooltip("Give points if remaining time before leave is still bigger than this (in percentage)")][Range(0, 100)]public int minimumTimePercentage;
        public int points;
    }
}
