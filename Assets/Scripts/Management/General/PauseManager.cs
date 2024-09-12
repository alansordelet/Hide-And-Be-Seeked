using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public bool isPaused = false;

    [SerializeField] public Slider sensitivityMusicSlider;
    [SerializeField] private TMP_Text sensitivityMusicText;

    [SerializeField] public Slider sensitivitySoundSlider;
    [SerializeField] private TMP_Text sensitivitySoundText;

    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityText;

    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject menuPanel;

    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private Toggle fullscreenToggle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    private void Start()
    {
        LoadSettings();
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    private void Update()
    {
        AudioManager.instance.SetMusicVolume((float)sensitivityMusicSlider.value);
        AudioManager.instance.SetSoundVolume((float)sensitivitySoundSlider.value);

        if (Input.GetKeyDown(KeyCode.Escape) && !settingsPanel.activeSelf)
        {
            Pause();
        }

        int roundedValueMusic = Mathf.RoundToInt(sensitivityMusicSlider.value * 100.0f);
        sensitivityMusicText.text = roundedValueMusic.ToString();
        PlayerPrefs.SetFloat("SensitivityMusic", sensitivityMusicSlider.value);

        int roundedValueSound = Mathf.RoundToInt(sensitivitySoundSlider.value * 100.0f);
        sensitivitySoundText.text = roundedValueSound.ToString();
        PlayerPrefs.SetFloat("SensitivitySound", sensitivitySoundSlider.value);

        sensitivityText.text = sensitivitySlider.value.ToString();
        PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);
    }

    void Pause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            ActivateMenu();
        }
        else
        {
            DeactivateMenu();
        }
    }
    public void BacktoMainMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
    public void ActivateMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
        menuPanel.SetActive(true);
    }
    public void DeactivateMenu()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        menuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }


    public void QuitGame()
    {
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
    public void ToggleSettings()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    // Method to handle saving settings
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("SensitivityMusic", sensitivityMusicSlider.value);
        PlayerPrefs.SetFloat("SensitivitySound", sensitivitySoundSlider.value);
        PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);

    }

    public void LoadSettings()
    {
        vsyncToggle.isOn = QualitySettings.vSyncCount > 0;

        fullscreenToggle.isOn = Screen.fullScreen;

        sensitivityMusicSlider.value = PlayerPrefs.GetFloat("SensitivityMusic", 0.5f);
        sensitivitySoundSlider.value = PlayerPrefs.GetFloat("SensitivitySound", 0.5f);
        sensitivitySlider.value = PlayerPrefs.GetInt("Sensitivity", 500);

    }
}

