using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Find interactables in a radius around character. And call function to interact
/// </summary>
public class InteractComponent : MonoBehaviour
{
    [Tooltip("Area to check for interactables")][SerializeField] float radiusInteract = 1f;
    [Tooltip("Hit only interacts with this layer")][SerializeField] LayerMask interactLayer = -1;
    [Tooltip("This is used if you call ScanInteractablesInDirection")][SerializeField] float maxAngle = 50f;
    [SerializeField] bool showRadiusDebug;

    //events
    public System.Action<IInteractable> onFoundInteractable;
    public System.Action<IInteractable> onLostInteractable;
    public System.Action<IInteractable> onInteract;             //when user interact with Interactable
    public System.Action onFailInteract;                        //when user try to interact but Interactable is null or CanInteract return false

    public IInteractable CurrentInteractable;

    private void OnDrawGizmosSelected()
    {
        //draw area interactable
        if (showRadiusDebug)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, radiusInteract);

            //draw max angle
            Gizmos.color = Color.red;
            Vector3 dir = transform.forward * radiusInteract;
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(maxAngle, Vector3.up) * dir);
            Gizmos.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-maxAngle, Vector3.up) * dir);

            Gizmos.color = Color.white;
        }
    }

    /// <summary>
    /// Find interactables in radius and set Current Interactable
    /// </summary>
    public void ScanInteractables()
    {
        //find nearest interactable
        var possibleInteractables = GetPossibleInteractables();
        IInteractable newInteractable = FindNearest(possibleInteractables);

        //if changed interactable, call events
        CheckChangeInteractable(newInteractable);
    }

    /// <summary>
    /// Find interactables in radius, but only if inside max angle
    /// </summary>
    /// <param name="direction"></param>
    public void ScanInteractablesInDirection(Vector3 direction)
    {
        var possibleInteractables = GetPossibleInteractables();

        //remove interactables not in angle
        direction = direction.normalized;
        foreach (Transform collider in new List<Transform>(possibleInteractables.Keys))
        {
            Vector3 dir = (collider.position - transform.position).normalized;
            if (Vector3.Angle(dir, direction) > maxAngle)
                possibleInteractables.Remove(collider);
        }

        //find nearest interactable
        IInteractable newInteractable = FindNearest(possibleInteractables);

        //if changed interactable, call events
        CheckChangeInteractable(newInteractable);
    }

    /// <summary>
    /// Interact with current interactable
    /// </summary>
    public void TryInteract()
    {
        if (CurrentInteractable != null && CurrentInteractable.CanInteract(this))
        {
            CurrentInteractable.Interact(this);
            onInteract?.Invoke(CurrentInteractable);
        }
        else
        {
            onFailInteract?.Invoke();
        }
    }

    /// <summary>
    /// Set this as current interactable and try to interact with it
    /// </summary>
    /// <param name="interactable"></param>
    public void TryInteract(IInteractable interactable)
    {
        //if changed interactable, call events
        CheckChangeInteractable(interactable);

        //interact
        TryInteract();
    }

    #region private API

    Dictionary<Transform, IInteractable> GetPossibleInteractables()
    {
        //find interactables in area
        Dictionary<Transform, IInteractable> possibleInteractables = new Dictionary<Transform, IInteractable>();
        foreach (Collider col in Physics.OverlapSphere(transform.position, radiusInteract, interactLayer))
        {
            //add to dictionary if CanInteract is true
            IInteractable interactable = col.GetComponentInParent<IInteractable>();
            if (interactable != null && interactable.CanInteract(this))
            {
                possibleInteractables.Add(col.transform, interactable);
            }
        }
        return possibleInteractables;
    }

    IInteractable FindNearest(Dictionary<Transform, IInteractable> possibleInteractables)
    {
        IInteractable nearest = null;
        float distance = Mathf.Infinity;

        //find nearest interactable
        foreach (Transform t in possibleInteractables.Keys)
        {
            float newDistance = (t.position - transform.position).sqrMagnitude;
            if (newDistance < distance)
            {
                distance = newDistance;
                nearest = possibleInteractables[t];
            }
        }

        //return interactable
        return nearest;
    }

    void CheckChangeInteractable(IInteractable newInteractable)
    {
        //if changed interactable, call events
        if (newInteractable != CurrentInteractable)
        {
            if (CurrentInteractable != null)
                onLostInteractable?.Invoke(CurrentInteractable);

            if (newInteractable != null)
                onFoundInteractable?.Invoke(newInteractable);

            //and set current interactable
            CurrentInteractable = newInteractable;
        }
    }

    #endregion
}
