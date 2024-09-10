using System.Collections;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private Customer customerPrefab;
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 10f;

    TablesManager tablesManager;
    IngredientsManager ingredientsManager;

    void Awake()
    {
        tablesManager = FindObjectOfType<TablesManager>();
        ingredientsManager = FindObjectOfType<IngredientsManager>();
    }

    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            int randomMembersCount = Random.Range(0, 5);

            if (tablesManager.HasAvaiableTables)
            {
                Table targetTable = tablesManager.GetRandomEmptyTable();

                for (int i = 0; i < randomMembersCount; i++)
                {
                    var newCustomer = Instantiate(customerPrefab, transform);
                    newCustomer.transform.localPosition = Vector3.zero;

                    newCustomer.Init(ingredientsManager.GetRandomIngredient(), targetTable, transform.position);

                    yield return new WaitForSecondsRealtime(1f);
                }

            }

            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSecondsRealtime(randomDelay);
        }
    }

}
