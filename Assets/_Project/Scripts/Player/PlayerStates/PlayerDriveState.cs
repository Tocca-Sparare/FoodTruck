using System;
using UnityEngine;

[System.Serializable]
public class PlayerDriveState : State
{
    FoodTruckDriveInteractable foodTruckController;

    Camera cam;
    PlayerPawn player;
    InputManager inputManager;
    PlayerStateMachine playerStateMachine;

    protected override void OnInit()
    {
        base.OnInit();

        //get references
        if (cam == null)
            cam = Camera.main;
        if (cam == null)
            Debug.LogError($"Missing camera on {GetType().Name}", StateMachine);
        if (player == null && TryGetStateMachineUnityComponent(out player) == false)
            Debug.LogError($"Missing PlayerPawn on {GetType().Name}", StateMachine);
        if (player.CurrentController == null || player.CurrentController.TryGetComponent(out inputManager) == false)
            Debug.LogError($"Missing inputManager on {GetType().Name}", StateMachine);
        playerStateMachine = GetStateMachine<PlayerStateMachine>();
        if (playerStateMachine == null)
            Debug.LogError($"Statemachine isn't PlayerStateMachine on {GetType().Name}", StateMachine);
    }

    protected override void OnEnter()
    {
        base.OnEnter();

        foodTruckController = playerStateMachine.FoodTruckDriver;

        if (foodTruckController == null)
            Debug.LogError($"Missing foodTruckController on {GetType().Name}", StateMachine);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (inputManager == null || foodTruckController == null)
            return;

        Debug.Log($"inputManager.Move {inputManager.Move}");

        var dotDirection = Vector3.Dot(foodTruckController.transform.forward, inputManager.Move);
        Debug.Log($"dotDirection {dotDirection}");


        if (dotDirection > 0.8)
            foodTruckController.MoveForward();

        if (dotDirection < -0.8)
            foodTruckController.MoveBackward();

        if (inputManager.Move.x == 0 && inputManager.Move.y == 0)
            foodTruckController.Stop();

        //leave cannon
        if (inputManager.InteractWasPressedThisFrame)
            foodTruckController.Dismiss();
    }
}
