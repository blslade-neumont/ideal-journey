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

    public static LoginResponse GetCurrentLogin()
    {
        var loginResponseStr = PlayerPrefs.GetString(Keys.AUTH_TOKEN);
        if (loginResponseStr == null || string.IsNullOrEmpty(loginResponseStr.Trim())) return null;
        return JsonUtility.FromJson<LoginResponse>(loginResponseStr);
    }

    public static void SetCurrentLogin(LoginResponse loginResponse)
    {
        var loginResponseStr = loginResponse == null ? "" : JsonUtility.ToJson(loginResponse);
        PlayerPrefs.SetString(Keys.AUTH_TOKEN, loginResponseStr);
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
