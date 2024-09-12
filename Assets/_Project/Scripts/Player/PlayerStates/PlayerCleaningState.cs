using UnityEngine;

[System.Serializable]
public class PlayerCleaningState : State
{

    PlayerPawn player;
    InputManager inputManager;
    PlayerStateMachine playerStateMachine;
    TableInteractable tableInteractable;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerPawn on {GetType().Name}", StateMachine);
        if (player.CurrentController == null || player.CurrentController.TryGetComponent(out inputManager) == false)
            Debug.LogError($"Missing inputManager on {GetType().Name}", StateMachine);
        playerStateMachine = GetStateMachine<PlayerStateMachine>();
        if (playerStateMachine == null) Debug.LogError($"Statemachine isn't PlayerStateMachine on {GetType().Name}", StateMachine);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        tableInteractable = playerStateMachine.Table;
        Debug.Log($"inputManager= {inputManager == null} tableInteractable = { tableInteractable == null}");
        if (inputManager == null || tableInteractable == null)
            return;

        if (inputManager.InteractIsPressed)
            tableInteractable.Clean(Time.deltaTime);
        else
        {
            playerStateMachine.SetState(playerStateMachine.NormalState);
            tableInteractable.Dismiss();
        }
    }
}
