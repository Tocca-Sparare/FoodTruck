
public class ReadMe
{
    /*
     * CustomerFeedback has "online" region with RPCs
     * LevelManager has IsOnline and IsServer checks to avoid stamp error when can't find CustomerSpawner
     * CustomerSpawner has IsOnline and IsServer checks to be sure coroutine isn't spawning on clients (teorically not necessary because it's disabled by GameSceneControllerOnline)
     * CannonFeedback has "online" region with Networked Properties and RPCs
     * CustomerNormalState use OnLateUpdate because online we OnUpdate for some reason doesn't update positions and OnFixedUpdate is laggy
     * PlayerTransportingObjectsState use OnLateUpdate because online we OnUpdate for some reason doesn't update positions and OnFixedUpdate is laggy
     * */
}
