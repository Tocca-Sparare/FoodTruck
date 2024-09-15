using UnityEngine;
using PathCreation;
using TMPro;
using System.Collections;

public class ObstacleCar : MonoBehaviour
{
    [SerializeField] float speed = 20;
    [SerializeField] float stoppingDuration;

    [Header("Offsets")]
    [SerializeField] Vector3 offsetAngle;
    [SerializeField][Range(0, 1)] float offsetPercentage;
    [SerializeField] float verticalOffset;
    
    [Header("References")]
    [SerializeField] Transform stoppingPoint;
    [SerializeField] PathCreator currentPath;
    [SerializeField] TMP_Text offsetText;

    float distanceTravelled;
    float stoppingDistance;
    float currentSpeed;

    public System.Action<float> OnMovingToStopPosition;
    public System.Action<float> OnStop;
    public System.Action OnFinish;

    void Start()
    {
        if (currentPath != null)
        {
            currentSpeed = speed;

            // calculate offset
            distanceTravelled = currentPath.path.length * offsetPercentage;

            if (offsetText != null)
                offsetText.text = offsetPercentage.ToString();

            if (stoppingPoint != null)
                stoppingDistance = currentPath.path.GetClosestDistanceAlongPath(stoppingPoint.position);
        }
    }

    void Update()
    {
        if (currentPath != null)
        {
            distanceTravelled += currentSpeed * Time.deltaTime;
            transform.position = currentPath.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Loop) + verticalOffset * transform.up;
            transform.rotation = currentPath.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Loop) * Quaternion.Euler(offsetAngle);

            OnMovingToStopPosition?.Invoke(stoppingDistance - distanceTravelled);

            if (stoppingDistance != -1 && distanceTravelled >= stoppingDistance)
            {
                OnStopping();
            }

            if (distanceTravelled >= currentPath.path.length)
            {
                OnFinishPath();
            }
        }
    }

    void OnStopping()
    {
        currentSpeed = 0;
        stoppingDistance = -1;
        StartCoroutine(StopCorotuine());

        OnStop?.Invoke(stoppingDuration);
    }

    void OnFinishPath()
    {
        distanceTravelled = 0;
        if (stoppingPoint != null)
            stoppingDistance = currentPath.path.GetClosestDistanceAlongPath(stoppingPoint.position);

        OnFinish?.Invoke();
    }

    IEnumerator StopCorotuine()
    {
        yield return new WaitForSeconds(stoppingDuration);
        currentSpeed = speed;
    }
}
