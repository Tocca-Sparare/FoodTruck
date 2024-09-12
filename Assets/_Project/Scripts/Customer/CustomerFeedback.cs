using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] List<GameObject> prefabHats;
    [SerializeField] Transform hatTransform;


    Customer customer;
    Animator animator;
    AudioSource audioSource;


    void Awake()
    {
        //get refs
        customer = GetComponent<Customer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();

        InstantiateHelper.Instantiate(prefabHats[Random.Range(0, prefabHats.Count)], hatTransform);

        //add events
        customer.OnInit += OnInit;
        customer.OnSit += OnSit;
        customer.OnStandUp += OnStandUp;
        customer.OnSatisfied += OnSatisfied;
        customer.OnUnsatisfied += OnUnsatisfied;
        customer.OnHungryIncreased += OnHungryIncreased;
        customer.OnSatisfyRequest += OnSatistyRequest;
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
            customer.OnSatisfyRequest -= OnSatistyRequest;
        }
    }

    void OnInit()
    {
        //set color
        meshRenderer.sharedMaterial = customer.RequestedFood.material;
        customer.Table.OnOrderReady += OnOrderReady;
    }

    void OnSit()
    {
        //set sit animation and play sound
        animator.SetBool("IsSitting", true);
        StartCoroutine(DoHungrySound());
    }

    void OnStandUp()
    {
        customer.Table.OnOrderReady -= OnOrderReady;
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
        StartCoroutine(DoAngrySound());
    }

    void OnSatistyRequest()
    {
        requestFoodHolder.SetActive(false);
    }

    void OnOrderReady()
    {
        requestFoodHolder.SetActive(true);
        requestFoodSpriteRenderer.sprite = customer.RequestedFood.icon;
    }


    IEnumerator DoShowHungryIcon()
    {
        hungryIcon.SetActive(true);
        yield return new WaitForSeconds(1);
        hungryIcon.SetActive(false);
    }

    IEnumerator DoHungrySound()
    {
        if (hungrySound == null)
            yield break;

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
        if (angrySound == null)
            yield break;

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
        if (burpSound == null)
            yield break;

        float randomDelay = Random.Range(0.1f, 1);
        yield return new WaitForSecondsRealtime(randomDelay);

        audioSource.clip = burpSound;
        audioSource.Play();
    }
}
