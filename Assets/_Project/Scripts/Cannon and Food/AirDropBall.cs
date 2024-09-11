using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class AirDropBall : MonoBehaviour
{
    [SerializeField] float flightHeight = 10;
    [SerializeField] float flightLength = 15;
    [SerializeField] float flightTime = 2;

    float fireRateTimer;
    [SerializeField] Food foodPrefabRed;
    [SerializeField] Food foodPrefabBlue;
    [SerializeField] Food foodPrefabGreen;
    [HideInInspector] Food foodPrefabChosen;

    Coroutine coroutineHandle;
    [HideInInspector] Vector3 targetPosition;

    bool isMoving = false;
    void Start()
    {
        StartCoroutine(Shoot());
    }

    void Update() { 

    }


    public void StartRed() {
        foodPrefabChosen = foodPrefabRed;
        Setup();
    }
    public void StartBlue()
    {
        foodPrefabChosen = foodPrefabBlue;
        Setup();
    }

    public void StartGreen()
    {
        foodPrefabChosen = foodPrefabGreen;
        Setup();
    }

    public void Setup() {

        isMoving = false;

        List<Table> table = FindObjectsOfType<Table>().ToList();
        var randomTableIndex = Random.RandomRange(0, table.Count()-1);
        float xpos = Random.Range(-1.0f, 1.0f);
        float zpos = Random.Range(-1.0f, 1.0f);

        Vector3 randomPosition = flightLength * new Vector3(xpos, 0, zpos).normalized;
        Vector3 randomPositionStart = table[randomTableIndex].transform.position - randomPosition;
        Vector3 randomPositionEnd = table[randomTableIndex].transform.position + randomPosition;
        transform.position = new Vector3(randomPositionStart.x, flightHeight, randomPositionStart.z);
        targetPosition = new Vector3(randomPositionEnd.x, flightHeight, randomPositionEnd.z);
        if (coroutineHandle!=null)
            StopCoroutine(coroutineHandle);
        
        coroutineHandle = StartCoroutine(LerpPosition(targetPosition, flightTime));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        isMoving = true;
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        isMoving = false;

    }

    public IEnumerator Shoot()
    {
        while (true) {
            if (isMoving)
            {
                InstantiateHelper.Instantiate(foodPrefabChosen, transform.position + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f)), Quaternion.Euler(90, 0, 0));
                yield return new WaitForSecondsRealtime(0.01f);
            }
            yield return null;

        }

    }

}
