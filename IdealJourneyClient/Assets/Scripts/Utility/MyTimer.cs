using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTimer
{
    private float m_cycleTime = 1.0f;
    public float CycleTime
    {
        get { return m_cycleTime; }
        set
        {
            m_cycleTime = value;
            ResetTimer();
        }
    }

    public Action OnCycle { get; set; }
    public float TimeRemaining { get; private set; }
    public float TimeElapsed
    {
        get { return CycleTime - TimeRemaining; }
    }

    public MyTimer(float cycleTime, Action onCycle)
    {
        CycleTime = cycleTime;
        OnCycle = onCycle;
    }

    public void ResetTimer()
    {
        TimeRemaining = CycleTime;
    }

    public void TickTimer(float dt)
    {
        TimeRemaining -= dt;
        if (TimeRemaining <= 0.0f)
        {
            TimeRemaining += CycleTime;
            OnCycle();
        }
    }
}
