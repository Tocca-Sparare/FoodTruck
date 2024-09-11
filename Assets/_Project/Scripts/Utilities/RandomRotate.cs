using Unity.VisualScripting;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    float rotationSpeed;
    Vector3 rotationAxis;

    void Awake()
    {
        rotationSpeed = Random.Range(500, 1000);
        rotationAxis = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
    }

    void Update()
    {
        transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
    }
}
