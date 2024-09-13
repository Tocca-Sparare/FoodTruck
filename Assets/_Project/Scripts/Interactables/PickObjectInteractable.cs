using redd096.Attributes;
using UnityEngine;

/// <summary>
/// When player interact with this, instantiate an object and pick in the hand
/// </summary>
public class PickObjectInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] TransportableObject transportableObject;

    [Button]
    void ShowObject()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = transportableObject.BulletPrefab.icon;
    }

    public void Interact(InteractComponent interactor)
    {
        //instantiate object and makes player pick it
        var obj = InstantiateHelper.Instantiate(transportableObject, transform.position, transform.rotation);
        obj.Pick(interactor);
    }
}
