using redd096.UIControl;
using UnityEngine;

/// <summary>
/// Hide things that don't work in WebGL
/// </summary>
public class OptionsWebGL : MonoBehaviour
{
    [Tooltip("If true, in editor, hide things like in webGL build")][SerializeField] bool testInEditor;
    [SerializeField] GameObject fullScreenToggle;

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
        fullScreenToggle.SetActive(false);

        //refresh ui
        StartCoroutine(ForceRefreshUI.RefreshLayoutGroupsImmediateAndRecursive(fullScreenToggle.transform.parent.gameObject));
    }
}
