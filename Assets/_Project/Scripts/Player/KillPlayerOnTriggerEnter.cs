using UnityEngine;

public class KillPlayerOnTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerStateMachine>();

        if (player != null)
        {
            player.SetState(player.DeadState);
        }
    }
}
