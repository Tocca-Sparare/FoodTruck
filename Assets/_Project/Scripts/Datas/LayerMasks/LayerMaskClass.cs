using redd096.Attributes;
using UnityEngine;

/// <summary>
/// Use this instead of LayerMask in inspector, to get LayerMask from data and keep order in the project
/// </summary>
[System.Serializable]
public class LayerMaskClass
{
    //inspector
    [SerializeField] LayerMaskData data;
    [Dropdown(nameof(GetNames))][SerializeField] string elementName;

    private LayerMaskData.Element element;

    /// <summary>
    /// Get element
    /// </summary>
    public LayerMaskData.Element Element => GetElement(showErrors: true);

    /// <summary>
    /// Get layer mask
    /// </summary>
    public LayerMask Layer => Element.LayerMask;

    public LayerMaskClass(LayerMaskData.Element element)
    {
        this.element = element;
    }

    /// <summary>
    /// Check if this object exists and have a element
    /// </summary>
    /// <returns></returns>
    public bool IsValid()
    {
        return this != null && GetElement(showErrors: false) != null;
    }

    /// <summary>
    /// Force refresh also if Element is already != null. 
    /// NB if data is null, this return null
    /// </summary>
    public void RefreshElement()
    {
        element = data != null ? data.GetElement(elementName) : null;
    }

    #region private API

    private LayerMaskData.Element GetElement(bool showErrors)
    {
        //update if element isn't already setted
        if (element == null || string.IsNullOrEmpty(element.Name))
        {
            //check data isn't null
            if (data != null)
                element = data.GetElement(elementName, showErrors);
            else if (showErrors)
                Debug.LogError($"Missing data on {GetType().Name}");
        }

        return element;
    }

    #endregion

    #region editor

#if UNITY_EDITOR
    string[] GetNames()
    {
        if (data == null)
            return new string[0];

        string[] s = new string[data.Elements.Length];
        for (int i = 0; i < s.Length; i++)
            s[i] = data.Elements[i].Name;

        return s;
    }
#endif

    #endregion
}