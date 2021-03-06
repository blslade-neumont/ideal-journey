﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisableVoice : MonoBehaviour
{
    private Toggle myToggle = null;

    private void Awake()
    {
        myToggle = GetComponent<Toggle>();
        AudioHelper.VoiceEnabled = PersistToDeviceHelper.IsVoiceSavedOn();
        myToggle.isOn = AudioHelper.VoiceEnabled;
    }

    private void OnDisable()
    {
        AudioHelper.VoiceEnabled = myToggle.isOn;
        PersistToDeviceHelper.SaveVoiceOption(AudioHelper.VoiceEnabled);
    }

    public void OnToggleMe()
    {
        AudioHelper.VoiceEnabled = myToggle.isOn;
    }
}
