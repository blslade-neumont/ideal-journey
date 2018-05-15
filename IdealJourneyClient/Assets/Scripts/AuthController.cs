using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#pragma warning disable 0649

[RequireComponent(typeof(Button))]
public class AuthController : MenuControlsBase
{
    [SerializeField]
    private InputField txtEmail;
    [SerializeField]
    private InputField txtPassword;

    private Button btnLogIn;

    [SerializeField]
    private Text errorText;

    private void Awake()
    {
        if (btnLogIn == null) btnLogIn = GetComponent<Button>();
    }

    public void LogIn()
    {
        var email = txtEmail.text;
        var password = txtPassword.text;

        StartCoroutine(TryLogIn(email, password));
    }

    public IEnumerator TryLogIn(string email, string password)
    {
        txtEmail.enabled = false;
        txtPassword.enabled = false;
        btnLogIn.enabled = false;

        yield return new WaitForSeconds(.5f);

        if (email != "abc" || password != "123")
        {
            errorText.text = "Failed to log in. Try again.";
            txtEmail.enabled = true;
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
