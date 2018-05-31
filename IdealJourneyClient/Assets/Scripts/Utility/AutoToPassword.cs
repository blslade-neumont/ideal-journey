using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoToPassword : MonoBehaviour {

    private EventSystem m_eventSystem;
    [SerializeField] private GameObject m_password;

    private void Start()
    {
        m_eventSystem = EventSystem.current;
    }

    public void ToPassword()
    {
        if (!m_eventSystem.alreadySelecting)
        {
            m_eventSystem.SetSelectedGameObject(m_password);
        }
    }
}
