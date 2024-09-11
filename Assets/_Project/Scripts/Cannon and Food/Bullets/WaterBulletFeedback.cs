using redd096;
using UnityEngine;

/// <summary>
/// This is the script for every graphic or audio feedback for the Water Bullet
/// </summary>
public class WaterBulletFeedback : MonoBehaviour
{
    [SerializeField] AudioClip cleanTableSound;

    WaterBullet bullet;

    private void Awake()
    {
        //get refs
        bullet = GetComponent<WaterBullet>();
        if (bullet == null) Debug.LogError($"Missing WaterBullet on {name}", gameObject);

        //add events
        if (bullet)
        {
            bullet.OnCleanTable += OnCleanTable;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (bullet)
        {
            bullet.OnCleanTable -= OnCleanTable;
        }
    }

    void OnCleanTable()
    {
        //randomize
        float randomPitch = Random.Range(0.8f, 1.2f);   //default is 1
        float randomStereoPan = Random.Range(-1f, 1f);  //default is 0

        //play sound
        SoundManager.instance.Play(new AudioClip[] { cleanTableSound }, default, pitch: randomPitch, stereoPan: randomStereoPan);
    }
}
