using Fusion;
using redd096;
using UnityEngine;

/// <summary>
/// This is for every graphic and audio feedback for the table
/// </summary>
public class TableFeedback : MonoBehaviour
{
    [SerializeField] GameObject dirtyStainsContainer;
    [SerializeField] SpriteRenderer[] dirtyStainSprites;
    [SerializeField] Material defaultDirtMaterial;
    [SerializeField] LoadingBar loadingBar;
    [SerializeField] AudioClass cleaningAudio;
    [SerializeField] AudioClass completeCleanAudio;

    Table table;
    TableInteractable tableInteractable;
    float timerCleaningAudio;

    private void Awake()
    {
        //get refs
        if (table == null && TryGetComponent(out table) == false)
            Debug.LogError($"Missing table on {name}", gameObject);

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
        var material = food == null ? defaultDirtMaterial : food.material;

        //show dirty with food color
        foreach (var stain in dirtyStainSprites)
            stain.color = material.color;

        dirtyStainsContainer.SetActive(true);
    }

    void OnUpdateClean(float percentage)
    {
        loadingBar.Updatebar(percentage);

        //play sound
        if (NetworkManager.IsOnline)
            RPC_OnUpdateClean();
        else
            CleaningAudio();
    }

    private void OnTableClean()
    {
        dirtyStainsContainer.SetActive(false);

        //play sound
        if (NetworkManager.IsOnline)
            RPC_OnTableClean();
        else
            SoundManager.instance.Play(completeCleanAudio);
    }

    void CleaningAudio()
    {
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

    #region online

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnUpdateClean(RpcInfo info = default)
    {
        CleaningAudio();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, Channel = RpcChannel.Unreliable)]
    public void RPC_OnTableClean(RpcInfo info = default)
    {
        SoundManager.instance.Play(completeCleanAudio);
    }

    #endregion
}
