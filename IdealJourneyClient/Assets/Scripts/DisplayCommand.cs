using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DisplayCommand : MonoBehaviour
{
    [SerializeField] GameController m_gameController;
    private Text m_text;

    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    private void Update()
    {
        m_text.text = m_gameController.IsWaitingForAction
                    ? m_gameController.CurrentCommand.AsText()
                    : "Wait for it...";
    }
}
