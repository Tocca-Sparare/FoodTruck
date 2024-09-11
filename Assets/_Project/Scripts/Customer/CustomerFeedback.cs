using System.Collections;
using UnityEngine;

public class CustomerFeedback : MonoBehaviour
{
    [SerializeField] Renderer meshRenderer;
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

        customer.OnInit += OnInit;
        customer.OnSit += OnSit;
        customer.OnStandUp += OnStandUp;
        customer.OnSatisfied += OnSatisfied;
    }

    void OnDestroy()
    {
        if (customer)
        {
            customer.OnInit -= OnInit;
            customer.OnSit -= OnSit;
            customer.OnStandUp -= OnStandUp;
            customer.OnSatisfied -= OnSatisfied;
        }
    }

    void OnInit()
    {
        meshRenderer.sharedMaterial = customer.DemandingFood.material;
    }

    void OnSit()
    {
        animator.SetBool("IsSitting", true);
        StartCoroutine(DoHungrySound());
    }

    void OnStandUp()
    {
        animator.SetBool("IsSitting", false);
    }

    void OnSatisfied()
    {
        StartCoroutine(DoBurpSound());
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
