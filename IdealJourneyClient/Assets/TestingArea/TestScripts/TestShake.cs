using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShake : MonoBehaviour {
    private bool m_listen;
    [SerializeField] private Text m_text;
    private float accelerometerUpdateInterval = 1.0f / 60.0f;
    private float lowPassKernelWidthInSeconds = 1.0f;
    private float shakeDetectionThreshold = 3.0f;
    private float lowPassFilterFactor;
    private Vector3 lowPassValue;

    private void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.gyro.userAcceleration;
    }

    private void Update()
    {
        if (m_listen)
        {
            Vector3 acceleration = Input.gyro.userAcceleration;
            lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
            Vector3 deltaAcceleration = acceleration - lowPassValue;

            if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
            {
                m_text.text = "Did a shake";
                Debug.Log("Shake event detected at time " + Time.time);
            }
        }
    }

    public void Shake()
    {
        m_listen = true;
        m_text.text = "";
    }
}
