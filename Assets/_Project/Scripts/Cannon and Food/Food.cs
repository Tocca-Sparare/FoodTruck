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
    [SerializeField] float lifeTime = 10;

    Rigidbody rb;
    float lifeTimeTimer;
    bool isDestroyed;

    private void Awake()
    {
        //get components
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogError($"Missing rigidbody on {GetType().Name}", gameObject);

        //reset values
        lifeTimeTimer = Time.time + lifeTime;
        isDestroyed = false;
    }

    private void FixedUpdate()
    {
        //move
        rb.velocity = transform.forward * speed;

        //destroy after few seconds
        if (Time.time > lifeTimeTimer)
        {
            DestroyBullet();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //do nothing if already destroyed
        if (isDestroyed)
            return;

        //destroy bullet if hit layer
        if (ContainsLayer(hittableLayers.Layer, other.gameObject.layer))
        {
            //check if hit table
            Table table = other.GetComponentInParent<Table>();
            if (table)
                table.OnHitTable(this);
            else
                Debug.Log("If bullet doesn't hit table, set floor dirty?");

            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        if (isDestroyed == false)
        {
            isDestroyed = true;
            InstantiateHelper.Destroy(gameObject);
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
