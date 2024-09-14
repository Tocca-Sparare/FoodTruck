using PathCreation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarObstaclesManager : MonoBehaviour
{
    [SerializeField] PathCreator path;
    [SerializeField] List<ObstacleCar> cars = new List<ObstacleCar>();
    [SerializeField] float spawnDelay;

    private void Start()
    {
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
