using UnityEngine;

/// <summary>
/// This is attached to player prefab. Disable its statemachine to avoid call Update to read inputs, and use photon update instead.
/// </summary>
public class StateMachineOnline : MonoBehaviour, IFixedUpdateNetworkHandler
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
    /// With new Photon Fusion, this is called only on PlayerController, so we tell PlayerInputManagerOnline to call this also on pawn
    /// </summary>
    public void FixedUpdateNetwork()
    {
        if (enabled == false)
            return;

        //call update state in this function (because in Update is where we check inputs, but now we receive input from photon functions)
        if (stateMachine && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.Update();
        }
    }

    private void FixedUpdate()
    {
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
