using Fusion;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PointsManagerOnline : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(CurrentPointsChanged))]
    public int CurrentPoints { get; set; }

    PointsManager pointsManager;

    private void Awake()
    {
        //get ref
        pointsManager = FindObjectOfType<PointsManager>();
        if (pointsManager == null)
            Debug.LogError($"Missing points manager on {name}", gameObject);

        //add events
        if (pointsManager)
        {
            pointsManager.OnAddPoints += OnAddPoints;
            pointsManager.OnRemovePoints += OnRemovePoints;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (pointsManager)
        {
            pointsManager.OnAddPoints -= OnAddPoints;
            pointsManager.OnRemovePoints -= OnRemovePoints;
        }
    }

    /// <summary>
    /// Only server update points
    /// </summary>
    /// <param name="currentPoints"></param>
    private void OnAddPoints(int currentPoints)
    {
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            CurrentPoints = currentPoints;
    }

    /// <summary>
    /// Only server update points
    /// </summary>
    /// <param name="currentPoints"></param>
    private void OnRemovePoints(int currentPoints)
    {
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer)
            CurrentPoints = currentPoints;
    }

    /// <summary>
    /// Everyone receive the new points, but only client will update PointsManager and UIManager
    /// </summary>
    private void CurrentPointsChanged()
    {
        if (NetworkManager.IsOnline && NetworkManager.instance.Runner.IsServer == false)
        {
            if (pointsManager)
                pointsManager.SetPoints(CurrentPoints);
        }
    }
}
