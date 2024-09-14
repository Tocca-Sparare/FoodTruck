using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponentInParent<PlayerStateMachine>();

        if (player != null)
        {
            if (player.CurrentState == player.TransportingObjectsState)
                player.Drop();

            player.SetState(player.DeadState);
        }
    }
}
