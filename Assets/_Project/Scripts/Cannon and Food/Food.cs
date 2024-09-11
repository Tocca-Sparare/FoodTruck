using redd096;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] LayerMask hittableLayers = -1;

    public string FoodName;
    public Material material;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError($"Missing rigidbody on {GetType().Name}", gameObject);
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //check if hit table
        Table table = other.GetComponentInParent<Table>();
        if (table)
        {
            table.OnHitTable(this);
        }

        //destroy bullet if hit layer
        if (hittableLayers.ContainsLayer(other.gameObject.layer))
        {
            InstantiateHelper.Destroy(gameObject);
        }
    }
}
