using UnityEngine;

/// <summary>
/// Food demanded by Customer and shooted by Cannon
/// </summary>
public class Food : CannonBullet
{
    [Space]
    public string FoodName;
    public Material material;
    public Sprite icon;

    protected override void OnHitCorrectLayer(Collider other)
    {
        //check if hit table
        Table table = other.GetComponentInParent<Table>();
        if (table)
            table.OnHitTable(this);
        else
            Debug.Log("If bullet doesn't hit table, set floor dirty?");
    }
}
