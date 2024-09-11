using UnityEngine;

/// <summary>
/// Rotate object in input direction
/// </summary>
public class RotateCharacterFeedback : MonoBehaviour
{
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] Transform objectToRotate;

    public Transform ObjectToRotate => objectToRotate;

    bool isForcedDirection;

    private void Awake()
    {
        //check there are components
        if (movementComponent == null && TryGetComponent(out movementComponent) == false)
            Debug.LogError($"Missing movementComponent on {name}", gameObject);
        if (objectToRotate == null)
            Debug.LogError($"Missing objectToRotate on {name}", gameObject);
    }

    private void Update()
    {
        //do nothing if forced to look in direction
        if (isForcedDirection)
            return;

        //rotate in movement direction
        if (movementComponent && objectToRotate)
        {
            if (movementComponent.MoveDirectionInput != Vector3.zero)
                objectToRotate.rotation = Quaternion.LookRotation(movementComponent.MoveDirectionInput, Vector3.up);
        }
    }

    /// <summary>
    /// Force character to look only in this direction
    /// </summary>
    /// <param name="isForced"></param>
    /// <param name="direction"></param>
    public void ForceDirection(bool isForced, Vector3 direction = default)
    {
        isForcedDirection = isForced;

        if (direction != Vector3.zero)
            objectToRotate.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
    }
}
