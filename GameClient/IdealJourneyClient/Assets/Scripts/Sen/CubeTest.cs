using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour {

    [Header("Configuration")]
    [Header("Internal Information")]
    [SerializeField] [Range(1.0f, 20.0f)] private float m_Speed = 20.0f;

    private Vector3 m_RotationDirection = Vector3.up;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(m_RotationDirection * Time.deltaTime * m_Speed);
	}

    public void ToggleRotationDirection()
    {
        Debug.Log("Toggling rotation direction");

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
