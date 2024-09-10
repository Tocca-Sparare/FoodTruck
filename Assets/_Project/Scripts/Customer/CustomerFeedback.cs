using System.Collections;
using System.Linq;
using UnityEngine;

public class CustomerFeedback : MonoBehaviour
{
    [SerializeField] AudioClip angrySound;
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

        StartCoroutine(DoAngrySound());
    }

    private void OnStandUp()
    {
        animator.SetBool("IsSitting", false);
        StartCoroutine(DoBurpSound());
    }

    private void OnSit()
    {
        animator.SetBool("IsSitting", true);
    }

    void OnDestroy()
    {
        customer.OnSit -= OnSit;
        customer.OnStandUp -= OnStandUp;
    }

    private IEnumerator DoAngrySound()
    {
        int randomDelay = Random.Range(2, 4);
        yield return new WaitForSecondsRealtime(randomDelay);

        float randomPitch = Random.Range(0.8f, 1.2f);
        float randomStereoPan = Random.Range(-1f, 1f);

        audioSource.panStereo = randomStereoPan;
        audioSource.pitch = randomPitch;
        audioSource.clip = angrySound;
        audioSource.Play();
    }

    private IEnumerator DoBurpSound()
    {
        float randomDelay = Random.Range(0.1f, 1);
        yield return new WaitForSecondsRealtime(randomDelay);

        audioSource.clip = burpSound;
        audioSource.Play();
    }
}
