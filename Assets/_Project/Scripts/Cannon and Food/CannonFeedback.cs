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

    FoodManager foodManager;
    CannonInteractable cannon;
    AudioSource audioSource;
    GameObject bulletToShow;

    private void Awake()
    {
        foodManager = FindObjectOfType<FoodManager>();
        cannon = GetComponent<CannonInteractable>();
        audioSource = GetComponentInChildren<AudioSource>();
        if (foodManager == null)
            Debug.LogError($"Missing foodManager on {name}", gameObject);
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
        //online do on every client
        if (NetworkManager.IsOnline)
        {
            RPC_OnShoot();
            return;
        }

        //randomize
        float randomPitch = Random.Range(0.8f, 1.2f);   //default is 1
        float randomStereoPan = Random.Range(-1f, 1f);  //default is 0
        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;

        //play sound
        audioSource.clip = shootSound;
        audioSource.Play();
    }

    void OnShootButNoAmmo(Vector3 bulletDirection, Quaternion bulletRotation)
    {
        //online do on every client
        if (NetworkManager.IsOnline)
        {
            RPC_OnShootButNoAmmo();
            return;
        }

        //randomize
        float randomPitch = Random.Range(0.8f, 1.2f);   //default is 1
        float randomStereoPan = Random.Range(-1f, 1f);  //default is 0
        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;

        //play sound
        audioSource.clip = shootWhenNoAmmo;
        audioSource.Play();
    }

    private void OnInsertBullet(CannonBullet bullet)
    {
        //show bullet to understand what is going to shoot
        bulletToShow = InstantiateHelper.Instantiate(bullet.bulletPrefabToShowInCannon, cannon.BulletContainer, onlyLocal: true);

        if (NetworkManager.IsOnline)
            RPC_OnInsertBullet((bullet as Food).FoodName);
    }

    private void OnRemoveBullet()
    {
        //remove bullet
        if (bulletToShow)
            InstantiateHelper.Destroy(bulletToShow, onlyLocal: true);

        if (NetworkManager.IsOnline)
            RPC_OnRemoveBullet();
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

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    public void RPC_OnInsertBullet(string foodName, RpcInfo info = default)
    {
        //show bullet to understand what is going to shoot
        InstantiateHelper.Instantiate(foodManager.GetFoodByName(foodName).bulletPrefabToShowInCannon, cannon.BulletContainer, onlyLocal: true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, InvokeLocal = false)]
    public void RPC_OnRemoveBullet(RpcInfo info = default)
    {
        //remove bullet
        if (bulletToShow)
            InstantiateHelper.Destroy(bulletToShow, onlyLocal: true);
    }

    #endregion
}
