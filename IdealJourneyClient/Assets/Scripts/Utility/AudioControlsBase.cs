using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControlsBase : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(0.25f);

    protected IEnumerator LoadDelayed(string name)
    {
        return InvokeDelayed(() => SceneManager.LoadScene(name));
    }
    protected IEnumerator InvokeDelayed(Action act)
    {
        yield return wait;
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
}
