using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every graphic and sound feedback for the Customer
/// </summary>
public class CustomerFeedback : NetworkBehaviour
{
    [SerializeField] SpriteRenderer requestFoodSpriteRenderer;
    [SerializeField] GameObject requestFoodHolder;
    [SerializeField] Renderer meshRenderer;
    [SerializeField] GameObject[] hungryIcons;
    [Space]
    [SerializeField] AudioClip hungrySound;
    [SerializeField] AudioClip burpSound;
    [SerializeField] AudioClip angrySound;
    [SerializeField] List<GameObject> prefabHats;
    [SerializeField] Transform hatTransform;

    FoodManager foodManager;
    Customer customer;
    Animator animator;
    AudioSource audioSource;

    void Awake()
    {
        //get refs
        foodManager = FindObjectOfType<FoodManager>();
        customer = GetComponent<Customer>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();

        InstantiateHelper.Instantiate(prefabHats[Random.Range(0, prefabHats.Count)], hatTransform, onlyLocal: true);

        //add events
        customer.OnInit += OnInit;
        customer.OnSit += OnSit;
        customer.OnStandUp += OnStandUp;
        customer.OnSatisfied += OnSatisfied;
        customer.OnUnsatisfied += OnUnsatisfied;
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
            customer.OnSatisfyRequest -= OnSatistyRequest;
        }
    }

    void OnInit()
    {
        customer.Table.OnOrderReady += OnOrderReady;
        customer.Table.OnHungerLevelIncreased += OnHungerLevelIncreased;
    }

    void OnSit()
    {
        RPC_OnSit();
    }

    void OnStandUp()
    {
        customer.Table.OnOrderReady -= OnOrderReady;
        customer.Table.OnHungerLevelIncreased -= OnHungerLevelIncreased;

        RPC_OnStandUp();
    }

    void OnSatisfied()
    {
        RPC_OnSatisfied();
    }

    void OnUnsatisfied()
    {
        RPC_OnUnsatisfied();
    }

    void OnSatistyRequest()
    {
        RPC_OnSatistyRequest();
    }

    void OnOrderReady()
    {
        RPC_OnOrderReady(customer.RequestedFood.FoodName);
    }

    private void OnHungerLevelIncreased(int currentLevel)
    {
        RPC_OnHungerLevelIncreased(currentLevel);
    }

    IEnumerator DoShowHungryIcon(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            hungryIcons[i].SetActive(true);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < hungryIcons.Length; i++)
            hungryIcons[i].SetActive(false);
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

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnSit(RpcInfo info = default)
    {
        //set sit animation and play sound
        animator.SetBool("IsSitting", true);
        StartCoroutine(DoHungrySound());
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnStandUp(RpcInfo info = default)
    {
        //set stand up animation
        animator.SetBool("IsSitting", false);

        requestFoodHolder.SetActive(false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnSatisfied(RpcInfo info = default)
    {
        //play sound
        StartCoroutine(DoBurpSound());
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnUnsatisfied(RpcInfo info = default)
    {
        //play sound
        StartCoroutine(DoAngrySound());
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnSatistyRequest(RpcInfo info = default)
    {
        requestFoodHolder.SetActive(false);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnOrderReady(string foodName, RpcInfo info = default)
    {
        requestFoodHolder.SetActive(true);
        requestFoodSpriteRenderer.sprite = foodManager.GetFoodByName(foodName).icon;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnHungerLevelIncreased(int currentLevel, RpcInfo info = default)
    {
        StartCoroutine(DoShowHungryIcon(currentLevel));
    }

    #endregion
}
