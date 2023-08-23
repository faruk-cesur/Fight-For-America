using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
{
    #region Variables

    [field: SerializeField] public CanvasGroup SettingsUI { get; set; }
    [field: SerializeField, BoxGroup("Sound and Vibration")] public GameObject SoundOnButton { get; set; }
    [field: SerializeField, BoxGroup("Sound and Vibration")] public GameObject SoundOffButton { get; set; }
    [field: SerializeField, BoxGroup("Sound and Vibration")] public GameObject VibrationOnButton { get; set; }
    [field: SerializeField, BoxGroup("Sound and Vibration")] public GameObject VibrationOffButton { get; set; }
    public UnityAction OnSaveSettings;
    public bool IsSoundActivated { get; private set; }
    public bool IsVibrationActivated { get; private set; }
    private bool IsSettingsOpened { get; set; }

    #endregion

    #region Get Starting Data

    private void Start() => GetStartingData();

    private void GetStartingData()
    {
        if (!PlayerPrefs.HasKey("SoundSettings"))
        {
            EnableSound(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundSettings") == 0)
            {
                DisableSound();
            }
            else if (PlayerPrefs.GetInt("SoundSettings") == 1)
            {
                EnableSound(false);
            }
        }

        if (!PlayerPrefs.HasKey("VibrationSettings"))
        {
            EnableVibration(false);
        }
        else
        {
            if (PlayerPrefs.GetInt("VibrationSettings") == 0)
            {
                DisableVibration(false);
            }
            else if (PlayerPrefs.GetInt("VibrationSettings") == 1)
            {
                EnableVibration(false);
            }
        }
    }

    #endregion

    #region Settings

    public void OpenSettingsUI()
    {
        UIManager.Instance.FadeCanvasGroup(SettingsUI, 0.5f, true);
    }

    public void CloseSettingsUI()
    {
        UIManager.Instance.FadeCanvasGroup(SettingsUI, 0.5f, false);
    }

    public void EnableSound(bool playAudio)
    {
        SoundOnButton.SetActive(true);
        SoundOffButton.SetActive(false);
        IsSoundActivated = true;
        PlayerPrefs.SetInt("SoundSettings", 1);
        AudioListener.pause = false;
    }

    public void DisableSound()
    {
        SoundOnButton.SetActive(false);
        SoundOffButton.SetActive(true);
        IsSoundActivated = false;
        PlayerPrefs.SetInt("SoundSettings", 0);
        AudioListener.pause = true;
    }

    public void EnableVibration(bool playAudio)
    {
        VibrationOnButton.SetActive(true);
        VibrationOffButton.SetActive(false);
        IsVibrationActivated = true;
        PlayerPrefs.SetInt("VibrationSettings", 1);
        Vibration.VibratePeek();
    }

    public void DisableVibration(bool playAudio)
    {
        VibrationOnButton.SetActive(false);
        VibrationOffButton.SetActive(true);
        IsVibrationActivated = false;
        PlayerPrefs.SetInt("VibrationSettings", 0);
    }

    #endregion
}