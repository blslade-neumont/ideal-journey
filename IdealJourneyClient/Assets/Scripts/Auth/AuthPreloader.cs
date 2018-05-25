using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CurrentUsernameText : MonoBehaviour
{
    private Text m_text;

    private void Awake()
    {
        m_text = GetComponent<Text>();
    }

    private void Update()
    {
        if (AuthController.CurrentAuthToken == null || AuthController.CurrentAuthToken.user == null) m_text.text = "(Not logged in)";
        else m_text.text = AuthController.CurrentAuthToken.user.username;
    }
}
