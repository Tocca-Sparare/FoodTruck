using UnityEngine;

public class CannonFeedback : MonoBehaviour
{
    [SerializeField] Transform objectToRotate;
    CannonInteractable cannon;

    private void Awake()
    {
        cannon = GetComponent<CannonInteractable>();
        if (cannon == null)
            Debug.LogError($"Missing cannon on {name}", gameObject);
        if (objectToRotate == null)
            Debug.LogError($"Missing objectToRotate on {name}", gameObject);

        //add events
        if (cannon)
        {
            cannon.onUpdateAimDirection += OnUpdateAimDirection;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (cannon)
        {
            cannon.onUpdateAimDirection -= OnUpdateAimDirection;
        }
    }

    void OnUpdateAimDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            rotation.eulerAngles = new Vector3(0, rotation.eulerAngles.y, 0);
            objectToRotate.rotation = rotation;
        }
    }
}
