using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public class MainMenuManagerScript : MonoBehaviour
{
    private static MainMenuManagerScript _instance;
    private MainMenuManagerScript() { }

    [SerializeField] public Slider sensitivityMusicSlider;
    [SerializeField] private TMP_Text sensitivityMusicText;

    [SerializeField] public Slider sensitivitySoundSlider;
    [SerializeField] private TMP_Text sensitivitySoundText;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Button playButton;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private CinemachineVirtualCamera camMenu;
    [SerializeField] private CinemachineVirtualCamera camSettings;

    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private Image fadeimage;

    private bool playClicked = false;

    private bool settingsEnabled = false;

    public static MainMenuManagerScript GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;
        vsyncToggle.onValueChanged.AddListener(ChangeVSyncSetting);

        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(ChangeFullscreenSetting);

        sensitivityMusicSlider.value = PlayerPrefs.GetFloat("SensitivityMusic", 0.5f);
        sensitivitySoundSlider.value = PlayerPrefs.GetFloat("SensitivitySound", 0.5f);
        sensitivitySlider.value = PlayerPrefs.GetInt("Sensitivity", 500);
        Time.timeScale = 1f;

        menuMusic.Play();
    }
    void Update()
    {
        AudioManager.instance.SetMusicVolume((float)sensitivityMusicSlider.value);
        AudioManager.instance.SetSoundVolume((float)sensitivitySoundSlider.value);

        int roundedValueMusic = Mathf.RoundToInt(sensitivityMusicSlider.value * 100.0f);
        sensitivityMusicText.text = roundedValueMusic.ToString();
        PlayerPrefs.SetFloat("SensitivityMusic", sensitivityMusicSlider.value);

        int roundedValueSound = Mathf.RoundToInt(sensitivitySoundSlider.value * 100.0f);
        sensitivitySoundText.text = roundedValueSound.ToString();
        PlayerPrefs.SetFloat("SensitivitySound", sensitivitySoundSlider.value);

        sensitivityText.text = sensitivitySlider.value.ToString();
        PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);

        if (settingsEnabled)
        {
          
            if (Input.GetKeyUp(KeyCode.Escape))
            {               
                camSettings.Priority = 9;
                camMenu.Priority = 10;

                settingsPanel.SetActive(false);
                menuPanel.SetActive(true);
                settingsEnabled = false;
            }
        }
        if (playClicked)
        {
            FadeIn();
            menuMusic.volume = Mathf.Max(menuMusic.volume - 0.25f * Time.deltaTime, 0.3f);
        }
    }
    public void OnPlayButtonClicked()
    {
        //  AudioManager.GetInstance().PlaySoundSelect();

        playClicked = true;
       
    }
    public void OnSettingsButtonClicked()
    {
      //  AudioManager.GetInstance().PlaySoundSelect();
        settingsEnabled = true;

        camMenu.Priority = 9;
        camSettings.Priority = 10;

        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void OnBackButtonClicked()
    {
        //  AudioManager.GetInstance().PlaySoundSelect();
        settingsEnabled = false;

        camSettings.Priority = 9;
        camMenu.Priority = 10;

        settingsPanel.SetActive(false);
     
        menuPanel.SetActive(true);
    }
    public void OnQuitButtonClicked()
    {
       // AudioManager.GetInstance().PlaySoundSelect();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
    public void ChangeVSyncSetting(bool isOn)
    {
       // AudioManager.GetInstance().PlaySoundSelect();
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }
    public void ChangeFullscreenSetting(bool isOn)
    {
        //  AudioManager.GetInstance().PlaySoundSelect();
        Screen.fullScreen = isOn;
    }
    private UnityEvent onFadeInComplete;
    void FadeIn()
    {
        Color currentColor = fadeimage.color;
        currentColor.a = Mathf.MoveTowards(currentColor.a, 1.0f, 1f * Time.deltaTime);
        fadeimage.color = currentColor;

        if (Mathf.Approximately(currentColor.a, 1.0f))
        {
            SceneManager.LoadScene("Game");

        }
    }
}
