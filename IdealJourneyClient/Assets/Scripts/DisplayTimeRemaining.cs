using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayTimeRemaining : MonoBehaviour
{
    [SerializeField] GameController m_gameController;
    private Text m_text;

    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    private void Update()
    {
        m_text.text = string.Concat("Time Remaining: ", m_gameController.TimeLeft.ToString("0.00"));
    }
}
