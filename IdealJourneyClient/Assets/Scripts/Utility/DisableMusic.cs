using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class DisableMusic : MonoBehaviour
{
    private Toggle myToggle = null;
    private AudioSource m_titleSrc = null;
    private AudioSource m_optionsSrc
    {
        get
        {
            if (m_titleSrc == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC);
                m_titleSrc = obj == null ? null : obj.GetComponent<AudioSource>();
            }

            return m_titleSrc;
        }
    }

    void Awake()
    {
        myToggle = GetComponent<Toggle>();
        AudioHelper.BGMEnabled = PersistToDeviceHelper.IsBackgroundMusicSavedOn();
        myToggle.isOn = AudioHelper.BGMEnabled;
    }

    void OnDisable()
    {
        AudioHelper.BGMEnabled = myToggle.isOn;
        PersistToDeviceHelper.SaveBackgroundMusicOption(AudioHelper.BGMEnabled);
        if (m_optionsSrc != null) { m_optionsSrc.enabled = AudioHelper.BGMEnabled; }
    }

    public void OnToggleMe()
    {
        AudioHelper.BGMEnabled = myToggle.isOn;
        if (m_optionsSrc != null) { m_optionsSrc.enabled = AudioHelper.BGMEnabled; }
    }
}
