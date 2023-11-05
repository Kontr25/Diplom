using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class DrawnWindow : MonoBehaviour, IFinishable
    {
        [SerializeField] private UIMover[] _uiMovers;
        [SerializeField] private float _delayBetweenMove;
        [SerializeField] private GameObject[] _objectForDisable;
        [SerializeField] private AudioSource _victorySound;
        [SerializeField] private OverlayImage _overlayImage;
        [SerializeField] private Button _menuButton;

        private WaitForSeconds _delay;

        private void Start()
        {
            _delay = new WaitForSeconds(_delayBetweenMove);
            _menuButton.onClick.AddListener(GoMenu);
        }
        
        private void OnDestroy()
        {
            _menuButton.onClick.RemoveListener(GoMenu);
        }

        public void StartActionOnWin()
        {

        }

        public void StartActionOnLose()
        {

        }

        public void StartActionOnDrawn()
        {
            _victorySound.Play();
            StartCoroutine(MoveUI());
            _overlayImage.FadeImage();
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
}