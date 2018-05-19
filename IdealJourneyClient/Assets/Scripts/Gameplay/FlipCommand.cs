using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCommand : GameCommand
{
    private Gyroscope m_gyro;
    private Vector3 m_target;
    private const float TOLERANCE = 45;

    public override void Begin()
    {
        m_gyro = Input.gyro;
        m_target = (m_gyro.attitude * Vector3.back).normalized;
    }

    public override bool IsComplete()
    {
        Vector3 currentForward = (m_gyro.attitude * Vector3.forward).normalized;
        float angle = Mathf.Acos(Vector3.Dot(m_target, currentForward));
        return angle < Mathf.Deg2Rad * TOLERANCE || base.IsComplete();
    }

    public override string AsText()
    {
        return "FLIP!!!";
    }

    public override void PlayVoiceOverSFX()
    {
        GetCacheSFX(Tags.FLIP_SFX);
        base.PlayVoiceOverSFX();
    }
}
