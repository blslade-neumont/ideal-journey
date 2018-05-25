using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class TimeToRemainingBar : MonoBehaviour {
    private Image m_image;
    [SerializeField] private GameController m_gameController;
    [SerializeField] private Color m_minColor;
    [SerializeField] private Color m_maxColor;
    [SerializeField] private float m_threshold = 0.75f;

    private void Awake()
    {
        m_image = GetComponent<Image>();
    }

    void Update()
    {
        if (!m_gameController.DidFail)
        {
            float lerpPercent = m_gameController.FillAmount;
            m_image.fillAmount = lerpPercent;
            m_image.color = Color.Lerp(m_minColor, m_maxColor, lerpPercent < m_threshold ? lerpPercent / m_threshold : 1.0f);
        }
        else
        {
            m_image.fillAmount = 1.0f;
            m_image.color = m_minColor;
        }
	}
}
