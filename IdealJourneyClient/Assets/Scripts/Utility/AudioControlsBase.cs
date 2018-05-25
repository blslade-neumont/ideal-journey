using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControlsBase : MonoBehaviour
{
    protected IEnumerator LoadDelayed(string name, float timeToWait = 0.25f)
    {
        return InvokeDelayed(timeToWait, () => SceneManager.LoadScene(name));
    }
    protected IEnumerator InvokeDelayed(float timeToWait, Action act)
    {
        yield return new WaitForSeconds(timeToWait);
        act();
    }

    protected void KeepTitleMusic()
    {
        GameObject titleMusic = GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC);
        if (titleMusic != null) { DontDestroyOnLoad(titleMusic); }
    }

    protected void DontKeepTitleMusic()
    {
        GameObject titleMusic = GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC);
        if (titleMusic != null) { Destroy(titleMusic); }
    }

    protected void PersistBackground()
    {
        GameObject backgroundObject = GameObject.FindGameObjectWithTag(Tags.BACKGROUND_OBJECT);
        if (backgroundObject != null) { DontDestroyOnLoad(backgroundObject); }
    }
}
