using UnityEngine;

public class KillPlayerOnCollision : MonoBehaviour
{
    KillPlayerManager manager;

    private void OnCollisionEnter(Collision collision)
    {
        //be sure to have manager
        if (manager == null)
            manager = FindObjectOfType<KillPlayerManager>();

        //call KillPlayer
        if (manager)
            manager.OnKillPlayer(collision.gameObject);
    }
}
