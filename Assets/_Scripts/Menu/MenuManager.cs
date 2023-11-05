using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Menu
{
    public class MenuManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField _createInput;
        [SerializeField] private TMP_InputField _joinInput;
        [SerializeField] private Button _createButton;
        [SerializeField] private Button _joinButton;
        [SerializeField] private TMP_Text _cantJoinRoomText;
        [SerializeField] private TMP_Text _victoryCountText;

        private string _playFabID;

        private void Start()
        {
            _createButton.onClick.AddListener(CreateRoom);
            _joinButton.onClick.AddListener(JoinRoom);
            SetPlayFabId();
        }

        private void OnDestroy()
        {
            _createButton.onClick.RemoveListener(CreateRoom);
            _joinButton.onClick.RemoveListener(JoinRoom);
        }

        private void CreateRoom()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(_createInput.text, roomOptions);
        }

        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(_joinInput.text);
        }

        public override void OnJoinedRoom()
        {
            if (_cantJoinRoomText.gameObject.activeInHierarchy)
            {
                _cantJoinRoomText.gameObject.SetActive(false);
            }
            PhotonNetwork.LoadLevel("Game");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            _cantJoinRoomText.gameObject.SetActive(true);
            _cantJoinRoomText.text = $"There is no such room";
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            _cantJoinRoomText.gameObject.SetActive(true);
            _cantJoinRoomText.text = $"Such a room already exists";
        }
        private void SetUserData()
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest
                {
                    Data = new Dictionary<string, string>
                    {
                        {"victory_count", 0.ToString()}
                    }
                },
                result =>
                {
                    Debug.Log("Set user data succes");
                    _victoryCountText.text = $"Number of wins: 0";
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
                PlayFabId = _playFabID
            }, result =>
            {
                if (result.Data.ContainsKey(keyData))
                {
                    _victoryCountText.text = $"Number of wins: {result.Data[keyData].Value}";
                }
                else
                {
                    SetUserData();
                }
            }, ErrorMessage);
        }
    
        private void SetPlayFabId()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest
            {
    
            }, result =>
            {
                _playFabID = result.AccountInfo.PlayFabId;
                SetVictoryCount("victory_count");
            }, ErrorMessage);
        }
    }
}