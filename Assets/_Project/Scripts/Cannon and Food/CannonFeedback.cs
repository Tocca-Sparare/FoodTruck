using Fusion;
using UnityEngine;

/// <summary>
/// Every graphic or sound feedback for CannonInteractable
/// </summary>
public class CannonFeedback : NetworkBehaviour
{
    [SerializeField] Transform objectToRotate;
    [Space]
    [SerializeField] AudioClip shootSound;
    [SerializeField] AudioClip shootWhenNoAmmo;

    CannonInteractable cannon;
    AudioSource audioSource;
    GameObject bulletToShow;

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

        //add events
        if (cannon)
        {
            cannon.OnUpdateAimDirection += OnUpdateAimDirection;
            cannon.OnShoot += OnShoot;
            cannon.OnShootButNoAmmo += OnShootButNoAmmo;
            cannon.OnInsertBullet += OnInsertBullet;
            cannon.OnRemoveBullet += OnRemoveBullet;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (cannon)
        {
            cannon.OnUpdateAimDirection -= OnUpdateAimDirection;
            cannon.OnShoot -= OnShoot;
            cannon.OnShootButNoAmmo -= OnShootButNoAmmo;
            cannon.OnInsertBullet -= OnInsertBullet;
            cannon.OnRemoveBullet -= OnRemoveBullet;
        }
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
        RPC_OnShoot();
    }

    void OnShootButNoAmmo(Vector3 bulletDirection, Quaternion bulletRotation)
    {
        RPC_OnShootButNoAmmo();
    }

    private void OnInsertBullet(CannonBullet bullet)
    {
        //show bullet to understand what is going to shoot
        bulletToShow = InstantiateHelper.Instantiate(bullet.bulletPrefabToShowInCannon, cannon.BulletContainer);
    }

    private void OnRemoveBullet()
    {
        //remove bullet
        if (bulletToShow)
            InstantiateHelper.Destroy(bulletToShow.gameObject);
    }

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnShoot(RpcInfo info = default)
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

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnShootButNoAmmo(RpcInfo info = default)
    {
        //randomize
        float randomPitch = Random.Range(0.8f, 1.2f);   //default is 1
        float randomStereoPan = Random.Range(-1f, 1f);  //default is 0
        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;

        //play sound
        audioSource.clip = shootWhenNoAmmo;
        audioSource.Play();
    }

    #endregion
}
