using UnityEngine;

/// <summary>
/// This is attached to CannonBullet prefab, to turn off colliders on clients and keep only on server
/// </summary>
public class CannonBulletOnline : MonoBehaviour
{
    private void Awake()
    {
        //online but not server, disable colliders
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
        {
            foreach (var collider in GetComponentsInChildren<Collider>())
                collider.enabled = false;
        }
    }
}
