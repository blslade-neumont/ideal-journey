using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ButtonSFX : MonoBehaviour
{
    private AudioSource m_sfxToPlay;

    private void Awake()
    {
        m_sfxToPlay = GetComponent<AudioSource>();
    }

    public void PlaySFX()
    {
        AudioHelper.PlaySFX(m_sfxToPlay);
    }
}
