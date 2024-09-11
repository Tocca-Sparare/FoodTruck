using UnityEngine;

/// <summary>
/// Food demanded by Customer and shooted by Cannon
/// </summary>
public class Food : MonoBehaviour
{
    public string FoodName;
    public Material material;

    [Header("Movement")]
    [SerializeField] float speed = 5;
    [SerializeField] LayerMaskClass hittableLayers;

    Rigidbody rb;

    private void Awake()
    {
        //get components
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError($"Missing rigidbody on {GetType().Name}", gameObject);
    }

    private void FixedUpdate()
    {
        //move
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
        if (ContainsLayer(hittableLayers.Layer, other.gameObject.layer))
        {
            InstantiateHelper.Destroy(gameObject);

            if (table == null)
                Debug.Log("If bullet doesn't hit table, set floor dirty?");
        }
    }

    /// <summary>
    /// Check if this layer is inside LayerMask
    /// </summary>
    bool ContainsLayer(LayerMask layerMask, int layerToCompare)
    {
        //if add layer to this layermask, and layermask remain equals, then layermask contains this layer
        return layerMask == (layerMask | (1 << layerToCompare));
    }
}
