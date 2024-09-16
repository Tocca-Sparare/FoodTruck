using redd096;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manage options ui - For FullScreen probably you want to enable ResizableWindow in PlayerSettings
/// </summary>
public class OptionsUI : MonoBehaviour
{
    [SerializeField] bool autoRegisterEvents = true;
    [Space]
    [SerializeField] TMP_Text masterVolumeText;
    [SerializeField] Slider masterVolumeSlider;
    [Space]
    [SerializeField] TMP_Text musicVolumeText;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] TMP_Text sfxVolumeText;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] TMP_Text uiVolumeText;
    [SerializeField] Slider uiVolumeSlider;
    [Space]
    [SerializeField] TMP_Text fullScreenText;
    [SerializeField] SwitchToggle fullScreenToggle;

    const string fullScreenOn = "On";
    const string fullScreenOff = "Off";

#if UNITY_EDITOR
    [redd096.Attributes.Button]
    void SetResizableWindow()
    {
        UnityEditor.PlayerSettings.resizableWindow = true;
    }
#endif

    //update ui in Start, so if we want to load values in GameManager Awake we have time
    private void Start()
    {
        //auto register events, instead of put them in inspector
        if (autoRegisterEvents)
        {
            masterVolumeSlider.onValueChanged.AddListener(OnSetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(OnSetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(OnSetSfxVolume);
            uiVolumeSlider.onValueChanged.AddListener(OnSetUIVolume);
            fullScreenToggle.onValueChanged.AddListener(OnSetFullScreen);
        }

        //update ui
        UpdateUI();
    }

    private void OnDestroy()
    {
        //unregister events
        if (autoRegisterEvents)
        {
            masterVolumeSlider.onValueChanged.RemoveListener(OnSetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(OnSetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSetSfxVolume);
            uiVolumeSlider.onValueChanged.RemoveListener(OnSetUIVolume);
            fullScreenToggle.onValueChanged.RemoveListener(OnSetFullScreen);
        }
    }

    #region public API

    public void ApplyInGame()
    {
        SoundManager.instance.SetMasterVolume(GameManager.instance.masterVolume);

        SoundManager.instance.SetTypeVolume(AudioData.EAudioType.Music, GameManager.instance.musicVolume);
        SoundManager.instance.SetTypeVolume(AudioData.EAudioType.Sfx, GameManager.instance.sfxVolume);
        SoundManager.instance.SetTypeVolume(AudioData.EAudioType.UI, GameManager.instance.uiVolume);

        Screen.fullScreen = GameManager.instance.fullScreen;
    }

    public void UpdateUI()
    {
        //get values
        float masterVolume = GameManager.instance.masterVolume;
        float musicVolume = GameManager.instance.musicVolume;
        float sfxVolume = GameManager.instance.sfxVolume;
        float uiVolume = GameManager.instance.uiVolume;
        bool fullScreen = GameManager.instance.fullScreen;

        //update ui
        masterVolumeText.text = Mathf.CeilToInt(masterVolume * 100).ToString();
        masterVolumeSlider.SetValueWithoutNotify(masterVolume);

        musicVolumeText.text = Mathf.CeilToInt(musicVolume * 100).ToString();
        musicVolumeSlider.SetValueWithoutNotify(musicVolume);
        sfxVolumeText.text = Mathf.CeilToInt(sfxVolume * 100).ToString();
        sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
        uiVolumeText.text = Mathf.CeilToInt(uiVolume * 100).ToString();
        uiVolumeSlider.SetValueWithoutNotify(uiVolume);

        fullScreenText.text = fullScreen ? fullScreenOn : fullScreenOff;
        fullScreenToggle.SetToggle(fullScreen, triggerEvent: false);
    }

    #endregion

    #region events

    private void OnSetMasterVolume(float value)
    {
        GameManager.instance.masterVolume = value;
        ApplyInGame();
        UpdateUI();
    }

    private void OnSetMusicVolume(float value)
    {
        GameManager.instance.musicVolume = value;
        ApplyInGame();
        UpdateUI();
    }

    private void OnSetSfxVolume(float value)
    {
        GameManager.instance.sfxVolume = value;
        ApplyInGame();
        UpdateUI();
    }

    private void OnSetUIVolume(float value)
    {
        GameManager.instance.uiVolume = value;
        ApplyInGame();
        UpdateUI();
    }

    private void OnSetFullScreen(bool value)
    {
        GameManager.instance.fullScreen = value;
        ApplyInGame();
        UpdateUI();
    }

    #endregion
}
