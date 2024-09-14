using UnityEngine;

/// <summary>
/// Use this class to call Physics.Overlap or PhysicsRaycast both on local and online
/// </summary>
public static class PhysicsHelper
{
    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance, int layer)
    {
        //online
        if (NetworkManager.IsOnline)
        {
            return NetworkManager.instance.Runner.GetPhysicsScene().Raycast(origin, direction, out hit, maxDistance, layer);
        }
        //local
        return Physics.Raycast(origin, direction, out hit, maxDistance, layer);
    }

    public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hit, float maxDistance)
    {
        //online
        if (NetworkManager.IsOnline)
        {
            return NetworkManager.instance.Runner.GetPhysicsScene().Raycast(origin, direction, out hit, maxDistance);
        }
        //local
        return Physics.Raycast(origin, direction, out hit, maxDistance);
    }

    public static int OverlapSphere(Vector3 position, float radius, Collider[] results, int layer, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        //online
        if (NetworkManager.IsOnline)
        {
            return NetworkManager.instance.Runner.GetPhysicsScene().OverlapSphere(position, radius, results, layer, queryTriggerInteraction);
        }

        //local
        return Physics.OverlapSphereNonAlloc(position, radius, results, layer, queryTriggerInteraction);
    }
}
