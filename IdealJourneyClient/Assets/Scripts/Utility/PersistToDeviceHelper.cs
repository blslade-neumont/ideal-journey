using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistToDeviceHelper
{
    public static void SaveVibrationOption(bool value)
    {
        SaveBooleanToPlayerPrefs(Keys.VIBRATE_OPTION, value);
    }

    public static bool IsVibrationSavedOn()
    {
        return GetBooleanFromPlayerPrefs(Keys.VIBRATE_OPTION);
    }

    public static bool IsSFXSavedOn()
    {
        return GetBooleanFromPlayerPrefs(Keys.SFX_OPTION);
    }

    public static bool IsVoiceSavedOn()
    {
        return GetBooleanFromPlayerPrefs(Keys.VOICE_OPTION);
    }

    public static void SaveBackgroundMusicOption(bool value)
    {
        SaveBooleanToPlayerPrefs(Keys.BACKGROUND_MUSIC_OPTION, value);
    }

    public static void SaveSFXOption(bool SFXEnabled)
    {
        SaveBooleanToPlayerPrefs(Keys.SFX_OPTION, SFXEnabled);
    }

    public static void SaveVoiceOption(bool voiceEnabled)
    {
        SaveBooleanToPlayerPrefs(Keys.VOICE_OPTION, voiceEnabled);
    }

    public static bool IsBackgroundMusicSavedOn()
    {
        return GetBooleanFromPlayerPrefs(Keys.BACKGROUND_MUSIC_OPTION);
    }

    private static void SaveBooleanToPlayerPrefs(string key, bool value)
    {
        PlayerPrefs.SetString(key, value ? "true" : "false");
    }

    private static bool GetBooleanFromPlayerPrefs(string key, bool defaultValue = true)
    {
        return PlayerPrefs.GetString(key, defaultValue ? "true" : "false") == "true";
    }
}
