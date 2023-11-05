using System.Collections.Generic;
using _Scripts.Player;
using _Scripts.UI;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace _Scripts.Photon
{
    public class PhotonPlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private PlayerController _firstPlayerPrefab;
        [SerializeField] private PlayerController _secondPlayerPrefab;
        [SerializeField] private CinemachineVirtualCamera _playerCamera;
        [SerializeField] private EventTrigger _inputTrigger;
        [SerializeField] private PlayerDeathCounter _playerDeathCounter;
        [SerializeField] private WaitOpponentWindow _waitOpponentWindow;
        [SerializeField] private PhotonView _photonView;
        
        private PlayerController _player;
        private int _lastPoint = 0;

        private void Start()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                SpawnPlayer(_firstPlayerPrefab);
            }
            else
            {
                SpawnPlayer(_secondPlayerPrefab);
                _photonView.RPC("DisableWaitWindowRPC", RpcTarget.AllBuffered);
            }
        }

        [PunRPC]
        private void DisableWaitWindowRPC()
        {
            _waitOpponentWindow.Disable();
            _playerDeathCounter.StartTimer();
        }
        
        public void OnPointerDownDelegate(PointerEventData data)
        {
            _player.MouseDown();
        }
        
        public void OnPointerUpDelegate(PointerEventData data)
        {
            _player.MouseUp();
        }

        private void SpawnPlayer(PlayerController playerPrefab)
        {
            int i = 0;
            while (i == _lastPoint)
            {
                i = Random.Range(0, _spawnPoints.Count);
            }
            _lastPoint = i;

            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, _spawnPoints[i].position, Quaternion.identity);
            _player = player.GetComponent<PlayerController>();
            _player.PlayerDeathCounter = _playerDeathCounter;
            _player.RespPoints = _spawnPoints;
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                _player.FirstPlayer();
            }
            else
            {
                _player.SecondPlayer();
            }

            Transform playerTransform = _player.transform;
            _playerCamera.Follow = playerTransform;
            _playerCamera.LookAt = playerTransform;
            SetEventTrigger();
        }

        private void SetEventTrigger()
        {
            EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
            entryPointerDown.eventID = EventTriggerType.PointerDown;
            entryPointerDown.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
            _inputTrigger.triggers.Add(entryPointerDown);
            
            EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
            entryPointerUp.eventID = EventTriggerType.PointerUp;
            entryPointerUp.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
            _inputTrigger.triggers.Add(entryPointerUp);
        }
    }
}