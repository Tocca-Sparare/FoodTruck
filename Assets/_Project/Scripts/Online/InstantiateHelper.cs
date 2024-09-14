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
                NetworkObject spawned = NetworkManager.instance.Runner.Spawn(networkObj, position, rotation);
                return GetElement<T>(spawned);
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

    /// <summary>
    /// Instantiate and set parent
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <param name="onlyLocal">Also if we are online, instantiate only in local</param>
    /// <returns></returns>
    public static T Instantiate<T>(T prefab, Transform parent, bool onlyLocal = false) where T : Object
    {
        //online
        if (onlyLocal == false && NetworkManager.IsOnline)
        {
            if (NetworkManager.instance.Runner.IsServer && GetGameObject(prefab).TryGetComponent(out NetworkObject networkObj))
            {
                NetworkObject spawned = NetworkManager.instance.Runner.Spawn(networkObj, parent.position, parent.rotation, onBeforeSpawned: (networkRunner, obj) =>
                { GetGameObject(obj).transform.SetParent(parent); });
                return GetElement<T>(spawned);
            }
            else
            {
                Debug.LogError($"Error to spawn online {prefab}. This isn't the server or this isn't a NetworkObject!");
                return null;
            }
        }

        //local
        return Object.Instantiate(prefab, parent);
    }

    public static void Destroy(GameObject objectInScene)
    {
        //online
        if (NetworkManager.IsOnline)
        {
            if (NetworkManager.instance.Runner.IsServer && objectInScene.TryGetComponent(out NetworkObject networkObj))
                NetworkManager.instance.Runner.Despawn(networkObj);
            else
                Debug.LogError($"Error to despawn online {objectInScene}. This isn't the server or this isn't a NetworkObject!");

            return;
        }

        //local
        Object.Destroy(objectInScene);
    }

    private static GameObject GetGameObject(Object obj)
    {
        if (obj is GameObject go)
            return go;
        else
            return (obj as Component).gameObject;
    }

    private static T GetElement<T>(NetworkObject networkObj) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
            return networkObj.gameObject as T;
        else
            return networkObj.GetComponent<T>();
    }
}
