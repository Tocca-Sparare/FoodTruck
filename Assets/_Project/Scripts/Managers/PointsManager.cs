using UnityEngine;

/// <summary>
/// This is the manager to know how much points the players did in this level
/// </summary>
public class PointsManager : MonoBehaviour
{
    private int currentPoints;
    public int CurrentPoints => currentPoints;

    /// <summary>
    /// Add to current points
    /// </summary>
    /// <param name="points"></param>
    public void AddPoints(int points)
    {
        currentPoints += points;
        //Debug.Log($"<color=green>Add {points} = {currentPoints}</color>");
    }

    /// <summary>
    /// Remove from current points
    /// </summary>
    /// <param name="points"></param>
    public void RemovePoints(int points)
    {
        currentPoints -= points;
        //Debug.Log($"<color=red>Remove {points} = {currentPoints}</color>");
    }
}
