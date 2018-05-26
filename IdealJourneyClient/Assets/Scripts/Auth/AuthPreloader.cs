using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthPreloader : MenuControlsBase
{
    private void Start()
    {
        StartCoroutine(Preload());
    }

    private IEnumerator Preload()
    {
        bool loggedIn = false;
        var currentLogin = PersistToDeviceHelper.GetCurrentLogin();
        if (currentLogin != null)
        {
            var formFields = new Dictionary<string, string>();
            formFields.Add("authToken", currentLogin.authToken);
            using (var request = UnityWebRequest.Post(AuthConfig.ApiServerRoot + "verify-token", formFields))
            {
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
                    loggedIn = true;
                }
            }
        }

        if (!loggedIn)
        {
            //Fire and forget
            UnityWebRequest.Post(AuthConfig.ApiServerRoot + "poke", new Dictionary<string, string>()).SendWebRequest();

            //Navigate to login screen
            MoveToLoginScreen();
            yield break;
        }

        MoveToTitle();
    }

    public void MoveToLoginScreen()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.LOGIN));
    }

    public void MoveToTitle()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.TITLE));
    }
}
