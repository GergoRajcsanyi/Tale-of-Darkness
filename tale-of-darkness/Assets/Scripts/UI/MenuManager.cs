using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour, DataInterface
{
    #region Audio

    [Header("AUDIO")]
    public Slider audioSlider;
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void PlayAudio()
    {
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
    #endregion

    #region Joystick visibility

    public Toggle toggle;
    public static bool visibility;
    public event EventHandler OnVisibiltyChanged;

    public void SetJoystickVisible(bool isVisible)
    { 
        visibility = isVisible;
        toggle.isOn = visibility;
        OnVisibiltyChanged?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    #region Save System

    public void LoadData(GameData data)
    {
        this.toggle.isOn = data.joystickVisibility;
        this.audioSlider.value = data.audioVolume;
    }

    public void SaveData(GameData data)
    {
        data.joystickVisibility = this.toggle.isOn;
        data.audioVolume = this.audioSlider.value;
    }
    #endregion
}
