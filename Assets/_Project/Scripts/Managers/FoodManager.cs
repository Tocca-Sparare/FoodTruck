using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager in scene with a list of every possible ingredient
/// </summary>
public class FoodManager : MonoBehaviour
{
    public List<Food> foods = new List<Food>();

    /// <summary>
    /// Get random food from the list
    /// </summary>
    /// <returns></returns>
    public Food GetRandomIngredient()
    {
        if (foods.Count == 0)
        {
            Debug.LogError($"There aren't foods in {name}", gameObject);
            return null;
        }

        int randomIndex = Random.Range(0, foods.Count);
        return foods[randomIndex];
        //return foods.Skip(randomIndex).Take(1).First();
    }
}
