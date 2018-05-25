using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour {

    [Header("Configuration")]

    [Header("Internal Information")]
    [SerializeField] [Range(0.0f, 10.0f)] private float m_startDelay = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float m_animationSpeed = 0.5f;

    // Private Internal Information
    private Vector3 m_position;
    private float m_timer;


    // Use this for initialization
    private void Start () {

        m_timer = m_startDelay;

        // Store original Location
        m_position = transform.position;
        
        // Set button location arbitrarily to the right
        transform.position = new Vector3(Screen.width * 2, m_position.y, m_position.z);
    }

    private bool firstActiveFrame = true;

    // Update is called once per frame
    private void Update () {
        if(m_timer > 0)
        {
            m_timer -= Time.deltaTime;
            return;
        }
        if(firstActiveFrame)
        {
            firstActiveFrame = false;
        }
        
        float x = Mathf.Lerp(transform.position.x, m_position.x, ElasticOut(Time.deltaTime) * m_animationSpeed);
        if (x > m_position.x)
        {
            transform.position = new Vector3(x, m_position.y, m_position.z);
        }
	}

    public float ElasticIn(float t)
    {
        if (t == 0.0f || t == 1.0f) return t;

        return -Mathf.Pow(2.0f, 10.0f * (t - 1.0f)) * Mathf.Sin((t - 1.1f) * 5.0f * Mathf.PI);
    }

    public float ElasticOut(float t)
    {
        if (t == 0.0f || t == 1.0f) return t;

        return Mathf.Pow(2.0f, -10.0f * t) * Mathf.Sin((t - 0.1f) * 5.0f * Mathf.PI) + 1.0f;
    }
}