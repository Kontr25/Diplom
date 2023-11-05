using System.Collections;
using _Scripts.UI;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private UIMover[] _uiMovers;
    [SerializeField] private float _delayBetweenMove;
    [SerializeField] private GameObject[] _objectForDisable;
    [SerializeField] private AudioSource _defeatSound;
    [SerializeField] private OverlayImage _overlayImage;
    [SerializeField] private Button _menuButton;

    private WaitForSeconds _delay;

    private void Start()
    {
        _menuButton.onClick.AddListener(GoMenu);
        _delay = new WaitForSeconds(_delayBetweenMove);
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
        _defeatSound.Play();
        StartCoroutine(MoveUI());
        _overlayImage.FadeImage();
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
