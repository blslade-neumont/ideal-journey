using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HighscoreController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_loadingSpinner;
    [SerializeField]
    private Text m_txtGlobalUsernames;
    [SerializeField]
    private Text m_txtGlobalScores;
    [SerializeField]
    private Text m_txtPersonalScore;

    private void Awake()
    {
        this.RefreshPersonalHighscore();
        StartCoroutine(this.LoadGlobalHighscores());
    }

    private void RefreshPersonalHighscore()
    {
        var currentLogin = PersistToDeviceHelper.GetCurrentLogin();
        if (currentLogin == null) return; //WTF?

        this.m_txtPersonalScore.text = "Best: " + currentLogin.user.bestScore;
    }

    private IEnumerator LoadGlobalHighscores()
    {
        if (this.m_loadingSpinner != null) this.m_loadingSpinner.SetActive(true);
        if (this.m_txtGlobalUsernames != null) this.m_txtGlobalUsernames.enabled = false;
        if (this.m_txtGlobalScores != null) this.m_txtGlobalScores.enabled = false;

        User[] globalHighscores;
        using (var request = UnityWebRequest.Get(AuthConfig.ApiServerRoot + "api/highscores"))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                yield break;
            }
            else
            {
                var value = request.downloadHandler.text;
                var response = JsonUtility.FromJson<HighscoresResponse>(value);
                globalHighscores = response.highscores;
            }
        }
        if (globalHighscores.Length > 5) globalHighscores = globalHighscores.Take(5).ToArray();

        if (this.m_loadingSpinner != null) this.m_loadingSpinner.SetActive(false);
        if (this.m_txtGlobalUsernames != null)
        {
            this.m_txtGlobalUsernames.enabled = true;
            this.m_txtGlobalUsernames.text = string.Join("\r\n", globalHighscores.Select(user => user.username).ToArray());
        }
        if (this.m_txtGlobalScores != null)
        {
            this.m_txtGlobalScores.enabled = true;
            this.m_txtGlobalScores.text = string.Join("\r\n", globalHighscores.Select(user => user.bestScore.ToString()).ToArray());
        }
    }
}
