using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public List<Food> ingredients = new List<Food>();

    public Food GetRandomIngredient()
    {
        int toSkip = Random.Range(0, ingredients.Count);
        return ingredients.Skip(toSkip).Take(1).First();
    }
}
