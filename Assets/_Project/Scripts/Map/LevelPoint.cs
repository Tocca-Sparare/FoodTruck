using Cinemachine;
using Fusion;
using redd096.Attributes;
using System.Collections.Generic;
using UnityEngine;

public class LevelPoint : NetworkBehaviour, IInteractable
{
    [SceneInstance][SerializeField] string levelScene;
    [SerializeField] Sprite locandinaSprite;
    [SerializeField] List<GameObject> emptyStars;
    [SerializeField] List<GameObject> fullStars;

    [Space]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject levelBanner;
    [SerializeField] SpriteRenderer locandinaSpriteRenderer;

    private void Awake()
    {
        locandinaSpriteRenderer.sprite = locandinaSprite;
        SetFullStars(0);
    }

    public void Interact(InteractComponent interactor)
    {
        if (locandinaSprite == null)
        {
            Debug.Log($"Livello non disponibile {gameObject.name}", gameObject);
            return;
        }

        //only offline or server can call this button
        if (NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer)
        {
            SceneLoader.LoadScene(levelScene);
        }
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
            if (NetworkManager.IsOnline)
            {
                if (NetworkManager.instance.Runner.IsServer)
                    RPC_OnActivateLevelPoint();
            }
            else
            {
                virtualCamera.gameObject.SetActive(true);
                levelBanner.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var mapPlayer = other.GetComponentInParent<MapPlayer>();

        if (mapPlayer != null)
        {
            if (NetworkManager.IsOnline)
            {
                if (NetworkManager.instance.Runner.IsServer)
                    RPC_OnDeactivateLevelPoint();
            }
            else
            {
                virtualCamera.gameObject.SetActive(false);
                levelBanner.SetActive(false);
            }
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnActivateLevelPoint(RpcInfo info = default)
    {
        virtualCamera.gameObject.SetActive(true);
        levelBanner.SetActive(true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnDeactivateLevelPoint(RpcInfo info = default)
    {
        virtualCamera.gameObject.SetActive(false);
        levelBanner.SetActive(false);
    }
}
