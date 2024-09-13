using redd096.UIControl;
using UnityEngine;

/// <summary>
/// Hide things that don't work in WebGL
/// </summary>
public class MainMenuWebGL : MonoBehaviour
{
    [Tooltip("If true, in editor, hide things like in webGL build")][SerializeField] bool testInEditor;
    [SerializeField] GameObject onlineHint;
    [SerializeField] GameObject quitButton;

    private void Awake()
    {
#if UNITY_EDITOR
        if (testInEditor)
            HideThings();
#elif UNITY_WEBGL
        HideThings();  
#endif
    }

    void HideThings()
    {
        //hide objects
        onlineHint.SetActive(true);
        quitButton.SetActive(false);

        //refresh ui
        StartCoroutine(ForceRefreshUI.RefreshLayoutGroupsImmediateAndRecursive(onlineHint.transform.parent.gameObject));
        StartCoroutine(ForceRefreshUI.RefreshLayoutGroupsImmediateAndRecursive(quitButton.transform.parent.gameObject));
    }
}
