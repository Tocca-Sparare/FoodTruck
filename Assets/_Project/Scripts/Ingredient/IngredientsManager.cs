using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IngredientsManager : MonoBehaviour
{
    public List<Ingredient> ingredients = new List<Ingredient>();

    public Ingredient GetRandomIngredient()
    {
        int toSkip = Random.Range(0, ingredients.Count);
        return ingredients.Skip(toSkip).Take(1).First();
    }
}
