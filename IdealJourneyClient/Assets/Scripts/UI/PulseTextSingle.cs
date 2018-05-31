using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulseTextSingle : MonoBehaviour
{

    private enum TextState
    {
        FADE_OUT,
        PULSING,
        FADE_IN,
        INACTIVE,
    }

    [Header("Configuration")]

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
    private Text m_text;

    // Use this for initialization
    void Awake()
    {
        if(m_textState == TextState.INACTIVE)
        {
            return;
        }
        m_text = gameObject.GetComponent<Text>();
        if (m_textState == TextState.FADE_IN)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, 0.0f);
        }
        else if (m_textState == TextState.FADE_OUT)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, 1.0f);
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

            if (m_text.color.a <= 0)
            {
                m_isFading = false;
                m_pauseTimer = m_fadeOutPauseLength;
            }
            else if (m_text.color.a >= 1)
            {
                m_isFading = true;
                m_pauseTimer = m_fadeInPauseLength;
            }
            
            if (m_isFading)
            {
                m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, m_text.color.a - (Time.deltaTime / m_fadeOutSpeed));
            }
            else
            {
                m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, m_text.color.a + (Time.deltaTime / m_fadeInSpeed));
            }
        }
        else if (m_textState == TextState.FADE_IN)
        {
            if (m_pauseTimer >= 0.0f)
            {
                m_pauseTimer -= Time.deltaTime;
                return;
            }

            if (m_text.color.a <= 0)
            {
                m_isFading = false;
                m_pauseTimer = m_fadeOutPauseLength;
            }

            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, m_text.color.a + (Time.deltaTime / m_fadeInSpeed));
        }

        else if (m_textState == TextState.FADE_OUT)
        {
            if (m_pauseTimer >= 0.0f)
            {
                m_pauseTimer -= Time.deltaTime;
                return;
            }

            if (m_text.color.a >= 1)
            {
                m_isFading = true;
                m_pauseTimer = m_fadeInPauseLength;
            }

            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, m_text.color.a + (Time.deltaTime / m_fadeOutSpeed));
        }
    }
}
