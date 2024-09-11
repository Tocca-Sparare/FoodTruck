using UnityEngine;

/// <summary>
/// Every graphic or sound feedback for CannonInteractable
/// </summary>
public class CannonFeedback : MonoBehaviour
{
    [SerializeField] SpriteRenderer bulletSpriteRenderer;
    [SerializeField] Transform objectToRotate;
    [Space]
    [SerializeField] AudioClip shootSound;

    CannonInteractable cannon;
    AudioSource audioSource;

    private void Awake()
    {
        cannon = GetComponent<CannonInteractable>();
        audioSource = GetComponentInChildren<AudioSource>();
        if (cannon == null)
            Debug.LogError($"Missing cannon on {name}", gameObject);
        if (objectToRotate == null)
            Debug.LogError($"Missing objectToRotate on {name}", gameObject);
        if (audioSource == null)
            Debug.LogError($"Missing audioSource on {name}", gameObject);

        SetBulletIcon();

        //add events
        if (cannon)
        {
            cannon.OnUpdateAimDirection += OnUpdateAimDirection;
            cannon.OnShoot += OnShoot;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (cannon)
        {
            cannon.OnUpdateAimDirection -= OnUpdateAimDirection;
            cannon.OnShoot -= OnShoot;
        }
    }

    public void SetBulletIcon()
    {
        bulletSpriteRenderer.sprite = GetComponent<CannonInteractable>().Bullet.icon;
    }

    void OnUpdateAimDirection(Vector3 direction)
    {
        //set rotate direction to look where player is aiming
        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            objectToRotate.rotation = Quaternion.AngleAxis(rotation.eulerAngles.y, Vector3.up);
        }
    }

    void OnShoot()
    {
        //randomize
        float randomPitch = Random.Range(0.8f, 1.2f);   //default is 1
        float randomStereoPan = Random.Range(-1f, 1f);  //default is 0
        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;

        //play sound
        audioSource.clip = shootSound;
        audioSource.Play();
    }
}
