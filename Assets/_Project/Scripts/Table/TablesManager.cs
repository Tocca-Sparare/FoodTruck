using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TablesManager : MonoBehaviour
{
    public List<Table> tables = new();

    public bool HasAvailableTables => tables.Any(t => t.IsAvailable);

    void Awake()
    {
        tables = FindObjectsOfType<Table>().ToList();
    }

    public Table GetRandomEmptyTable()
    {
        var validTables = tables.Where(t => t.IsAvailable).ToList();
        var randomIndex = Random.Range(0, validTables.Count());

        return validTables[randomIndex];
    }
}
