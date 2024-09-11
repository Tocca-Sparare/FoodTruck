using UnityEngine;

/// <summary>
/// This is the manager to know how much points the players did in this level
/// </summary>
public class PointsManager : MonoBehaviour
{
    private int currentPoints;
    public int CurrentPoints => currentPoints;

    public System.Action<int> OnAddPoints;
    public System.Action<int> OnRemovePoints;

    /// <summary>
    /// Add to current points
    /// </summary>
    /// <param name="points"></param>
    public void AddPoints(int points)
    {
        currentPoints += points;
        OnAddPoints?.Invoke(currentPoints);
    }

    /// <summary>
    /// Remove from current points
    /// </summary>
    /// <param name="points"></param>
    public void RemovePoints(int points)
    {
        currentPoints -= points;
        OnRemovePoints?.Invoke(currentPoints);
    }
}
