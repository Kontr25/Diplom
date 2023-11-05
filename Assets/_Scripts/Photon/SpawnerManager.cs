using Photon.Pun;

namespace _Scripts.Photon
{
    public class SpawnerManager : MonoBehaviourPun, IPunObservable
    {
        private int _playerCount;

        public int PlayerCount
        {
            get => _playerCount;
            set => _playerCount = value;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_playerCount);
            }
            else
            {
                _playerCount = (int) stream.ReceiveNext();
            }
        }
    }
}