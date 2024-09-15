using redd096;
using System.Collections;
using UnityEngine;

public class ObstacleCarFeedback : MonoBehaviour
{
    [SerializeField] float distanceToPlayStopSound = 60;
    [SerializeField] float playStartSoundFewSecondsBefore = 2;
    [SerializeField] AudioClass stopSound;
    [SerializeField] AudioClass startSound;

    ObstacleCar obstacleCar;
    bool canPlayStopSound;

    private void Awake()
    {
        if (obstacleCar == null && TryGetComponent(out obstacleCar) == false)
            Debug.LogError($"Missing obstacle car on {name}", gameObject);

        //add events
        if (obstacleCar)
        {
            obstacleCar.OnMovingToStopPosition += OnMovingToStopPosition;
            obstacleCar.OnStop += OnStop;
            obstacleCar.OnFinish += OnFinish;
        }

        canPlayStopSound = true;
    }

    private void OnDestroy()
    {
        //remove events
        if (obstacleCar)
        {
            obstacleCar.OnMovingToStopPosition -= OnMovingToStopPosition;
            obstacleCar.OnStop -= OnStop;
            obstacleCar.OnFinish -= OnFinish;
        }
    }

    void OnMovingToStopPosition(float distanceToStopPosition)
    {
        //when near to stop position, play stop sound
        if (distanceToStopPosition < distanceToPlayStopSound)
        {
            //do once
            if (canPlayStopSound)
            {
                canPlayStopSound = false;
                SoundManager.instance.Play(stopSound);
            }
        }
    }

    void OnStop(float stoppingDuration)
    {
        StartCoroutine(OnStopCoroutine(stoppingDuration));
    }

    void OnFinish()
    {
        //on finish path, reset vars
        canPlayStopSound = true;
    }

    IEnumerator OnStopCoroutine(float stoppingDuration)
    {
        //wait, then play sound before move again
        yield return new WaitForSeconds(stoppingDuration - playStartSoundFewSecondsBefore);
        SoundManager.instance.Play(startSound);
    }
}
