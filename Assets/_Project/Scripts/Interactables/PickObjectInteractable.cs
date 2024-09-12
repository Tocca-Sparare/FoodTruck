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
        //show object 
        var instancedObj = Instantiate(transportableObject, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        GameObject go = instancedObj.gameObject;
        go.transform.SetParent(transform);

        //but remove scripts
        var scripts = go.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
            DestroyImmediate(script);

        //and colliders
        var colliders = go.GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
            DestroyImmediate(col);
    }

    public void Interact(InteractComponent interactor)
    {
        //instantiate object and makes player pick it
        var obj = InstantiateHelper.Instantiate(transportableObject, transform.position, transform.rotation);
        obj.Pick(interactor);
    }
}
