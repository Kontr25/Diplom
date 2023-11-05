using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.UI;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private UIMover[] _uiMovers;
    [SerializeField] private float _delayBetweenMove;
    [SerializeField] private GameObject[] _objectForDisable;
    [SerializeField] private AudioSource _victorySound;
    [SerializeField] private OverlayImage _overlayImage;
    [SerializeField] private Button _menuButton;
    private int _victoryCount;

    private WaitForSeconds _delay;

    private void Start()
    {
        _menuButton.onClick.AddListener(GoMenu);
        _delay = new WaitForSeconds(_delayBetweenMove);
        SetVictoryCount("victory_count");
    }

    private void OnDestroy()
    {
        _menuButton.onClick.RemoveListener(GoMenu);
    }

    public void StartActionOnWin()
    {
        SetUserData();
        _victorySound.Play();
        StartCoroutine(MoveUI());
        _overlayImage.FadeImage();
    }

    private void SetUserData()
    {
        _victoryCount++;
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    {"victory_count", _victoryCount.ToString()}
                }
            },
            result =>
            {
                Debug.Log($"Set user data succes and victory count now = {_victoryCount + 1}");
                
            }, ErrorMessage);
    }

    private void  ErrorMessage(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError(errorMessage);
    }

    private void SetVictoryCount(string keyData)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest
        {
            PlayFabId = PlayFabId()
        }, result =>
        {
            if (result.Data.ContainsKey(keyData))
            {
                _victoryCount = int.Parse(result.Data[keyData].Value);
                Debug.Log( $"_victoryCount = {_victoryCount}");
            }
        }, ErrorMessage);
    }

    private string PlayFabId()
    {
        string playfabId = "";
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
        {

        }, result =>
        {
            playfabId = result.AccountInfo.PlayFabId;
            Debug.Log( $"Playfab ID = {playfabId}");
        }, ErrorMessage);
        return playfabId;
    }

    public void StartActionOnLose()
    {
        
    }

    public void StartActionOnDrawn()
    {
        
    }

    public void GoMenu()
    {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(2);
    }

    private IEnumerator MoveUI()
    {
        for (int i = 0; i < _objectForDisable.Length; i++)
        {
            _objectForDisable[i].SetActive(false);
        }
        for (int i = 0; i < _uiMovers.Length; i++)
        {
            _uiMovers[i].Move();
            yield return _delay;
        }
    }
}
