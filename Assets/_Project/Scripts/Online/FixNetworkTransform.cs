using Fusion;
using UnityEngine;

/// <summary>
/// This is attached to every object with NetworkTransform, to disable it on server and use it only on clients
/// </summary>
public class FixNetworkTransform : MonoBehaviour
{
    private void Awake()
    {
        //if offline, or this is the server, we don't need network transform
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
        {
            NetworkTransform[] netTransforms = GetComponentsInChildren<NetworkTransform>();
            foreach (var netTransform in netTransforms)
                netTransform.enabled = false;
        }
    }
}
