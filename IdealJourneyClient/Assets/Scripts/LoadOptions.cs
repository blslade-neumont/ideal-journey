using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOptions : MonoBehaviour
{
	private void Awake()
    {
        AudioHelper.BGMEnabled = PersistToDeviceHelper.IsBackgroundMusicSavedOn();
        VibrateHelper.VibrateEnabled = PersistToDeviceHelper.IsVibrationSavedOn();
        AudioHelper.SFXEnabled = PersistToDeviceHelper.IsSFXSavedOn();
        AudioHelper.VoiceEnabled = PersistToDeviceHelper.IsVoiceSavedOn();
    }

}
