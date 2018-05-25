using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOrientation : MonoBehaviour {

    // Use this for initialization
    private void Start () {
        Gyroscope gyro = Input.gyro;
        gyro.enabled = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToPortrait = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
