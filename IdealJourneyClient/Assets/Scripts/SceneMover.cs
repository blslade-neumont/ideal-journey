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
        StartCoroutine(LoadDelayed(SceneNames.TITLE));
    }

    public void MoveToEnd()
    {
        DontKeepTitleMusic();
        StartCoroutine(LoadDelayed(SceneNames.END));
    }

    public void MoveToOptions()
    {
        KeepTitleMusic();
        StartCoroutine(LoadDelayed(SceneNames.OPTIONS));
    }

    public void MoveToHighScore()
    {
        KeepTitleMusic();
        StartCoroutine(LoadDelayed(SceneNames.HIGHSCORE));
    }

    public void MoveToGame()
    {
        DontKeepTitleMusic();
        StartCoroutine(LoadDelayed(SceneNames.GAME));
    }

    public void QuitGame()
    {
        StartCoroutine(QuitDelayed());
    }

    public void Logout()
    {
        KeepTitleMusic();
        StartCoroutine(LoadDelayed(SceneNames.AUTH));
    }

    private IEnumerator QuitDelayed()
    {
        Debug.Log("Quitting");
        return InvokeDelayed(() => Application.Quit());
    }
}
