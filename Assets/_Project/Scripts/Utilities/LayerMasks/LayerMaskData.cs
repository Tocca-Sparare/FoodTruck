using UnityEngine;

/// <summary>
/// Scriptable object used to set LayerMask in inspector
/// </summary>
[CreateAssetMenu(fileName = "LayerMaskData", menuName = "Datas/LayerMask Data")]
public class LayerMaskData : ScriptableObject
{
    public Element[] Elements;

    /// <summary>
    /// Get element in array by name
    /// </summary>
    /// <param name="elementName"></param>
    /// <param name="showErrors"></param>
    /// <returns></returns>
    public Element GetElement(string elementName, bool showErrors = true)
    {
        //find element in array by name
        if (Elements != null)
        {
            foreach (var element in Elements)
                if (element.Name == elementName)
                    return element;
        }

        if (showErrors) Debug.LogError("Impossible to find: " + elementName);
        return null;
    }

    [System.Serializable]
    public class Element
    {
        public string Name;
        public LayerMask LayerMask;
    }
}
