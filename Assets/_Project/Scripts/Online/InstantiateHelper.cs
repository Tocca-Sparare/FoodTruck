using Fusion;
using UnityEngine;

/// <summary>
/// Use this class to instantiate and Destroy both in local or online
/// </summary>
public static class InstantiateHelper
{
    public static T Instantiate<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
    {
        //online
        if (NetworkManager.IsOnline)
        {
            if (NetworkManager.instance.Runner.IsServer && GetGameObject(prefab).TryGetComponent(out NetworkObject networkObj))
            {
                return NetworkManager.instance.Runner.Spawn(networkObj, position, rotation).GetComponent<T>();
            }
            else
            {
                Debug.LogError($"Error to spawn online {prefab}. This isn't the server or this isn't a NetworkObject!");
                return null;
            }
        }

        //local
        return Object.Instantiate(prefab, position, rotation);
    }

    public static T Instantiate<T>(T prefab, Transform parent) where T : Object
    {
        return Object.Instantiate(prefab, parent);
    }

    public static void Destroy(GameObject objectInScene)
    {
        Object.Destroy(objectInScene);
    }

    private static GameObject GetGameObject(Object obj)
    {
        if (obj is GameObject go)
            return go;
        else
            return (obj as Component).gameObject;
    }
}
