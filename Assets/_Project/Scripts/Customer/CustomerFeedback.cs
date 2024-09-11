using System.Collections;
using UnityEngine;

public class CustomerFeedback : MonoBehaviour
{
    [SerializeField] AudioClip angrySound;
    [SerializeField] AudioClip hungrySound;
    [SerializeField] AudioClip burpSound;

    Customer customer;
    Animator animator;
    AudioSource audioSource;


    void Awake()
    {
        customer = GetComponent<Customer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();

        customer.OnSit += OnSit;
        customer.OnStandUp += OnStandUp;
        customer.OnSatisfied += OnSatisfied;
    }

    void OnStandUp()
    {
        animator.SetBool("IsSitting", false);
    }

    void OnSit()
    {
        animator.SetBool("IsSitting", true);
        StartCoroutine(DoHungrySound());
    }

    void OnSatisfied()
    {
        StartCoroutine(DoBurpSound());
    }

    void OnDestroy()
    {
        customer.OnSit -= OnSit;
        customer.OnStandUp -= OnStandUp;
        customer.OnSatisfied -= OnSatisfied;
    }

    IEnumerator DoHungrySound()
    {
        float randomDelay = Random.Range(2, 4);
        yield return new WaitForSecondsRealtime(randomDelay);

        float randomPitch = Random.Range(0.8f, 1.2f);
        float randomStereoPan = Random.Range(-1f, 1f);

        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;
        audioSource.clip = hungrySound;
        audioSource.Play();
    }

    IEnumerator DoAngrySound()
    {
        float randomDelay = Random.Range(1, 2);
        yield return new WaitForSecondsRealtime(randomDelay);

        float randomPitch = Random.Range(0.8f, 1.2f);
        float randomStereoPan = Random.Range(-1f, 1f);

        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;
        audioSource.clip = angrySound;
        audioSource.Play();
    }


    IEnumerator DoBurpSound()
    {
        float randomDelay = Random.Range(0.1f, 1);
        yield return new WaitForSecondsRealtime(randomDelay);

        audioSource.clip = burpSound;
        audioSource.Play();
    }
}
