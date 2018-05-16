using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShake : MonoBehaviour {

    bool m_listen;
    [SerializeField] private Text m_text;

    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;
    float shakeDetectionThreshold = 3.0f;

    float lowPassFilterFactor;
    Vector3 lowPassValue;

    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.gyro.userAcceleration;
    }

    void Update()
    {
        Vector3 acceleration = Input.gyro.userAcceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        {
            m_text.text = "Did a shake";
            Debug.Log("Shake event detected at time " + Time.time);
            Debug.Log(deltaAcceleration);
        }
        //if (m_listen)
        //{
        //    Vector3 acceleration = Input.gyro.userAcceleration;
        //    lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        //    Vector3 deltaAcceleration = acceleration - lowPassValue;

        //    if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold)
        //    {
        //        m_text.text = "Did a shake";
        //        Debug.Log("Shake event detected at time " + Time.time);
        //    }
        //}
    }

    public void Shake()
    {
        m_listen = true;
        m_text.text = "";
    }
}
