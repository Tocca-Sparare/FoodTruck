using redd096.Attributes;
using UnityEngine;

/// <summary>
/// Every object a player can pick and transport in scene
/// </summary>
public class TransportableObject : MonoBehaviour, IInteractable
{
    [SerializeField] Food bulletPrefab;

    InteractComponent playerTransportingThisObject;
    Collider[] colliders;
    Rigidbody rb;
    bool wasThrowed;

    public Food BulletPrefab => bulletPrefab;

    [Button]
    void SetMaterials()
    {
        foreach (var rend in GetComponentsInChildren<Renderer>())
            rend.material = bulletPrefab.material;
    }

    void GetColliders()
    {
        if (colliders == null)
            colliders = GetComponentsInChildren<Collider>(true);
    }

    /// <summary>
    /// Call this when player pick object
    /// </summary>
    public void Pick(InteractComponent interactor)
    {
        wasThrowed = false;

        playerTransportingThisObject = interactor;

        //be sure to NOT have rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb)
            Destroy(rb);

        //and deactive colliders
        GetColliders();
        foreach (var col in colliders)
            col.enabled = false;

        //and set player in transportingObject state
        PlayerStateMachine playerStateMachine = playerTransportingThisObject.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
        {
            playerStateMachine.SetTrasportingObject(this);
            playerStateMachine.SetState(new PlayerTransitionState(playerStateMachine.TrasnportingObjectsState));
        }
    }

    /// <summary>
    /// Call this when player drop object
    /// </summary>
    public void Drop()
    {
        //be sure to have rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.drag = 0.1f;
        }

        //and reactive colliders
        GetColliders();
        foreach (var col in colliders)
            col.enabled = true;

        //reset player state
        PlayerStateMachine playerStateMachine = playerTransportingThisObject.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
            playerStateMachine.SetState(playerStateMachine.NormalState);

        playerTransportingThisObject = null;
    }

    /// <summary>
    /// Pick on interact
    /// </summary>
    /// <param name="interactor"></param>
    public void Interact(InteractComponent interactor)
    {
        Pick(interactor);
    }

    /// <summary>
    /// Can interact only when dropped
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool CanInteract(InteractComponent interactor)
    {
        return playerTransportingThisObject == null;
    }

    /// <summary>
    /// Drop object and push it
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void Throw(Vector3 direction, float force)
    {
        Drop();
        rb.AddForce(direction * force, ForceMode.VelocityChange);

        wasThrowed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if was throwed, check if first hit is cannon
        if (wasThrowed)
        {
            wasThrowed = false;

            //and insert bullet
            CannonInteractable cannon = collision.collider.GetComponentInParent<CannonInteractable>();
            if (cannon)
            {
                cannon.InsertBullet(bulletPrefab);
                InstantiateHelper.Destroy(gameObject);
            }
        }
    }
}
