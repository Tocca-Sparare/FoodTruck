using UnityEngine;

/// <summary>
/// This is used just to write something in editor
/// </summary>
[CreateAssetMenu(fileName = "TextData", menuName = "Datas/Text Data")]
public class TextData : ScriptableObject
{
    [TextArea(20, 50)]
    public string text;
}
