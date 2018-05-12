using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAccel : MonoBehaviour {

    private Text m_text;

    private void Start()
    {
        m_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update () {
        m_text.text = Input.acceleration.ToString();
    }
}
