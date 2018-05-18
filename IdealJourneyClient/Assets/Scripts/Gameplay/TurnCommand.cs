using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCommand : GameCommand
{
    private Gyroscope m_gyro;
    private Vector3 m_startUp;
    private const float TOLERANCE = 45;

    public override void Begin()
    {
        m_gyro = Input.gyro;
        m_startUp = (m_gyro.attitude * Vector3.up).normalized;
    }

    public override bool IsComplete()
    {
        Vector3 currentUp = (m_gyro.attitude * Vector3.up).normalized;
        return Vector3.Dot(currentUp, m_startUp) < 0 || base.IsComplete();
    }

    public override string AsText()
    {
        return "TURN!!!";
    }

    public override void PlaySFX()
    {
        GetCacheSFX(Tags.TURN_SFX);
        base.PlaySFX();
    }
}
