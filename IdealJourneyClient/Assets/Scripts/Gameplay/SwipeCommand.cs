using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeCommand : GameCommand
{

    private Vector2 m_initTouchPos;
    private float m_sqrMagTolerance; // NOT CONST, can change based on screen dpi

    public override void Begin()
    {
        m_initTouchPos = Vector2.zero;
        m_sqrMagTolerance = Screen.dpi;
        m_sqrMagTolerance *= m_sqrMagTolerance;
    }

    public override bool IsComplete()
    {
        if (Input.touchCount == 0) return base.IsComplete();
        Touch currentTouch = Input.touches[0];
        if (currentTouch.phase == TouchPhase.Began)
        {
            m_initTouchPos = currentTouch.position;
        }
        return (currentTouch.phase == TouchPhase.Ended && (currentTouch.position - m_initTouchPos).sqrMagnitude > m_sqrMagTolerance)
            || base.IsComplete();
    }

    public override string AsText()
    {
        return "SWIPE!!!";
    }

    public override void PlayVoiceOverSFX()
    {
        GetCacheSFX(Tags.SWIPE_SFX);
        base.PlayVoiceOverSFX();
    }
}
