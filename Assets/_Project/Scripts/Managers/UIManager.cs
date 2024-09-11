using TMPro;
using UnityEngine;

/// <summary>
/// Manager UI in game
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Initial Countdown")]
    [SerializeField] GameObject initialCountdownContainer;
    [SerializeField] TMP_Text initialCountdownText;

    [Header("Game timer")]
    [SerializeField] TMP_Text gameTimerText;

    [Header("End Menu")]
    [SerializeField] GameObject endMenu;

    /// <summary>
    /// Update initial countdown text
    /// </summary>
    /// <param name="remainingTime"></param>
    public void UpdateInitialCountdown(int remainingTime)
    {
        initialCountdownText.text = remainingTime.ToString();
        initialCountdownContainer.SetActive(true);
    }

    /// <summary>
    /// Hide initial countdown text
    /// </summary>
    public void StopInitialCountdown()
    {
        initialCountdownContainer.SetActive(false);
    }

    /// <summary>
    /// Update timer during playing game
    /// </summary>
    /// <param name="remainigTime"></param>
    public void UpdateGameTimer(int remainigTime)
    {
        //set timer minutes:seconds
        int minutes = remainigTime / 60;
        int seconds = remainigTime % 60;
        gameTimerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowEndMenu()
    {
        endMenu.SetActive(true);
    }
}
