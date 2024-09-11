using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    float rotationSpeed;
    Vector3 rotationAxis;

    void Awake()
    {
        rotationSpeed = Random.Range(500, 1000);
        rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    void Update()
    {
        transform.Rotate(rotationAxis * (rotationSpeed * Time.deltaTime));
    }
}
