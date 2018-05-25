using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisableVibration : MonoBehaviour
{
    private Toggle myToggle = null;

    private void Awake()
    {
        myToggle = GetComponent<Toggle>();
        VibrateHelper.VibrateEnabled = PersistToDeviceHelper.IsVibrationSavedOn();
        myToggle.isOn = VibrateHelper.VibrateEnabled;
    }

    private void OnDisable()
    {
        VibrateHelper.VibrateEnabled = myToggle.isOn;
        PersistToDeviceHelper.SaveVibrationOption(VibrateHelper.VibrateEnabled);
    }

    public void OnToggleMe()
    {
        VibrateHelper.VibrateEnabled = myToggle.isOn;
    }
}
