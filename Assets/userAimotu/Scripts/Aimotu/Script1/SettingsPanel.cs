using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button closeBtn;
    public Slider bgmSlider;
    public Slider sfxSlider;
    
    public GameObject settingsPanel;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            bgmSlider.value = AudioManager.Instance.GetBGMVolume();
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        }
        closeBtn.onClick.AddListener(OnClickClose);
        
        bgmSlider.onValueChanged.AddListener(OnBGMChange);
        sfxSlider.onValueChanged.AddListener(OnSFXChange);
    }

    private void OnClickClose()
    {
        settingsPanel.SetActive(false);
    }

    private void OnBGMChange(float value)
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.SetBGMVolume(value);
        }
    }

    private void OnSFXChange(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSFXVolume(value);
        }
    }
}
