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

    [SerializeField]
    private string apiServerRoot = "https://ideal-journey.herokuapp.com/";

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
        Application.OpenURL(apiServerRoot + "register");
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
        using (var request = UnityWebRequest.Post(apiServerRoot + "login", formFields))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var jwt = request.downloadHandler.text;
                Debug.Log(jwt);
                //TODO: persist JWT
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
        StartCoroutine(LoadDelayed(SceneNames.TITLE));
    }
}
