using Fusion;
using redd096;
using UnityEngine;

/// <summary>
/// This is for every graphic and audio feedback for the table
/// </summary>
public class TableFeedback : NetworkBehaviour
{
    [SerializeField] GameObject dirtyStainsContainer;
    [SerializeField] SpriteRenderer[] dirtyStainSprites;
    [SerializeField] Material defaultDirtMaterial;
    [SerializeField] LoadingBar loadingBar;
    [SerializeField] AudioClass cleaningAudio;
    [SerializeField] AudioClass completeCleanAudio;

    FoodManager foodManager;
    Table table;
    TableInteractable tableInteractable;
    float timerCleaningAudio;

    private void Awake()
    {
        foodManager = FindObjectOfType<FoodManager>();

        //get refs
        if (table == null && TryGetComponent(out table) == false)
            Debug.LogError($"Missing table on {name}", gameObject);
        if (foodManager == null)
            Debug.LogError($"Missing food manager on {name}", gameObject);

        //add events
        if (table)
        {
            table.OnDirtyTable += OnDirtyTable;
            table.OnUpdateClean += OnUpdateClean;
            table.OnTableClean += OnTableClean;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (table)
        {
            table.OnDirtyTable -= OnDirtyTable;
            table.OnUpdateClean -= OnUpdateClean;
            table.OnTableClean -= OnTableClean;
        }
    }

    void OnDirtyTable(Food food)
    {
        if (NetworkManager.IsOnline)
        {
            RPC_OnDirtyTable(food ? food.FoodName : null);
            return;
        }

        var material = food == null ? defaultDirtMaterial : food.material;

        //show dirty with food color
        foreach (var stain in dirtyStainSprites)
            stain.color = material.color;

        dirtyStainsContainer.SetActive(true);
    }

    void OnUpdateClean(float percentage)
    {
        if (NetworkManager.IsOnline)
        {
            RPC_OnUpdateClean(percentage);
            return;
        }

        //update loading bar
        loadingBar.Updatebar(percentage);

        if (Time.time > timerCleaningAudio)
        {
            //update timer
            AudioSource source = SoundManager.instance.Play(cleaningAudio);
            if (source && source.clip)
            {
                timerCleaningAudio = Time.time + source.clip.length;
            }

            //play sound
            SoundManager.instance.Play(cleaningAudio);
        }
    }

    private void OnTableClean()
    {
        //play sound
        if (NetworkManager.IsOnline)
            RPC_OnTableClean();
        else
        {
            dirtyStainsContainer.SetActive(false);
            SoundManager.instance.Play(completeCleanAudio);
        }
    }

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnDirtyTable(string foodName, RpcInfo info = default)
    {
        Food food = foodManager.GetFoodByName(foodName);
        var material = food == null ? defaultDirtMaterial : food.material;

        //show dirty with food color
        foreach (var stain in dirtyStainSprites)
            stain.color = material.color;

        dirtyStainsContainer.SetActive(true);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnUpdateClean(float percentage, RpcInfo info = default)
    {
        loadingBar.Updatebar(percentage);

        if (Time.time > timerCleaningAudio)
        {
            //update timer
            AudioSource source = SoundManager.instance.Play(cleaningAudio);
            if (source && source.clip)
            {
                timerCleaningAudio = Time.time + source.clip.length;
            }

            //play sound
            SoundManager.instance.Play(cleaningAudio);
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnTableClean(RpcInfo info = default)
    {
        SoundManager.instance.Play(completeCleanAudio);
        dirtyStainsContainer.SetActive(false);
    }

    #endregion
}
