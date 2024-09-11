using System.Collections;
using UnityEngine;

/// <summary>
/// Every graphic and sound feedback for the Customer
/// </summary>
public class CustomerFeedback : MonoBehaviour
{
    [SerializeField] SpriteRenderer requestFoodSpriteRenderer;
    [SerializeField] GameObject requestFoodHolder;
    [SerializeField] Renderer meshRenderer;
    [SerializeField] GameObject hungryIcon;
    [Space]
    [SerializeField] AudioClip hungrySound;
    [SerializeField] AudioClip burpSound;
    [SerializeField] AudioClip angrySound;

    Customer customer;
    Animator animator;
    AudioSource audioSource;


    void Awake()
    {
        //get refs
        customer = GetComponent<Customer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();

        //add events
        customer.OnInit += OnInit;
        customer.OnSit += OnSit;
        customer.OnStandUp += OnStandUp;
        customer.OnSatisfied += OnSatisfied;
        customer.OnUnsatisfied += OnUnsatisfied;
        customer.OnHungryIncreased += OnHungryIncreased;
    }

    void OnDestroy()
    {
        //remove events
        if (customer)
        {
            customer.OnInit -= OnInit;
            customer.OnSit -= OnSit;
            customer.OnStandUp -= OnStandUp;
            customer.OnSatisfied -= OnSatisfied;
            customer.OnUnsatisfied -= OnUnsatisfied;
            customer.OnHungryIncreased -= OnHungryIncreased;
        }
    }

    void OnInit()
    {
        //set color
        meshRenderer.sharedMaterial = customer.DemandingFood.material;
    }

    void OnSit()
    {
        //set sit animation and play sound
        animator.SetBool("IsSitting", true);
        StartCoroutine(DoHungrySound());

        requestFoodHolder.SetActive(true);
        requestFoodSpriteRenderer.sprite = customer.DemandingFood.icon;
    }

    void OnStandUp()
    {
        //set stand up animation
        animator.SetBool("IsSitting", false);

        requestFoodHolder.SetActive(false);
    }

    void OnSatisfied()
    {
        //play sound
        StartCoroutine(DoBurpSound());
    }

    void OnUnsatisfied()
    {
        //play sound
        StartCoroutine(DoAngrySound());
    }

    void OnHungryIncreased()
    {
        StartCoroutine(DoShowHungryIcon());
    }

    IEnumerator DoShowHungryIcon()
    {
        hungryIcon.SetActive(true);
        yield return new WaitForSeconds(1);
        hungryIcon.SetActive(false);
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
