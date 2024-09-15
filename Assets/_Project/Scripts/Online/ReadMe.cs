
public class ReadMe
{
    /*
     * CustomerFeedback has "online" region with RPCs
     * CannonFeedback has "online" region with RPCs
     * TableFeedback has "online" region with RPCs
     * LevelManagerFeedback too
     * CustomerSpawnerFeedbacks too
     * LevelManager has IsOnline and IsServer checks to avoid stamp error when can't find CustomerSpawner
     * CustomerSpawner has IsOnline and IsServer checks to be sure coroutine isn't spawning on clients (teorically not necessary because it's disabled by GameSceneControllerOnline)
     * */
}
