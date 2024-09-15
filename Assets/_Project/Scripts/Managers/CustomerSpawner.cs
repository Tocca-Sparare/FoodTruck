using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every few time, spawn customer in random spawn point
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    //Time delay range between two consecutive spawn
    [SerializeField] private float minSlowDelay = 2f;
    [SerializeField] private float maxSlowDelay = 5f;
    [Space]
    [SerializeField] float minFastDelay = 2f;
    [SerializeField] float maxFastDelay = 3f;
    [Space]
    [SerializeField] float durationSlowPhase = 25f;
    [SerializeField] float totalLevelDuration = 80;
    [Space]
    [SerializeField] private Customer customerPrefab;
    [SerializeField] List<Transform> spawnPointTransforms;

    enum EState { None, SlowPhase, FastPhase, Finished }

    TablesManager tablesManager;
    FoodManager ingredientsManager;
    EState currentState;

    public System.Action OnSlowPhaseEndedCallback;

    void Awake()
    {
        tablesManager = FindObjectOfType<TablesManager>();
        ingredientsManager = FindObjectOfType<FoodManager>();
    }

    public void Init()
    {
        //start in slow phase
        currentState = EState.SlowPhase;
        StartCoroutine(TimerSlowPhaseCoroutine());
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator TimerSlowPhaseCoroutine()
    {
        yield return new WaitForSeconds(durationSlowPhase);

        //set fast phase
        currentState = EState.FastPhase;
        OnSlowPhaseEndedCallback?.Invoke();
    }

    private IEnumerator SpawnCustomer()
    {
        //continue spawn
        float timer = Time.time + totalLevelDuration;
        while (Time.time > timer)
        {
            foreach (var spawnPointTransform in spawnPointTransforms)
            {
                //spawn customers
                yield return SpawnAtPoint(spawnPointTransform);
                yield return new WaitForEndOfFrame();
            }
        }

        //finish spawn
        OnFinishSpawn();
    }

    IEnumerator SpawnAtPoint(Transform spawnPointTransform)
    {
        //find random table
        Table table = tablesManager.GetRandomEmptyTable();
        if (table != null)
        {
            //from 1 to 4 customers
            int randomCustomerCount = Random.Range(1, 5);
            table.SetApproachingCustomersCount(randomCustomerCount);

            for (int i = 0; i < randomCustomerCount; i++)
            {
                if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
                {
                    Debug.LogError("Non deve spawnare niente sui client!", gameObject);
                    yield break;
                }

                //spawn customer
                SpawnCustomer(spawnPointTransform, table);
                yield return new WaitForSeconds(.5f);
            }

            float randomDelay = currentState == EState.SlowPhase ? Random.Range(minSlowDelay, maxSlowDelay) : Random.Range(minFastDelay, maxFastDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }

    void SpawnCustomer(Transform spawnPointTransform, Table table)
    {
        var newCustomer = InstantiateHelper.Instantiate(customerPrefab, spawnPointTransform.position, spawnPointTransform.rotation);
        newCustomer.Init(ingredientsManager.GetRandomIngredient(), table, spawnPointTransform.position);
    }

    void OnFinishSpawn()
    {
        currentState = EState.Finished;
    }
}
