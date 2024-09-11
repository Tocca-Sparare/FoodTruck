using UnityEngine;

/// <summary>
/// This is the cannon in scene. Player can interact to shoot food
/// </summary>
public class CannonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Food foodPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] LayerMask hittableLayer = -1;
    [SerializeField] string blackboardName = "Cannon Interactable";

    InteractComponent playerUsingThisCannon;
    Vector3 bulletSpawnOffset;
    Vector3 aimDirection;

    public System.Action<Vector3> onUpdateAimDirection;

    void Awake()
    {
        //save offset
        bulletSpawnOffset = Quaternion.Inverse(transform.rotation) * (bulletSpawn.position - transform.position);
    }

    /// <summary>
    /// Set player using this cannon
    /// </summary>
    /// <param name="interactor"></param>
    public void Interact(InteractComponent interactor)
    {
        playerUsingThisCannon = interactor;

        //set player using cannon state
        PlayerStateMachine playerStateMachine = interactor.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
        {
            playerStateMachine.SetBlackboardElement(blackboardName, this);
            playerStateMachine.SetState(playerStateMachine.UsingCannonState);
        }
    }

    /// <summary>
    /// Can interact if nobody is using this cannon
    /// </summary>
    /// <param name="interactor"></param>
    /// <returns></returns>
    public bool CanInteract(InteractComponent interactor)
    {
        return playerUsingThisCannon == null;
    }

    /// <summary>
    /// Set aim direction to position
    /// </summary>
    /// <param name="position"></param>
    public void AimAtPosition(Vector3 position)
    {
        aimDirection = (position - transform.position).normalized;
        onUpdateAimDirection?.Invoke(aimDirection);
    }

    /// <summary>
    /// Set aim direction
    /// </summary>
    /// <param name="direction"></param>
    public void AimInDirection(Vector3 direction)
    {
        aimDirection = direction.normalized;
        onUpdateAimDirection?.Invoke(aimDirection);
    }

    /// <summary>
    /// Shoot a bullet in aim direction
    /// </summary>
    public void Shoot()
    {
        //rotate offset
        Quaternion rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        Vector3 rotatedOffset = rotation * bulletSpawnOffset;
        Vector3 bulletSpawnPosition = transform.position + rotatedOffset;

        Vector3 pos = transform.position + aimDirection * 70;
        pos.y = 0;

        //check if hit table, set Y direction to table position
        if (Physics.Raycast(transform.position, aimDirection, out RaycastHit hit, 100, hittableLayer))
        {
            Table table = hit.transform.GetComponentInParent<Table>();
            if (table)
                pos = new Vector3(hit.point.x, table.PhysicalTablePosition.y, hit.point.z);
        }

        //instantiate bullet
        Vector3 bulletDirection = (pos - bulletSpawnPosition).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection, Vector3.up);
        InstantiateHelper.Instantiate(foodPrefab, bulletSpawnPosition, bulletRotation);
    }

    /// <summary>
    /// Set player NOT using this cannon
    /// </summary>
    public void Dismiss()
    {
        PlayerStateMachine playerStateMachine = playerUsingThisCannon.GetComponent<PlayerStateMachine>();
        playerStateMachine.SetState(playerStateMachine.NormalState);
        playerUsingThisCannon = null;
    }
}
