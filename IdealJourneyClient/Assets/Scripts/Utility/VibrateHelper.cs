using UnityEngine;

public static class VibrateHelper
{
    public static bool VibrateEnabled = true;

    public static void Vibrate()
    {
        if (VibrateEnabled) { Handheld.Vibrate(); }
    }
}
