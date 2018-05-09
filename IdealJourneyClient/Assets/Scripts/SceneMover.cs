using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMover : MonoBehaviour
{
    private WaitForSeconds wait = new WaitForSeconds(0.25f);

    private IEnumerator LoadDelayed(string name)
    {
        yield return wait;
        SceneManager.LoadScene(name);
    }

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

    private void KeepTitleMusic()
    {
        GameObject titleMusic = GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC);
        if (titleMusic != null) { DontDestroyOnLoad(titleMusic); }
    }

    private void DontKeepTitleMusic()
    {
        GameObject titleMusic = GameObject.FindGameObjectWithTag(Tags.TITLE_MUSIC);
        if (titleMusic != null) { Destroy(titleMusic); }
    }

    public void QuitGame()
    {
        StartCoroutine(QuitDelayed());
    }

    private IEnumerator QuitDelayed()
    {
        yield return wait;
        Debug.Log("Quitting");
        Application.Quit();
    }
}
