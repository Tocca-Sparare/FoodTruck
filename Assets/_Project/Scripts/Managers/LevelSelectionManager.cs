using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    void Start()
    {
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetState(playerSM.NormalState);
    }
   
}
