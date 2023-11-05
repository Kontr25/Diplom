using System;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private const string _authGuidKey = "auth_guid_key";

    private bool _isConnected = false;
    private LoginWithCustomIDRequest request;
    private string id;

    public bool IsConnected => _isConnected;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "7B6D0";
        }

        _isConnected = true;

        /*var needCreation = PlayerPrefs.HasKey(_authGuidKey);
        id = PlayerPrefs.GetString(_authGuidKey, Guid.NewGuid().ToString());
        
        request = new LoginWithCustomIDRequest { CustomId = id, CreateAccount = !needCreation };
        
        Connect();*/
    }

    private void OnConnectButtonClick()
    {
        if(!_isConnected) Connect();
    }

    private void Connect()
    {
        PlayFabClientAPI.LoginWithCustomID(request, result =>
        {
            PlayerPrefs.SetString(_authGuidKey, id);
            OnLoginSuccess(result);
        }, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result)
    {
        _isConnected = true;
        Debug.Log("Congratulations, you made successful API call!");
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
