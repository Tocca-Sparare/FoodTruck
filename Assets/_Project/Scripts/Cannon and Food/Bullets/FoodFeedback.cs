using redd096.Attributes;
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
        if (meshRenderer)
            meshRenderer.sharedMaterial = food.material;
    }

    [Button("SetColor")]
    void SetColorInEditor()
    {
        //change material to show food color
        if (meshRenderer)
            meshRenderer.sharedMaterial = GetComponent<Food>().material;
    }
}
