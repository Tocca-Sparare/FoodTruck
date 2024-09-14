using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstaclesManager : MonoBehaviour
{
    [SerializeField] List<PathCreator> paths = new List<PathCreator>();

    [SerializeField] List<ObstacleCar> carPrefabs = new List<ObstacleCar>();
    [SerializeField] float spawnDelay;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);

            int randomPrefabIndex = Random.Range(0, carPrefabs.Count);
            var obstaclePrefab = carPrefabs[randomPrefabIndex];

            foreach (var path in paths)
            {
                InstantiateHelper.Instantiate(obstaclePrefab, path.gameObject.transform);
            }
        }
    }
}
