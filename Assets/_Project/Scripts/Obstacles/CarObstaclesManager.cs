using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarObstaclesManager : MonoBehaviour
{
    [SerializeField] float spawnDelay;
    [Space]
    [SerializeField] PathCreator path;
    [SerializeField] GameObject carsContainer;

    ObstacleCar[] cars;

    private void Start()
    {
        cars = carsContainer.GetComponentsInChildren<ObstacleCar>();
        StartCoroutine(ActivateCar());
    }

    IEnumerator ActivateCar()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            var selectableCars = cars.Where(c => c.IsAvailable).ToArray();

            if (selectableCars.Length > 0)
            {
                int randomPrefabIndex = Random.Range(0, selectableCars.Length);
                var selectedCar = selectableCars[randomPrefabIndex];
                selectedCar.SetPath(path);
                selectedCar.gameObject.SetActive(true);
            }
        }
    }
}
