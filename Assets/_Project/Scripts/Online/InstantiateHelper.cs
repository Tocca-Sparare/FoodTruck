using UnityEngine;

/// <summary>
/// Use this class to instantiate and Destroy both in local or online
/// </summary>
public static class InstantiateHelper
{
    public static T Instantiate<T>(T go, Vector3 position, Quaternion rotation) where T : Object
    {
        return Object.Instantiate(go, position, rotation);
    }

    public static T Instantiate<T>(T go, Transform parent) where T : Object
    {
        return Object.Instantiate(go, parent);
    }

    public static void Destroy(GameObject go)
    {
        Object.Destroy(go);
    }
}
