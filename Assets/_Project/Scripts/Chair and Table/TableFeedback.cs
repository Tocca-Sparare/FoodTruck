using System;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>
/// This is for every graphic and audio feedback for the table
/// </summary>
public class TableFeedback : MonoBehaviour
{
    [SerializeField] GameObject dirtyStainsContainer;
    [SerializeField] SpriteRenderer[] dirtyStainSprites;
    [SerializeField] Material defaultDirtMaterial;
    [SerializeField] LoadingBar loadingBar;

    Table table;

    private void Awake()
    {
        //get refs
        if (table == null && TryGetComponent(out table) == false)
            Debug.LogError($"Missing table on {name}", gameObject);

        //add events
        if (table)
        {
            table.OnDirtyTable += OnDirtyTable;
            table.OnUpdateClean += OnUpdateClean;
            table.OnTableClean += OnTableClean;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (table)
        {
            table.OnDirtyTable -= OnDirtyTable;
            table.OnUpdateClean -= OnUpdateClean;
            table.OnTableClean -= OnTableClean;
        }
    }

    void OnDirtyTable(Food food)
    {
        var material = food == null ? defaultDirtMaterial : food.material;

        //show dirty with food color
        foreach (var stain in dirtyStainSprites)
            stain.color = material.color;

        dirtyStainsContainer.SetActive(true);
    }

    void OnUpdateClean(float percentage)
    {
        loadingBar.Updatebar(percentage);
    }

    private void OnTableClean()
    {
        dirtyStainsContainer.SetActive(false);
    }
}
