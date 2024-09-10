using UnityEngine;

/// <summary>
/// This is used just to test interactables. Put this in scene and check if player can interact with this
/// </summary>
public class TestInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] bool canInteract = true;

    public void Interact(InteractComponent interactor)
    {
        Debug.Log($"{interactor} interacted with {name}", gameObject);
    }

    public bool CanInteract(InteractComponent interactor)
    {
        return canInteract;
    }
}
