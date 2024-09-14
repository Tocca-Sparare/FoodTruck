using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    void Start()
    {
        EnableStateMachines();
        EnableUnlockedLevels();
    }

    void EnableStateMachines()
    {
        //set every statemachine to NormalState
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
            playerSM.SetState(playerSM.NormalState);
    }

    void EnableUnlockedLevels()
    {
        //check saved points and unlock levels
        Debug.Log("DOBBIAMO SALVARE IL PUNTEGGIO QUANDO FINIAMO UN LIVELLO E VEDERE QUALE LIVELLI SONO SBLOCCATI");
    }

    /// <summary>
    /// This is called by UsePlayerInputManagerInEditor, to test rapidly the scene with more players
    /// </summary>
    public void AddPlayerInEditorForTestLocalMultiplayer()
    {
        //just be sure to have every statemachine to NormalState
        PlayerStateMachine[] playerStateMachines = FindObjectsOfType<PlayerStateMachine>();
        foreach (var playerSM in playerStateMachines)
        {
            if (playerSM.CurrentState == null)
                playerSM.SetState(playerSM.NormalState);
        }
    }
}
