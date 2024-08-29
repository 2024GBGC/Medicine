using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TMP_Dropdown resolutionDropdown;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    void Start()
    {
        LoadSettings();

        bgmSlider.onValueChanged.AddListener(delegate {
            PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
            bgmSource.volume = bgmSlider.value;
        });

        sfxSlider.onValueChanged.AddListener(delegate {
            PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
            sfxSource.volume = sfxSlider.value;
        });

        resolutionDropdown.onValueChanged.AddListener(delegate {
            PlayerPrefs.SetInt("ResolutionIndex", resolutionDropdown.value);
            ChangeResolution();
        });
    }

    void LoadSettings()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    void ChangeResolution()
    {
        string[] res = resolutionDropdown.options[resolutionDropdown.value].text.Split('x');
        int width = int.Parse(res[0]);
        int height = int.Parse(res[1]);
        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}