using redd096.Attributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] Slider gameTimerSlider;

    [Header("Points")]
    [SerializeField] TMP_Text pointsText;

    [Header("End Menu")]
    [SerializeField] GameObject endMenu;
    [SerializeField] GameObject[] blackStarsInOrder;
    [SerializeField] GameObject[] yellowStarsInOrder;
    [SerializeField] Button endButton;
    [SceneInstance][SerializeField] string sceneOnEnd;

    LevelManager levelManager;
    PointsManager pointsManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null) Debug.LogError($"Missing LevelManager on {name}", gameObject);
        pointsManager = FindObjectOfType<PointsManager>();
        if (pointsManager == null) Debug.LogError($"Missing PointsManager on {name}", gameObject);

        //add events
        if (levelManager)
        {
            levelManager.OnChangeLevelState += OnChangeLevelState;
            levelManager.OnUpdateInitialCountdown += OnUpdateInitialCountdown;
            levelManager.OnUpdateGameTimer += OnUpdateGameTimer;
            levelManager.OnUpdateStars += ShowEndMenu;
        }
        if (pointsManager)
        {
            pointsManager.OnAddPoints += OnAddPoints;
            pointsManager.OnRemovePoints += OnRemovePoints;
            pointsManager.OnSetPoints += OnSetPoints;
        }
    }

    private void OnDestroy()
    {
        //remove events
        if (levelManager)
        {
            levelManager.OnChangeLevelState -= OnChangeLevelState;
            levelManager.OnUpdateInitialCountdown -= OnUpdateInitialCountdown;
            levelManager.OnUpdateGameTimer -= OnUpdateGameTimer;
            levelManager.OnUpdateStars -= ShowEndMenu;
        }
        if (pointsManager)
        {
            pointsManager.OnAddPoints -= OnAddPoints;
            pointsManager.OnRemovePoints -= OnRemovePoints;
            pointsManager.OnSetPoints -= OnSetPoints;
        }
    }

    #region level manager events

    private void OnChangeLevelState(LevelManager.ELevelState state)
    {
        //on start countdown, show timers with correct value
        if (state == LevelManager.ELevelState.InitialCountdown)
        {
            SetInitialCountdownText(Mathf.CeilToInt(levelManager.InitialCountdown));
            SetGameTimerText(Mathf.CeilToInt(levelManager.LevelDuration), Mathf.CeilToInt(levelManager.LevelDuration));
        }
        //on start playing, hide countdown
        else if (state == LevelManager.ELevelState.Playing)
        {
            StopInitialCountdown();
        }
        //on end game, show end menu
        else
        {
            ShowEndMenu();
        }
    }

    private void OnUpdateInitialCountdown(float remainingTime)
    {
        SetInitialCountdownText(Mathf.CeilToInt(remainingTime));
    }

    private void OnUpdateGameTimer(float remainingTime)
    {
        SetGameTimerText(Mathf.CeilToInt(remainingTime), Mathf.CeilToInt(levelManager.LevelDuration));
    }

    #endregion

    #region points manager events

    private void OnAddPoints(int currentPoints)
    {
        SetPointsText(currentPoints);
    }

    private void OnRemovePoints(int currentPoints)
    {
        SetPointsText(currentPoints);
    }

    private void OnSetPoints(int currentPoints)
    {
        SetPointsText(currentPoints);
    }

    #endregion

    #region ui

    /// <summary>
    /// Update initial countdown text
    /// </summary>
    /// <param name="remainingTime"></param>
    public void SetInitialCountdownText(int remainingTime)
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
    public void SetGameTimerText(int remainigTime, float totalTime)
    {
        //set timer minutes:seconds
        int minutes = remainigTime / 60;
        int seconds = remainigTime % 60;
        gameTimerText.text = $"{minutes:00}:{seconds:00}";

        gameTimerSlider.value = remainigTime / totalTime;
    }

    /// <summary>
    /// Active end menu
    /// </summary>
    public void ShowEndMenu()
    {
        //enable end button only local or server
        endButton.enabled = NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer;

        //show stars
        for (int i = 0; i < blackStarsInOrder.Length; i++)
        {
            blackStarsInOrder[i].SetActive(i >= levelManager.StarsUnlocked);
            yellowStarsInOrder[i].SetActive(i < levelManager.StarsUnlocked);
        }

        //show end menu
        endMenu.SetActive(true);
    }

    /// <summary>
    /// This is setted in inspector, to change scene when click end button
    /// </summary>
    public void OnClickEndButton()
    {
        SceneLoader.LoadScene(sceneOnEnd);
    }

    /// <summary>
    /// Update points text
    /// </summary>
    /// <param name="currentPoints"></param>
    public void SetPointsText(int currentPoints)
    {
        pointsText.text = currentPoints.ToString();
    }

    #endregion
}
