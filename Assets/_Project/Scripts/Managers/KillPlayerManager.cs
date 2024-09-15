using UnityEngine;

/// <summary>
/// This is in ManagersInScene, to manage player death
/// </summary>
public class KillPlayerManager : MonoBehaviour
{
    /// <summary>
    /// When you want to kill the player. As parameter pass the hitted collider of the player
    /// </summary>
    /// <param name="hit"></param>
    public void OnKillPlayer(GameObject hit)
    {
        //only local or server
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
            return;

        //kill player
        var player = hit.GetComponentInParent<PlayerStateMachine>();
        if (player != null)
        {
            player.KillPlayer();
        }
    }
}
