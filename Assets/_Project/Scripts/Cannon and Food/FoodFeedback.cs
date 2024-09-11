using UnityEngine;

/// <summary>
/// Every graphic or sound feedback of the Food prefab
/// </summary>
public class FoodFeedback : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;

    Food food;

    private void Awake()
    {
        //get refs
        food = GetComponent<Food>();

        //set color
        SetColor();
    }

    void SetColor()
    {
        //change material to show food color
        meshRenderer.sharedMaterial = food.material;
    }
}
