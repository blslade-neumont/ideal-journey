using UnityEngine;

public static class AudioHelper
{
    public static float BGMMultiplier = 1.0f;
    public static float VoiceMultiplier = 1.0f;
    public static float SFXMultiplier = 1.0f;
    public static bool BGMEnabled = true;
    public static bool SFXEnabled = true;
    public static bool VoiceEnabled = true;

    public static void InitSFX(AudioSource sfx)
    {
        if (sfx != null)
        {
            sfx.volume *= SFXMultiplier;
            if (sfx.isPlaying && !SFXEnabled) { sfx.Stop(); }
        }
    }

    public static void PlaySFX(AudioSource sfx)
    {
        if (SFXEnabled && sfx != null) { sfx.Play(); }
    }
    public static void InitBGM(AudioSource bgm)
    {
        if (bgm != null)
        {
            bgm.volume *= BGMMultiplier;
            if (bgm.isPlaying && !BGMEnabled) { bgm.Stop(); }
        }
    }

    public static void PlayBGM(AudioSource bgm)
    {
        if (BGMEnabled && bgm != null) { bgm.Play(); }
    }

    public static void InitVoice(AudioSource voice)
    {
        if (voice != null)
        {
            voice.volume *= VoiceMultiplier;
            if (voice.isPlaying && !VoiceEnabled) { voice.Stop(); }
        }
    }

    public static void PlayVoice(AudioSource voice)
    {
        if (VoiceEnabled && voice != null) { voice.Play(); }
    }
}
