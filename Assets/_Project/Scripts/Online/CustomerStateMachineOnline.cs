using Fusion;
using UnityEngine;

/// <summary>
/// This is attached to customer prefab. Disable its statemachine to avoid call update and fixed update, and use photon fixed update instead
/// </summary>
public class CustomerStateMachineOnline : NetworkBehaviour
{
    BasicStateMachine stateMachine;

    private void Awake()
    {
        //get refs
        stateMachine = GetComponent<BasicStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogError($"Missing BasicStateMachine on {name}", gameObject);
            return;
        }

        //disable statemachine online, now we use this
        if (NetworkManager.IsOnline)
        {
            stateMachine.enabled = false;

            //if client, disable also this script, everything works only on server
            if (NetworkManager.instance.Runner.IsServer == false)
            {
                enabled = false;
            }
        }
        //viceversa in local, disable this and keep local statemachine
        else
        {
            enabled = false;
        }
    }

    /// <summary>
    /// On customer we can call FixedUpdateNetwork probably because is spawned at runtime
    /// </summary>
    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (enabled == false)
            return;

        //call update state in this function (because in Update is where we check inputs, but now we receive input from photon functions)
        if (stateMachine && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.Update();
        }

        //call FixedUpdate normally
        if (stateMachine && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.FixedUpdate();
        }
    }

    private void LateUpdate()
    {
        //call LateUpdate normally
        if (stateMachine && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.LateUpdate();
        }
    }
}
