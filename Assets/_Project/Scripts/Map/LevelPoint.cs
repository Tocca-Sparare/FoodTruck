using redd096.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPoint : MonoBehaviour, IInteractable
{
    [SceneInstance][SerializeField] string levelScene;
    [SerializeField] Sprite locandinaSprite;

    [Space]
    [SerializeField] GameObject levelBanner;
    [SerializeField] SpriteRenderer locandinaSpriteRenderer;

    private void Awake()
    {
        locandinaSpriteRenderer.sprite = locandinaSprite;
    }

    public void Interact(InteractComponent interactor)
    {
        SceneManager.LoadScene(levelScene);
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
