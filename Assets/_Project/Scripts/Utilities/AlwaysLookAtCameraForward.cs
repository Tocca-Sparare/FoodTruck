using UnityEngine;

public class AlwaysLookAtCameraForward : MonoBehaviour
{
    Camera mainCamera;
    Quaternion startRotation;

    void Awake()
    {
        mainCamera = Camera.main;
        startRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Vector3 dir = -mainCamera.transform.forward;
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
        Quaternion finalRotation = Quaternion.AngleAxis(rotation.eulerAngles.y, Vector3.up) * startRotation;
        transform.rotation = finalRotation;
    }
}
