using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649

[RequireComponent(typeof(Button))]
public class AuthController : MenuControlsBase
{
    [SerializeField]
    private InputField txtUsername;
    [SerializeField]
    private InputField txtPassword;

    private Button btnLogIn;

    [SerializeField]
    private Text errorText;

    public static LoginResponse CurrentAuthToken { get; set; }

    private void Awake()
    {
        if (btnLogIn == null) btnLogIn = GetComponent<Button>();
    }

    public void LogIn()
    {
        var username = txtUsername.text;
        var password = txtPassword.text;

        StartCoroutine(TryLogIn(username, password));
    }

    public void Register()
    {
        Application.OpenURL(AuthConfig.ApiServerRoot + "register");
    }

    public IEnumerator TryLogIn(string username, string password)
    {
        txtUsername.enabled = false;
        txtPassword.enabled = false;
        btnLogIn.enabled = false;

        bool loggedIn = false;
        var formFields = new Dictionary<string, string>();
        formFields.Add("username", username);
        formFields.Add("password", password);
        using (var request = UnityWebRequest.Post(AuthConfig.ApiServerRoot + "login", formFields))
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
                CurrentAuthToken = loginResponse;
                loggedIn = true;
            }
        }

        if (!loggedIn)
        {
            errorText.text = "Failed to log in. Try again.";
            txtUsername.enabled = true;
            txtPassword.enabled = true;
            btnLogIn.enabled = true;
            yield break;
        }
        
        MoveToTitle();
    }

    public void MoveToTitle()
    {
        KeepTitleMusic();
        PersistBackground();
        StartCoroutine(LoadDelayed(SceneNames.TITLE));
    }
}
