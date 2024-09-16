using redd096.Attributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] float remainingTimeForBlink = 5;
    [SerializeField] float durationBlink = 8;

    [Header("Points")]
    [SerializeField] TMP_Text pointsText;

    [Header("Storm Hint")]
    [SerializeField] GameObject stormContainer;
    [SerializeField] RectTransform transformToAnimate;
    [SerializeField] float animationDuration = 5;
    [SerializeField] float animationSpeed = 1;

    [Header("EndGame (before end menu)")]
    [SerializeField] RectTransform bombRect;
    [SerializeField] Transform bombContainerDuringAnimation;
    [SerializeField] float bombAnimationDuration = 1.5f;
    [SerializeField] float bombScale = 2;
    [SerializeField] GameObject endGameAnimationObject;
    [SerializeField] float delayBeforeOpenEndMenu = 2f;
    [SerializeField] float delayBeforeActiveEndMenuInteractable = 5f;

    [Header("End Menu")]
    [SerializeField] GameObject endMenu;
    [SerializeField] GameObject[] blackStarsInOrder;
    [SerializeField] GameObject[] yellowStarsInOrder;
    [SerializeField] Button endButton;
    [SceneInstance][SerializeField] string sceneOnEnd;

    LevelManager levelManager;
    PointsManager pointsManager;
    CustomerSpawner customerSpawner;

    Coroutine blinkCoroutine;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        if (levelManager == null) Debug.LogError($"Missing LevelManager on {name}", gameObject);
        pointsManager = FindObjectOfType<PointsManager>();
        if (pointsManager == null) Debug.LogError($"Missing PointsManager on {name}", gameObject);
        customerSpawner = FindObjectOfType<CustomerSpawner>();
        if (customerSpawner == null) Debug.LogError($"Missing customerSpawner on {name}", gameObject);

        //add events
        if (levelManager)
        {
            levelManager.OnChangeLevelState += OnChangeLevelState;
            levelManager.OnUpdateInitialCountdown += OnUpdateInitialCountdown;
            levelManager.OnUpdateGameTimer += OnUpdateGameTimer;
            levelManager.OnUpdateStars += OnUpdateStars;
        }
        if (pointsManager)
        {
            pointsManager.OnAddPoints += OnAddPoints;
            pointsManager.OnRemovePoints += OnRemovePoints;
            pointsManager.OnSetPoints += OnSetPoints;
        }
        if (customerSpawner)
        {
            customerSpawner.OnSlowPhaseEndedCallback += OnSlowPhaseEnded;
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
            levelManager.OnUpdateStars -= OnUpdateStars;
        }
        if (pointsManager)
        {
            pointsManager.OnAddPoints -= OnAddPoints;
            pointsManager.OnRemovePoints -= OnRemovePoints;
            pointsManager.OnSetPoints -= OnSetPoints;
        }
        if (customerSpawner)
        {
            customerSpawner.OnSlowPhaseEndedCallback -= OnSlowPhaseEnded;
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
            StartCoroutine(EndGameAnimationCoroutine());
        }
    }

    private void OnUpdateInitialCountdown(float remainingTime)
    {
        SetInitialCountdownText(Mathf.CeilToInt(remainingTime));
    }

    private void OnUpdateGameTimer(float remainingTime)
    {
        SetGameTimerText(Mathf.CeilToInt(remainingTime), Mathf.CeilToInt(levelManager.LevelDuration));

        //if almost finish timer, blink
        if (remainingTime < remainingTimeForBlink)
        {
            //check coroutine to start it only once
            if (blinkCoroutine == null)
                blinkCoroutine = StartCoroutine(BlinkGameTimer());
        }
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

    #region customer spawner events

    private void OnSlowPhaseEnded()
    {
        StartCoroutine(StormHintCoroutine());
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
    /// Update points text
    /// </summary>
    /// <param name="currentPoints"></param>
    public void SetPointsText(int currentPoints)
    {
        pointsText.text = currentPoints.ToString();
    }

    /// <summary>
    /// Active end menu
    /// </summary>
    public void ShowEndMenu()
    {
        //enable end button only local or server
        endButton.enabled = NetworkManager.IsOnline == false || NetworkManager.instance.Runner.IsServer;

        //show stars (on clients, this function will be call again by a level manager event)
        OnUpdateStars();

        //show end menu
        endMenu.SetActive(true);
    }

    /// <summary>
    /// Update stars in end menu
    /// </summary>
    public void OnUpdateStars()
    {
        //show stars
        for (int i = 0; i < blackStarsInOrder.Length; i++)
        {
            blackStarsInOrder[i].SetActive(i >= levelManager.StarsUnlocked);
            yellowStarsInOrder[i].SetActive(i < levelManager.StarsUnlocked);
        }
    }

    /// <summary>
    /// This is setted in inspector, to change scene when click end button
    /// </summary>
    public void OnClickEndButton()
    {
        SceneLoader.LoadScene(sceneOnEnd);
    }

    #endregion

    #region private coroutines

    /// <summary>
    /// Show text and animate hint
    /// </summary>
    /// <returns></returns>
    IEnumerator StormHintCoroutine()
    {
        stormContainer.SetActive(true);

        float duration = Time.time + animationDuration;
        bool big = true;
        float currentSize = 1f;
        while (duration > Time.time)
        {
            if (big)
                currentSize += Time.deltaTime * animationSpeed;
            else
                currentSize -= Time.deltaTime * animationSpeed;

            if ((big && currentSize > 1.3f) || (big == false && currentSize < 0.7f))
                big = !big;

            transformToAnimate.localScale = Vector3.one * currentSize;
            yield return null;
        }

        stormContainer.SetActive(false);
    }

    /// <summary>
    /// Blink animation for game timer
    /// </summary>
    /// <returns></returns>
    IEnumerator BlinkGameTimer()
    {
        float duration = Time.time + durationBlink;
        while (duration > Time.time)
        {
            gameTimerText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            gameTimerText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// Play end game animation, then open end menu
    /// </summary>
    /// <returns></returns>
    private IEnumerator EndGameAnimationCoroutine()
    {
        //disable event system during animation
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem)
            eventSystem.enabled = false;

        //change bomb parent for animation
        bombRect.SetParent(bombContainerDuringAnimation);

        float delta = 0;
        Vector3 startPosition = bombRect.localPosition;
        Vector3 endPosition = bombRect.sizeDelta;
        Vector3 startScale = bombRect.localScale;
        Vector3 endScale = startScale * bombScale;

        //play bomb animation
        while (delta < 1)
        {
            delta += Time.deltaTime / bombAnimationDuration;
            bombRect.localPosition = Vector3.Lerp(startPosition, endPosition, delta);
            bombRect.localScale = Vector3.Lerp(startScale, endScale, delta);

            yield return null;
        }

        //then start end animation
        endGameAnimationObject.SetActive(true);

        //wait before show end menu
        yield return new WaitForSeconds(delayBeforeOpenEndMenu);
        ShowEndMenu();

        //and re-enable event system
        yield return new WaitForSeconds(delayBeforeActiveEndMenuInteractable - delayBeforeOpenEndMenu);
        if (eventSystem)
            eventSystem.enabled = true;
    }

    #endregion
}
