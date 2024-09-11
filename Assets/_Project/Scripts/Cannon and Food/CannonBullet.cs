using UnityEngine;

/// <summary>
/// This is every bullet shooted by cannon
/// </summary>
public abstract class CannonBullet : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 5;
    [SerializeField] LayerMaskClass hittableLayers;
    [SerializeField] float lifeTime = 10;

    Rigidbody rb;
    float lifeTimeTimer;
    bool isDestroyed;

    protected abstract void OnHitCorrectLayer(Collider other);

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
            OnHitCorrectLayer(other);

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
