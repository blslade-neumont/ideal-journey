using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseTextController : MonoBehaviour
{

    private enum TextState
    {
        FADE_OUT,
        PULSING,
        FADE_IN,
    }

    [Header("Configuration")]
    [Header("External References")]
    [SerializeField]
    private Text[] m_texts;

    [Header("Internal Values")]
    [SerializeField]
    private TextState m_textState = TextState.PULSING;
    [SerializeField] [Range(0.0f, 2.0f)] private float m_fadeOutSpeed = 0.8f;
    [SerializeField] [Range(0.0f, 2.0f)] private float m_fadeOutPauseLength = 0.6f;
    [SerializeField] [Range(0.0f, 2.0f)] private float m_fadeInSpeed = 0.8f;
    [SerializeField] [Range(0.0f, 2.0f)] private float m_fadeInPauseLength = 1.2f;

    // Private Data Members
    private bool m_isFading = true;
    private float m_pauseTimer = 0.0f;

    // Use this for initialization
    void Awake()
    {
        foreach (Text text in m_texts)
        {
            if (m_textState == TextState.FADE_IN)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
            }
            else if (m_textState == TextState.FADE_OUT)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (m_textState == TextState.PULSING)
        {
            if (m_pauseTimer >= 0.0f)
            {
                m_pauseTimer -= Time.deltaTime;
                return;
            }

            if (m_texts[0].color.a <= 0)
            {
                m_isFading = false;
                m_pauseTimer = m_fadeOutPauseLength;
            }
            else if (m_texts[0].color.a >= 1)
            {
                m_isFading = true;
                m_pauseTimer = m_fadeInPauseLength;
            }

            foreach (Text text in m_texts)
            {
                if (m_isFading)
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / m_fadeOutSpeed));
                }
                else
                {
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / m_fadeInSpeed));
                }
            }
        }
        else if (m_textState == TextState.FADE_IN)
        {
            if (m_pauseTimer >= 0.0f)
            {
                m_pauseTimer -= Time.deltaTime;
                return;
            }

            if (m_texts[0].color.a <= 0)
            {
                m_isFading = false;
                m_pauseTimer = m_fadeOutPauseLength;
            }

            foreach (Text text in m_texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / m_fadeInSpeed));
            }
        }

        else if (m_textState == TextState.FADE_OUT)
        {
            if (m_pauseTimer >= 0.0f)
            {
                m_pauseTimer -= Time.deltaTime;
                return;
            }

            if (m_texts[0].color.a >= 1)
            {
                m_isFading = true;
                m_pauseTimer = m_fadeInPauseLength;
            }

            foreach (Text text in m_texts)
            {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / m_fadeOutSpeed));
            }
        }
    }
}
