using UnityEngine;

/// <summary>
/// This is a water bullet to clean tables
/// </summary>
public class WaterBullet : CannonBullet
{
    protected override void OnHitCorrectLayer(Collider other)
    {
        //check if hit table
        Table table = other.GetComponentInParent<Table>();
        if (table)
            table.CleanTable();
    }
}
