using UnityEngine;
using PathCreation;
using TMPro;

public class ObstacleCar : MonoBehaviour
{
    [SerializeField] float speed = 20;
    [SerializeField][Range(0, 1)] float offsetPercentage = 0;
    [SerializeField] TMP_Text offsetText;
    [SerializeField] Vector3 offsetAngle;

    PathCreator pathCreator;
    float distanceTravelled;
    float offset;

    void Start()
    {
        pathCreator = GetComponentInParent<PathCreator>();
        if (pathCreator != null)
        {
            // calculate offset
            offset = pathCreator.path.length * offsetPercentage;
            distanceTravelled = offset;
        }
    }


    void Update()
    {
        if (pathCreator != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop) * Quaternion.Euler(offsetAngle);
            
            if (distanceTravelled >= pathCreator.path.length)
                Destroy(gameObject);
        }
    }
}
