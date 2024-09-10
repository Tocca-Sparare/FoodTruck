using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    [SerializeField] AudioClip angrySound;
    [SerializeField] AudioClip burpSound;


    NavMeshAgent navMeshAgent;
    Chair targetChair;
    Animator animator;
    bool isExiting = false;
    AudioSource audioSource;
    Vector3 exitPoint;


    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponentInChildren<AudioSource>();
        StartCoroutine(DoAngrySound());
    }

    void FixedUpdate()
    {
        if (!isExiting && targetChair && Vector3.Distance(targetChair.transform.position, transform.position) < 0.3f)
            Sit();

        if (isExiting && Vector3.Distance(exitPoint, transform.position) < 0.3f)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    public void SetTargetTable(Table table)
    {
        targetChair = table.GetRandomEmptyChair();
        targetChair.IsEmpty = false;
        navMeshAgent.destination = targetChair.transform.position;
    }
    
    public void SetExitPoint(Vector3 exitPoint)
    {
        this.exitPoint = exitPoint;
    }

    public void Sit()
    {
        transform.SetParent(targetChair.transform);
        transform.SetPositionAndRotation(targetChair.transform.position, targetChair.transform.rotation);
        navMeshAgent.enabled = false;
        animator.SetBool("IsSitting", true);

        StartCoroutine(Exit());
    }

    public IEnumerator Exit()
    {
        yield return new WaitForSeconds(2);
        StartCoroutine(DoBurpSound());
        isExiting = true;
        animator.SetBool("IsSitting", false);
        navMeshAgent.enabled = true;
        navMeshAgent.destination = exitPoint;
        targetChair.IsEmpty = true;
    }

    private IEnumerator DoAngrySound()
    {
        int randomDelay = Random.Range(5, 20);
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
        int randomDelay = Random.Range(2, 6);
        yield return new WaitForSecondsRealtime(randomDelay);

        audioSource.clip = burpSound;
        audioSource.Play();
    }
}
