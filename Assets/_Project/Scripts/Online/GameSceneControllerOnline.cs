using UnityEngine;

/// <summary>
/// Deactive scripts if this isn't the server
/// </summary>
public class GameSceneControllerOnline : MonoBehaviour
{
    private void Awake()
    {
        //if we are online, but we aren't the server
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
        {
            //deactive every statemachine (players teorically are already disabled by StateMachineOnline, but disable also tables to be sure)
            BasicStateMachine[] stateMachines = FindObjectsByType<BasicStateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sm in stateMachines)
            {
                sm.enabled = false;
            }

            //deactive spawn manager
            var spawnManager = FindObjectOfType<CustomerSpawner>();
            if (spawnManager)
                spawnManager.gameObject.SetActive(false);
        }
    }
}
