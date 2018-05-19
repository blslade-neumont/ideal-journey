using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisableSFX : MonoBehaviour
{
    private Toggle myToggle = null;

    void Awake()
    {
        myToggle = GetComponent<Toggle>();
        AudioHelper.SFXEnabled = PersistToDeviceHelper.IsSFXSavedOn();
        myToggle.isOn = AudioHelper.SFXEnabled;
    }

    void OnDisable()
    {
        AudioHelper.SFXEnabled = myToggle.isOn;
        PersistToDeviceHelper.SaveSFXOption(AudioHelper.SFXEnabled);
    }

    public void OnToggleMe()
    {
        AudioHelper.SFXEnabled = myToggle.isOn;
    }
}
