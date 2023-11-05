using Photon.Pun;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class BulletManager : MonoBehaviourPun, IPunObservable
    {
        private bool _isCanMove = false;
        private bool _isFree = true;
        private bool _meshGOEnabled = false;
        private Vector3 _position;
        public bool IsCanMove
        {
            get => _isCanMove;
            set => _isCanMove = value;
        }

        public bool IsFree
        {
            get => _isFree;
            set => _isFree = value;
        }

        public bool MeshGoEnabled
        {
            get => _meshGOEnabled;
            set => _meshGOEnabled = value;
        }

        public Vector3 Position
        {
            get => _position;
            set => _position = value;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_isCanMove);
                stream.SendNext(_isFree);
                stream.SendNext(_meshGOEnabled);
                stream.SendNext(_position);
            }
            else
            {
                _isCanMove = (bool) stream.ReceiveNext();
                _isFree = (bool) stream.ReceiveNext();
                _meshGOEnabled = (bool) stream.ReceiveNext();
                _position = (Vector3) stream.ReceiveNext();
            }
        }
    }
}