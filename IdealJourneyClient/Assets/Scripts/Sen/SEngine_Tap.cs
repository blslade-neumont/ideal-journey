using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEngine_Tap : MonoBehaviour
{

    [Header("Configuration")]
    [Header("Internal Information")]
    [SerializeField] [Range(0.0f, 20.0f)] private float m_speed = 20.0f;

    // Private internal information
	private Vector3 m_RotationDirection = Vector3.up;

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(m_RotationDirection * Time.deltaTime * m_speed);
    }

    public void ToggleRotationDirection()
    {
        Debug.Log("Tap rotation direction");

        if (m_RotationDirection == Vector3.up) 
		{
			m_RotationDirection = Vector3.down;
		}
		else 
		{
			m_RotationDirection = Vector3.up;
        }
    }
}
