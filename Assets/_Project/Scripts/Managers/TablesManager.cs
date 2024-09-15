using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keep list of every table in scene and return available tables
/// </summary>
public class TablesManager : MonoBehaviour
{
    [SerializeField] int waitingTime;
    [SerializeField]
    [Range(0, 100)]
    int[] warningDelays;

    private List<Table> tables = new();

    public int WaitingTime => waitingTime;
    public int[] WarningDelays => warningDelays;

    void Awake()
    {
        tables = FindObjectsOfType<Table>().ToList();
    }

    public Table GetRandomEmptyTable()
    {
        //find available tables
        var validTables = tables.Where(t => t.IsAvailable).ToList();
        if (validTables.Count == 0)
            return null;

        //return random one
        int randomIndex = Random.Range(0, validTables.Count);
        return validTables[randomIndex];
    }

    /// <summary>
    /// Update settings by code
    /// </summary>
    /// <param name="waitingTime"></param>
    /// <param name="warningDelays"></param>
    public void SetTablesSettings(int waitingTime, int[] warningDelays)
    {
        this.waitingTime = waitingTime;
        this.warningDelays = warningDelays;
    }
}
