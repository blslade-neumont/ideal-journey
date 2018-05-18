using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCommand
{
    protected AudioSource m_sfx;

    public virtual bool IsComplete()
    {
        return Input.GetKeyUp(KeyCode.Space);
    }

    public abstract void Begin();
    public abstract string AsText();

    public virtual void PlaySFX()
    {
        if (m_sfx != null)
        {
            m_sfx.Play();
        }
    }

    protected void GetCacheSFX(string tag)
    {
        if (m_sfx == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag(tag);
            m_sfx = obj == null ? null : obj.GetComponent<AudioSource>();
        }
    }
}
