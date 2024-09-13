using Fusion;
using UnityEngine;

/// <summary>
/// This is attached to PlayerController with User. This is used online to get name and index
/// </summary>
[RequireComponent(typeof(User))]
public class UserOnline : NetworkBehaviour
{
    User user;

    [Networked, OnChangedRender(nameof(UsernameChanged))]
    public string PlayerName { get; set; }

    public int PlayerIndex { get { if (user == null) user = GetComponent<User>(); return user.PlayerIndex; } }

    /// <summary>
    /// When spawned, get player name and send to server
    /// </summary>
    public override void Spawned()
    {
        NetworkManager.instance.OnPlayerEnter?.Invoke(this);

        if (Object.HasInputAuthority)
        {
            RPC_SendName(NetworkManager.instance.LocalPlayerName);
        }
    }

    /// <summary>
    /// When server receive name for this player controller, set it. The variable is syncronized with every client
    /// </summary>
    /// <param name="playerName"></param>
    /// <param name="info"></param>
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SendName(string playerName, RpcInfo info = default)
    {
        PlayerName = playerName;
    }

    /// <summary>
    /// When server change name, everyone receive it because it's syncronized
    /// </summary>
    private void UsernameChanged()
    {
        NetworkManager.instance.OnPlayerRefreshName?.Invoke(this);
    }

    /// <summary>
    /// Call player despawned
    /// </summary>
    /// <param name="runner"></param>
    /// <param name="hasState"></param>
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        NetworkManager.instance.OnPlayerExit?.Invoke(this);
    }
}
