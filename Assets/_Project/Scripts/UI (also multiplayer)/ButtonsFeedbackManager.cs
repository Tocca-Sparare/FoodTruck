using redd096;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is in every scene, to find every ui Button and set a sound when clicked
/// </summary>
public class ButtonsFeedbackManager : MonoBehaviour
{
    [SerializeField] AudioClass buttonClickAudio;
    Button[] buttons;

    private void Awake()
    {
        buttons = FindObjectsOfType<Button>(true);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlaySound);
        }
    }

    private void OnDestroy()
    {
        if (buttons != null)
        {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveListener(PlaySound);
            }
        }
    }

    void PlaySound()
    {
        SoundManager.instance.Play(buttonClickAudio);
    }
}
