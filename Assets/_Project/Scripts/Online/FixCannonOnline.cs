using UnityEngine;

/// <summary>
/// This is attached to CannonInteractable to update rotation in local, because it's hard to sync
/// </summary>
public class FixCannonOnline : MonoBehaviour
{
    private Transform someoneUsingCannon;

    private void Awake()
    {
        //disable on local and server
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
            enabled = false;
    }

    private void Update()
    {
        
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
