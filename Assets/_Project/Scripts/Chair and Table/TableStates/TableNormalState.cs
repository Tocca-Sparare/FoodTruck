
using UnityEngine;

public class TableNormalState : State
{
    Table table;

    protected override void OnInit()
    {
        base.OnInit();

        if (table == null && TryGetStateMachineUnityComponent(out table) == false)
            Debug.LogError($"Missing Table on {GetType().Name}", StateMachine);
    }
}
