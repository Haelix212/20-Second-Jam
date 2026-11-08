using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;
    private Toggle cameraInvert;

    public AudioMixer mixer;

    public void Start()
    {
        Transform settingsPanel = this.transform.Find("SettingsParent");
        masterSlider = settingsPanel.Find("MasterVolumeSlider").GetComponent<Slider>();
        musicSlider = settingsPanel.Find("MusicVolumeSlider").GetComponent<Slider>();
        sfxSlider = settingsPanel.Find("SFXVolumeSlider").GetComponent<Slider>();
        cameraInvert = settingsPanel.Find("CameraInvert").GetComponent<Toggle>();

        // Initalize sound setting sliders
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume"));
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            mixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }
        SetSliders();

        // Initalize camera invert setting
        if (PlayerPrefs.HasKey("CameraInvert"))
        {
            if (PlayerPrefs.GetInt("CameraInvert") == -1)
            {
                cameraInvert.SetIsOnWithoutNotify(true); // Updates in-game toggle state without firing the function attached to it
            }
            else
            {
                PlayerPrefs.SetInt("CameraInvert", 1);
            }
        } 
        else
        {
            PlayerPrefs.SetInt("CameraInvert", 1);
        }

        // Initialize mouse sensitivity setting
    }

    private void SetSliders ()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void UpdateMasterVolume ()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
    }

    public void UpdateMusicVolume()
    {
        mixer.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void UpdateSFXVolume()
    {
        mixer.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
    }

    public void UpdateCameraInvert()
    {
        PlayerPrefs.SetInt("CameraInvert", -PlayerPrefs.GetInt("CameraInvert"));
    }
}
