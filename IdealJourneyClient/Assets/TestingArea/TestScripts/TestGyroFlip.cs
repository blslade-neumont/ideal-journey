using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGyroFlip : MonoBehaviour {

    private bool m_listen;
    private Quaternion m_targetFlipRot;
    private Gyroscope m_gyro;
    [SerializeField] private Text m_text;
    private Vector3 m_target;
    [SerializeField, Range(0, 90)] private float m_tolerance;

	// Use this for initialization
	void Start () {
        m_gyro = Input.gyro;
        m_gyro.enabled = true;
        m_listen = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (m_listen)
        {
            Vector3 currentForward = (m_gyro.attitude * Vector3.forward).normalized;
            float angle = Mathf.Acos(Vector3.Dot(m_target, currentForward));
            if (angle < Mathf.Deg2Rad * m_tolerance)
            {
                m_text.text = "OMG A FLIP";
                m_listen = false;
            }
        }
	}

    public void Flip()
    {
        m_text.text = "";

        m_target = (m_gyro.attitude * Vector3.back).normalized;
        
        m_listen = true;
    }
}
