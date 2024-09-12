using UnityEngine;

public class TablePoints : MonoBehaviour
{
    [SerializeField] int pointsToRemoveWhenUnsatisfied;
    [SerializeField] int[] pointsOnSatisfied;

    Table table;
    PointsManager pointsManager;

    private void Awake()
    {
        //get refs
        pointsManager = FindObjectOfType<PointsManager>();
        if (table == null && TryGetComponent(out table) == false)
            Debug.LogError($"Missing table on {name}", gameObject);

        //add events
        if (table)
        {
            table.OnOrderSatisfied += OnOrderSatisfied;
            table.OnOrderNotSatisfied += OnOrderNotSatistied;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (table)
        {
            table.OnOrderSatisfied -= OnOrderSatisfied;
            table.OnOrderNotSatisfied -= OnOrderNotSatistied;
        }
    }

    private void OnOrderSatisfied()
    {
        if (pointsManager)
            pointsManager.AddPoints(pointsOnSatisfied[table.HungerLevel]);
    }

    private void OnOrderNotSatistied()
    {
        //remove points when leave unsatisfied
        if (pointsManager)
            pointsManager.RemovePoints(pointsToRemoveWhenUnsatisfied);
    }
}
