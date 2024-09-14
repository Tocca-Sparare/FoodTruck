using UnityEngine;
using PathCreation;
using TMPro;

public class ObstacleCar : MonoBehaviour
{
    [SerializeField] float speed = 20;
    [SerializeField] TMP_Text offsetText;
    [SerializeField] Vector3 offsetAngle;

    PathCreator currentPath;
    float distanceTravelled;
    float offset;

    public bool IsAvailable => currentPath == null;

    void Start()
    {
        if (currentPath != null)
        {
            // calculate offset
            distanceTravelled = offset;
        }
    }

    void Update()
    {
        if (currentPath != null)
        {
            distanceTravelled += speed * Time.deltaTime;
            transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop) * Quaternion.Euler(offsetAngle);

            if (distanceTravelled >= currentPath.path.length)
            {
                currentPath = null;
                gameObject.SetActive(false);
            }
        }
    }

    public void SetPath(PathCreator path)
    {
        currentPath = path;
        distanceTravelled = 0;
    }
}
