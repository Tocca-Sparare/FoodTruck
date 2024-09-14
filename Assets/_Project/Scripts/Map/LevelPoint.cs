using redd096.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPoint : MonoBehaviour, IInteractable
{
    [SceneInstance][SerializeField] string levelScene;
    [SerializeField] Sprite locandinaSprite;
    [SerializeField] List<GameObject> emptyStars;
    [SerializeField] List<GameObject> fullStars;

    [Space]
    [SerializeField] GameObject levelBanner;
    [SerializeField] SpriteRenderer locandinaSpriteRenderer;

    private void Awake()
    {
        locandinaSpriteRenderer.sprite = locandinaSprite;
        SetFullStars(0);
    }

    public void Interact(InteractComponent interactor)
    {
        //only offline or server can call this button
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
            SceneManager.LoadScene(levelScene);
    }

    public void SetFullStars(int count)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < count)
            {
                emptyStars[i].SetActive(false);
                fullStars[i].SetActive(true);
            }
            else
            {
                emptyStars[i].SetActive(true);
                fullStars[i].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var mapPlayer = other.GetComponentInParent<MapPlayer>();

        if (mapPlayer != null)
        {
            levelBanner.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var mapPlayer = other.GetComponentInParent<MapPlayer>();

        if (mapPlayer != null)
        {
            levelBanner.SetActive(false);
        }
    }
}
