using UnityEngine;

public class AlwaysLookAtCamera : MonoBehaviour
{
    Camera mainCamera;
    
    void Awake()
    {
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        transform.LookAt(mainCamera.transform.position);
    }
}
