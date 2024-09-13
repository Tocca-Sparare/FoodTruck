using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : MonoBehaviour
{
    [SerializeField] GameObject levelBanner;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"trigger with {other.gameObject.name}", other.gameObject);
        var mapPlayer = other.GetComponentInParent<MapPlayer>();

        if (mapPlayer != null)
        {
            levelBanner.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var mapPlayer = other.GetComponentInParent<MapPlayer>();

        if (mapPlayer != null)
        {
            levelBanner.SetActive(false);
        }
    }
}
