
public class ReadMe
{
    /*
     * CustomerFeedback has "online" region with RPCs
     * CannonFeedback has "online" region with RPCs
     * TableFeedback has "online" region with RPCs
     * LevelManagerFeedback too
     * CustomerSpawnerFeedbacks too
     * DeadStatePlayerFeedback too
     * LevelPoint too
     * LevelManager has IsOnline and IsServer checks to avoid stamp error when can't find CustomerSpawner
     * CustomerSpawner has IsOnline and IsServer checks to be sure coroutine isn't spawning on clients (teorically not necessary because it's disabled by GameSceneControllerOnline)
     * PauseMenuManager has IsOnline and IsServer checks to know which scene load when press Back and to set TimeScale in local
     * UIManager has IsOnline and IsServer checks to disable EndMenu button
     * PlayerPawn is NetworkBehaviour but probably for no reason
     * PlayerStateMachine has a check to teleport rigidbody on respawn
     * */
}
