using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapEasterEgg : MonoBehaviour {

    private bool m_foundEgg = false;
    private int m_count = 0;
    private float m_timer;
    [SerializeField] private float m_timerMax;
    [SerializeField] private int m_countMax;
    [SerializeField] private Text m_text;
    [SerializeField] private float m_fadeSpeed;
    [SerializeField] private string m_posMessage;
    [SerializeField] private string m_negMessage;


    private void Start()
    {
        m_timer = m_timerMax;
        m_text.text = m_negMessage;
    }

    public void Tap()
    {
        if (!m_foundEgg)
        {
            ++m_count;
        }
    }

    private void Update()
    {
        if (!m_foundEgg)
        {
            if (m_count >= m_countMax)
            {
                m_foundEgg = true;
                m_text.text = m_text.text == m_negMessage ? m_posMessage : m_negMessage;
                m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, 1.0f);
                m_count = 0;
            }
            else if (m_timer <= 0.0f && m_count != 0)
            {
                --m_count;
                m_timer = m_timerMax;
            }
            m_timer -= Time.deltaTime;
        }
        else if (m_text.color.a > 0.0f)
        {
            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, m_text.color.a - (m_fadeSpeed * Time.deltaTime));
            if (m_text.color.a <= 0.0f)
            {
                m_foundEgg = false;
            }
        }
    }
}
