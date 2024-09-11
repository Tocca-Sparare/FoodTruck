using redd096.Attributes;
using UnityEngine;

/// <summary>
/// This is the cannon in scene. Player can interact to shoot food
/// </summary>
public class CannonInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Where snap the player on interact")][SerializeField] Transform playerPosition;
    [Space]
    [SerializeField] CannonBullet bulletPrefab;
    [Tooltip("Where spawn the bullet")][SerializeField] Transform bulletSpawn;
    [Tooltip("Used to check if hit table")][SerializeField] LayerMaskClass hittableLayer;
    [Space]
    [Tooltip("Limit horizontal rotation when look left")][Range(-0f, -180f)][SerializeField] float minRotationLimit = -70f;
    [Tooltip("Limit horizontal rotation when look right")][Range(0f, 180f)][SerializeField] float maxRotationLimit = 70f;
    [Tooltip("Can shoot every X seconds")][SerializeField] float fireRate = 0.2f;

    InteractComponent playerUsingThisCannon;
    Vector3 bulletSpawnOffset;
    Vector3 aimDirection;
    float fireRateTimer;

    public System.Action<Vector3> onUpdateAimDirection;
    public CannonBullet Bullet => bulletPrefab;

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 dir = transform.forward * 2;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(minRotationLimit, Vector3.up) * dir);
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(maxRotationLimit, Vector3.up) * dir);


        Gizmos.color = Color.white;
    }

    [Button]
    void UpdateMaterials()
    {
        GetComponent<CannonFeedback>().SetBulletIcon();
    }

#endif

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
            playerStateMachine.SetCannon(this);
            playerStateMachine.SetState(playerStateMachine.UsingCannonState);
        }

        //snap player position and rotation
        interactor.transform.position = playerPosition.position;
        RotateCharacterFeedback rotateCharacter = interactor.GetComponentInChildren<RotateCharacterFeedback>();
        Vector3 rotationDirection = transform.position - rotateCharacter.ObjectToRotate.position;
        rotationDirection.y = 0;
        if (rotateCharacter)
            rotateCharacter.ForceDirection(true, rotationDirection);
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
        AimInDirection(position - transform.position);
    }

    /// <summary>
    /// Set aim direction
    /// </summary>
    /// <param name="direction"></param>
    public void AimInDirection(Vector3 direction)
    {
        direction.y = 0;
        direction = direction.normalized;

        //clamp angle
        float angle = Vector3.SignedAngle(direction, transform.forward, Vector3.up);
        angle = Mathf.Clamp(angle, minRotationLimit, maxRotationLimit);

        //rotate aim direction
        aimDirection = Quaternion.AngleAxis(angle, Vector3.down) * transform.forward;

        onUpdateAimDirection?.Invoke(aimDirection);
    }

    /// <summary>
    /// Shoot a bullet in aim direction
    /// </summary>
    public void Shoot()
    {
        //can shoot only if fire rate timer is finished
        if (Time.time < fireRateTimer)
            return;

        fireRateTimer = Time.time + fireRate;

        //rotate offset
        Quaternion rotation = Quaternion.LookRotation(aimDirection, Vector3.up);
        Vector3 rotatedOffset = rotation * bulletSpawnOffset;
        Vector3 bulletSpawnPosition = transform.position + rotatedOffset;

        Vector3 pos = transform.position + aimDirection * 70;
        pos.y = 0;

        //check if hit table, set Y direction to table position
        if (Physics.Raycast(transform.position, aimDirection, out RaycastHit hit, 100, hittableLayer.Layer))
        {
            Table table = hit.transform.GetComponentInParent<Table>();
            if (table)
                pos = new Vector3(hit.point.x, table.PhysicalTablePosition.y, hit.point.z);
        }

        //instantiate bullet
        Vector3 bulletDirection = (pos - bulletSpawnPosition).normalized;
        Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection, Vector3.up);
        InstantiateHelper.Instantiate(bulletPrefab, bulletSpawnPosition, bulletRotation);
    }

    /// <summary>
    /// Set player NOT using this cannon
    /// </summary>
    public void Dismiss()
    {
        //reset player state
        PlayerStateMachine playerStateMachine = playerUsingThisCannon.GetComponent<PlayerStateMachine>();
        if (playerStateMachine)
            playerStateMachine.SetState(playerStateMachine.NormalState);

        //reset rotation
        RotateCharacterFeedback rotateCharacter = playerUsingThisCannon.GetComponentInChildren<RotateCharacterFeedback>();
        if (rotateCharacter)
            rotateCharacter.ForceDirection(false);

        playerUsingThisCannon = null;
    }
}
