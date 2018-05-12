using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAccel : MonoBehaviour {

    private float speed = 1;
    private Text m_text;

    private void Start()
    {
        m_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 dir = Vector3.zero;
        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;
        AccelerationEvent[] events = Input.accelerationEvents;

        foreach (var item in events)
        {
            if (item.acceleration.ToString() != Vector3.back.ToString())
            {
                Debug.Log("Start");
                m_text.text = item.acceleration.ToString();

                Debug.Log("End");
            }
        }
        if (dir.sqrMagnitude > 1)
            dir.Normalize();
        //Debug.Log(dir.x);
        //Debug.Log(dir.z);
        dir *= Time.deltaTime;
        transform.Translate(dir * speed);

    }
}
