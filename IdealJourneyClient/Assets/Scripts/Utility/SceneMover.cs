using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMover : MenuControlsBase
{
    public void MoveToTitle()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.TITLE));
    }

    public void MoveToEnd()
    {
        DontKeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.END, 1.25f));
    }

    public void MoveToOptions()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.OPTIONS));
    }

    public void MoveToCredits()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.CREDITS));
    }

    public void MoveToHighScore()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.HIGHSCORE));
    }

    public void MoveToGame()
    {
        DontKeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.GAME));
    }

    public void QuitGame()
    {
        StartCoroutine(QuitDelayed());
    }

    public void Logout()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.LOGIN));
    }

    private IEnumerator QuitDelayed()
    {
        Debug.Log("Quitting");
        return InvokeDelayed(0.25f, () => Application.Quit());
    }
}
