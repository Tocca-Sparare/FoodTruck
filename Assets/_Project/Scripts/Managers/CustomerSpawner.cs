using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;
    
    //Time delay range between two consecutive spawn
    [SerializeField] private float minSlowDelay = 5f;
    [SerializeField] private float maxSlowDelay = 10f;

    [SerializeField] private float spawnFastVelocityFactor = 10f;
    [SerializeField] private float currentVelocityFactor = 1f;


    [SerializeField] private float slowPhaseTimeLimit = 10f;
    [SerializeField] private float totalLevelTime = 100f;

    [SerializeField] List<Transform> spawnPointTransforms;

    TablesManager tablesManager;
    FoodManager ingredientsManager;

    private System.Timers.Timer levelTimer;

    void Awake()
    {
        tablesManager = FindObjectOfType<TablesManager>();
        ingredientsManager = FindObjectOfType<FoodManager>();
    }

    void Start()
    {
        Init();
        StartCoroutine(SpawnCustomer());
    }

    private void Init()
    {
        currentVelocityFactor = 1.0f;

        // Create a timer with a totalLevelTime second .
        levelTimer = new System.Timers.Timer(slowPhaseTimeLimit * 1000f);
        // Hook up the Elapsed event for the timer. 
        levelTimer.Elapsed += OnSlowPhaseEnded;
        levelTimer.Enabled = true;
        levelTimer.Start();
    }

    private void OnSlowPhaseEnded(object sender, ElapsedEventArgs e)
    {
        currentVelocityFactor = 1.0f / spawnFastVelocityFactor;
        levelTimer.Stop();

        levelTimer = new System.Timers.Timer(totalLevelTime * 1000f);
        levelTimer.Elapsed -= OnSlowPhaseEnded;
        levelTimer.Elapsed += OnLevelEnded;
        levelTimer.Enabled = true;
    }

    private void OnLevelEnded(object sender, ElapsedEventArgs e)
    {
        levelTimer.Stop();
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            foreach(var spawnPointTransform in spawnPointTransforms) {

                if (tablesManager.HasAvailableTables)
                {
                    Table targetTable = tablesManager.GetRandomEmptyTable();

                    var newCustomer = Instantiate(customerPrefab, spawnPointTransform);
                    newCustomer.transform.position = spawnPointTransform.position;

                    newCustomer.Init(ingredientsManager.GetRandomIngredient(), targetTable, spawnPointTransform.position);
                    float randomDelay = currentVelocityFactor * Random.Range(minSlowDelay, maxSlowDelay);
                    yield return new WaitForSecondsRealtime(randomDelay);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
