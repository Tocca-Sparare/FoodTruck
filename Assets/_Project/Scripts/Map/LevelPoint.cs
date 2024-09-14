using redd096.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPoint :  MonoBehaviour, IInteractable
{
    [SerializeField] GameObject levelBanner;
    [SceneInstance][SerializeField] string levelScene;

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
