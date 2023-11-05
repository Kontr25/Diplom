using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayFabLogin _playFabLogin;
    private Coroutine _checkPlayfabConnect;
    private const float _checkDelay = .5f;
    private WaitForSeconds _checkDelayWait;
    void Start()
    {
        _checkDelayWait = new WaitForSeconds(_checkDelay);
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    public override void OnConnectedToMaster()
    {
        if (_checkPlayfabConnect != null)
        {
            StopCoroutine(_checkPlayfabConnect);
            _checkPlayfabConnect = null;
        }

        _checkPlayfabConnect = StartCoroutine(CheckPlayfabConnect());
    }

    private IEnumerator CheckPlayfabConnect()
    {
        while (!_playFabLogin.IsConnected)
        {
            yield return _checkDelayWait;
        }
        SceneManager.LoadScene(1);
    }
}
