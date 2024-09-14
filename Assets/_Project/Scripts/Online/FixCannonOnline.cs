using UnityEngine;

/// <summary>
/// This is attached to CannonInteractable to update rotation in local, because it's hard to sync
/// </summary>
public class FixCannonOnline : MonoBehaviour
{
    private Transform someoneUsingCannon;
    private CannonInteractable cannon;

    private void Awake()
    {
        //disable on local and server
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
            enabled = false;

        //get refs
        if (cannon == null && TryGetComponent(out cannon) == false)
            Debug.LogError($"Missing CannonInteractable on {name}", gameObject);
    }

    private void Update()
    {
        //rotate cannon in local to forward 
        if (someoneUsingCannon && cannon)
        {
            cannon.AimInDirection(someoneUsingCannon.forward);
        }
    }

    /// <summary>
    /// Set if someone is using this cannon, to start syncronize rotation
    /// </summary>
    /// <param name="who"></param>
    public void SetWhoIsUsingCannon(Transform who)
    {
        someoneUsingCannon = who;
    }
}
