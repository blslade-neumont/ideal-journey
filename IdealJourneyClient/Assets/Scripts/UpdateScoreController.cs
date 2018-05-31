using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UpdateScoreController : MonoBehaviour
{
    [SerializeField]
    private Text m_txtScore;
    [SerializeField]
    private Text m_txtSendStatus;
    [SerializeField]
    private GameObject m_loadingSpinner;

    private Text m_txtNewHighScore;

    private void Awake()
    {
        var score = GameController.LastScore;
        if (m_txtScore != null) m_txtScore.text = "" + score;

        m_txtNewHighScore = GetComponent<Text>();

        if (AuthController.CurrentAuthToken == null || AuthController.CurrentAuthToken.user == null || score <= AuthController.CurrentAuthToken.user.bestScore)
        {
            m_txtNewHighScore.enabled = false;
            if (m_txtSendStatus != null) m_txtSendStatus.enabled = false;
            if (m_loadingSpinner != null) m_loadingSpinner.SetActive(false);
            return;
        }

        AuthController.CurrentAuthToken.user.bestScore = score;
        StartCoroutine(UpdateScore());
    }

    private IEnumerator UpdateScore()
    {
        var worked = false;

        while (!worked)
        {
            if (m_txtSendStatus != null)
            {
                m_txtSendStatus.enabled = true;
                m_txtSendStatus.text = "Sending Score to Server...";
            }
            if (m_loadingSpinner != null) m_loadingSpinner.SetActive(true);

            using (var request = UnityWebRequest.Get(AuthConfig.ApiServerRoot + "api/update-highscore?score=" + AuthController.CurrentAuthToken.user.bestScore))
            {
                request.SetRequestHeader("Authorization", "Bearer " + AuthController.CurrentAuthToken.authToken);
                yield return request.SendWebRequest();
                if (request.isNetworkError || request.isHttpError)
                {
                    Debug.Log(request.error);
                }
                else
                {
                    var jwt = request.downloadHandler.text;
                    var loginResponse = JsonUtility.FromJson<LoginResponse>(jwt);
                    PersistToDeviceHelper.SetCurrentLogin(loginResponse);
                    AuthController.CurrentAuthToken = loginResponse;
                    worked = true;
                }
            }

            if (m_loadingSpinner != null) m_loadingSpinner.SetActive(false);
            if (!worked && m_txtSendStatus != null)
            {
                m_txtSendStatus.text = "Failed to update score on server.";
                yield return new WaitForSeconds(15);
            }
        }

        if (m_txtSendStatus != null) m_txtSendStatus.enabled = false;
    }
}
