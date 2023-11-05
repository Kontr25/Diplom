using System.Collections;
using _Scripts.Player;
using DG.Tweening;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
    public class PlayerDeathCounter : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private TMP_Text _firstUserNameText;
        [SerializeField] private TMP_Text _secondUserNameText;
        [SerializeField] private TMP_Text _counterText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _lastSecondTimerText;
        [SerializeField] private AudioSource _lastSecondSound;
        [SerializeField] private int _gameTime;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private int _firstPlayerID;
        [SerializeField] private int _secondPlayerID;

        private PlayerController _firstPlayer;
        private PlayerController _secondPlayer;
        private Coroutine _timerCoroutine;
        private WaitForSeconds _second;
        private float _halfSecond = .5f;
        private int _firstPlayerDeathCount = 0;
        private int _secondPlayerDeathCount = 0;
        private int _currentGameTime;
        
        private void Start()
        {
            _second = new WaitForSeconds(1);
            
            if (_photonView.IsMine && PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                SetFirstPlayerName();
            }
            else
            {
                SetSecondPlayerName();
            }
        }

        private void SetFirstPlayerName()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccessFirst, OnFailure);
        }

        private void OnGetAccountSuccessFirst(GetAccountInfoResult result)
        {
            _photonView.RPC("OnGetAccountSuccessFirstRPC", RpcTarget.AllBuffered, result.AccountInfo.Username); 
        }

        [PunRPC]
        private void OnGetAccountSuccessFirstRPC(string name)
        {
            _firstUserNameText.text = name;
        }
        
        
        private void SetSecondPlayerName()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccessSecond, OnFailure);
        }

        private void OnGetAccountSuccessSecond(GetAccountInfoResult result)
        {
            _photonView.RPC("OnGetAccountSuccessSecondRPC", RpcTarget.AllBuffered, result.AccountInfo.Username); 
        }

        [PunRPC]
        private void OnGetAccountSuccessSecondRPC(string name)
        {
            _secondUserNameText.text = name;
        }
        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        public int FirstPlayerID
        {
            get => _firstPlayerID;
            set
            {
                _photonView.RPC("FirstPlayerIDRPC", RpcTarget.AllBuffered, value);
            }
        }

        [PunRPC]
        private void FirstPlayerIDRPC(int value)
        {
            _firstPlayerID = value;
        }

        public int SecondPlayerID
        {
            get => _secondPlayerID;
            set
            {
                _photonView.RPC("SecondPlayerIDRPC", RpcTarget.AllBuffered, value);
            }
        }

        public int FirstPlayerDeathCount
        {
            get => _firstPlayerDeathCount;
            set => _firstPlayerDeathCount = value;
        }

        public int SecondPlayerDeathCount
        {
            get => _secondPlayerDeathCount;
            set => _secondPlayerDeathCount = value;
        }

        public PlayerController FirstPlayer
        {
            get => _firstPlayer;
            set => _firstPlayer = value;
        }

        public PlayerController SecondPlayer
        {
            get => _secondPlayer;
            set => _secondPlayer = value;
        }

        [PunRPC]
        private void SecondPlayerIDRPC(int value)
        {
            _secondPlayerID = value;
        }

        public void Death(int playerID)
        {
            _photonView.RPC("UpdateCounterRPC", RpcTarget.AllBuffered, playerID);
        }

        [PunRPC]
        private void UpdateCounterRPC(int playerID)
        {
            if (playerID == _firstPlayerID)
            {
                _secondPlayerDeathCount++;
            }
            else if(playerID == _secondPlayerID)
            {
                _firstPlayerDeathCount++;
            }
            _counterText.text = $"{_firstPlayerDeathCount} : {_secondPlayerDeathCount}";
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_firstPlayerDeathCount);
                stream.SendNext(_secondPlayerDeathCount);
                stream.SendNext(_firstPlayerID);
                stream.SendNext(_secondPlayerID);
            }
            else
            {
                _firstPlayerDeathCount = (int) stream.ReceiveNext();
                _secondPlayerDeathCount = (int) stream.ReceiveNext();
                _firstPlayerID = (int) stream.ReceiveNext();
                _secondPlayerID = (int) stream.ReceiveNext();
            }
        }

        public void StartTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            _currentGameTime = _gameTime;
            while (_currentGameTime > 4)
            {
                _currentGameTime--;
                UpdateTimerText();
                yield return _second;
            }

            for (int i = 0; i < 4; i++)
            {
                _lastSecondTimerText.transform.localScale = Vector3.one;
                _currentGameTime--;
                UpdateTimerText();
                _timerText.gameObject.SetActive(false);
                _lastSecondTimerText.gameObject.SetActive(true);
                _lastSecondTimerText.transform.DOScale(.5f, _halfSecond).onComplete = () =>
                {
                    _lastSecondSound.Play();
                };
                yield return _second;
            }
            
            

            if (_firstPlayer != null)
            {
                _firstPlayer.FinishAction();
            }

            if (_secondPlayer != null)
            {
                _secondPlayer.FinishAction();
            }
        }

        private void UpdateTimerText()
        {
            _timerText.text = $"{_currentGameTime}";
            _lastSecondTimerText.text = $"{_currentGameTime}";
            
        }
    }
}