using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Keep list of every table in scene and return available tables
/// </summary>
public class TablesManager : MonoBehaviour
{
    public List<Table> tables = new();

    public bool HasAvailableTables => tables.Any(t => t.IsAvailable);   //at least one table is available

    void Awake()
    {
        tables = FindObjectsOfType<Table>().ToList();
    }

    public Table GetRandomEmptyTable()
    {
        //find available tables
        var validTables = tables.Where(t => t.IsAvailable).ToList();
        if (validTables.Count == 0)
        {
            Debug.LogError($"There aren't validTables in {name}", gameObject);
            return null;
        }

        //return random one
        int randomIndex = Random.Range(0, validTables.Count);
        return validTables[randomIndex];
    }
}
