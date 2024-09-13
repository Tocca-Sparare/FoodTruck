using UnityEngine;

public class GameSceneControllerOnline : MonoBehaviour
{
    private void Awake()
    {
        //if we are online, but we aren't the server
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
        {
            //deactive every statemachine (players and customers)
            BasicStateMachine[] stateMachines = FindObjectsByType<BasicStateMachine>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var sm in stateMachines)
            {
                sm.enabled = false;
            }
        }
    }
}
