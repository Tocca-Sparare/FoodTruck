using UnityEngine;

/// <summary>
/// Rotate object in input direction
/// </summary>
public class RotateCharacterFeedback : MonoBehaviour
{
    [SerializeField] MovementComponent movementComponent;
    [SerializeField] Transform objectToRotate;

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
        //rotate in direction
        if (movementComponent && objectToRotate)
        {
            if (movementComponent.MoveDirectionInput != Vector3.zero)
                objectToRotate.rotation = Quaternion.LookRotation(movementComponent.MoveDirectionInput, Vector3.up);
        }
    }
}
