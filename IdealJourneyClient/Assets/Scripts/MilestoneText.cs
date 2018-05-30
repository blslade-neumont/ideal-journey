using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class MilestoneText : MonoBehaviour {
    private Text m_text;
    private MyTimer m_timer;
    private bool m_timerOn;

    [SerializeField] private int m_minFont = 0;
    [SerializeField] private int m_maxFont = 124;

    private void Awake()
    {
        m_timerOn = false;
        m_text = GetComponent<Text>();
        m_timer = new MyTimer(0.0f, OnTimerEnd);
    }

    private void OnTimerEnd()
    {
        m_timerOn = false;
        m_text.enabled = false;
        m_text.fontSize = m_maxFont;
        m_text.text = "";
    }

    private void Update()
    {
        if (m_timerOn)
        {
            m_timer.TickTimer(Time.deltaTime);
            m_text.fontSize = Mathf.FloorToInt(Mathf.Lerp(m_minFont, m_maxFont, m_timer.TimeElapsed / m_timer.CycleTime));
        }
    }

    public void Begin(float time, string text)
    {
        m_timer.CycleTime = time;
        m_timerOn = true;
        m_text.enabled = true;
        m_text.text = text;
        m_text.fontSize = m_minFont;
    }
}
