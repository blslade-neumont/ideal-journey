using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGyro : MonoBehaviour {

    private Gyroscope m_gyro;

    // Use this for initialization
    private void Start () {
        m_gyro = Input.gyro;
        m_gyro.enabled = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToPortrait = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
	}

    // Update is called once per frame
    private void Update () {
        m_gyro = Input.gyro;
        transform.rotation = m_gyro.attitude;

        transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.z, -transform.rotation.eulerAngles.y);
	}
}
