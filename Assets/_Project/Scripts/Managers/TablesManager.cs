using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keep list of every table in scene and return available tables
/// </summary>
public class TablesManager : MonoBehaviour
{
    private List<Table> tables = new();

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
}
