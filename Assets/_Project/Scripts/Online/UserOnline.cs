using Fusion;
using UnityEngine;

/// <summary>
/// This is attached to PlayerController with User. This is used online to get name and index
/// </summary>
[RequireComponent(typeof(User))]
public class UserOnline : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(UsernameChanged))]
    public string PlayerName { get; set; }

    [Networked, OnChangedRender(nameof(IndexChanged))]
    public int PlayerIndex { get; set; }

    /// <summary>
    /// When spawned, get player name and send to server + get index from server
    /// </summary>
    public override void Spawned()
    {
        NetworkManager.instance.OnPlayerEnter?.Invoke(this);

        //client send name to server
        if (Object.HasInputAuthority)
        {
            RPC_SendName(NetworkManager.instance.LocalPlayerName);
        }
        //server set index and inform clients
        if (Object.HasStateAuthority)
        {
            PlayerIndex = GetComponent<User>().PlayerIndex;
        }
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
    /// When server change index, everyone receive it because it's syncronized
    /// </summary>
    private void IndexChanged()
    {
        NetworkManager.instance.OnPlayerRefreshIndex?.Invoke(this);
    }
}
