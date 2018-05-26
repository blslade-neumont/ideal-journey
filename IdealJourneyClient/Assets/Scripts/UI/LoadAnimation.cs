using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class LoadAnimation : MonoBehaviour {

    private Image m_icon;
    [SerializeField, Range(0, 2)] private float m_fadeSpeed;
    [SerializeField, Range(0, 300)] private float m_spinSpeed;

	// Use this for initialization
	void Start () {
        m_icon = GetComponent<Image>();
        m_icon.fillClockwise = true;
        m_icon.fillAmount = 0;
	}

    private void OnDisable()
    {
        transform.rotation = Quaternion.identity;
        m_icon.fillClockwise = true;
        m_icon.fillAmount = 0;
    }

    // Update is called once per frame
    void Update () {
        if (m_icon.fillClockwise && m_icon.fillAmount == 1)
        {
            m_icon.fillClockwise = false;
            m_fadeSpeed *= -1;
        }
        else if (!m_icon.fillClockwise && m_icon.fillAmount == 0)
        {
            m_icon.fillClockwise = true;
            m_fadeSpeed *= -1;
        }
        m_icon.transform.Rotate(0, 0,-1 * m_spinSpeed * Time.deltaTime);
        m_icon.fillAmount += m_fadeSpeed * Time.deltaTime;
	}
}
