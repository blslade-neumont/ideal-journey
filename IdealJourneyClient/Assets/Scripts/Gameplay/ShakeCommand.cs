using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCommand : GameCommand
{

    private float m_accelerometerUpdateInterval = 1.0f / 60.0f;
    private float m_lowPassKernelWidthInSeconds = 1.0f;
    private const float SHAKEDETECTIONTHRESHOLD = 25.0f; // This is a squared value, technical threshold is 3.0

    private float lowPassFilterFactor;
    private Vector3 lowPassValue;

    private Vector3 m_firstValidDelta;

    public override void Begin()
    {
        lowPassFilterFactor = m_accelerometerUpdateInterval / m_lowPassKernelWidthInSeconds;
        lowPassValue = Vector3.zero;
        m_firstValidDelta = Vector3.zero;
    }

    public override bool IsComplete()
    {
        Vector3 acceleration = Input.gyro.userAcceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;
        return deltaAcceleration.sqrMagnitude >= SHAKEDETECTIONTHRESHOLD || base.IsComplete();
    }

    public override string AsText()
    {
        return "SHAKE!!!";
    }
}
