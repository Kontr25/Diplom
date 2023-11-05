using System.Collections;
using _Scripts.Block;
using _Scripts.Player;
using _Scripts.PoolObject;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class Bullet : MonoBehaviourPun
    {
        [SerializeField] private GameObject _meshGameObject;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private PlayerController _mainPlayer;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private float _speed;
        [SerializeField] private float _disableTime;
        [SerializeField] private Pool _mainPool;
        [SerializeField] private PhotonTransformViewClassic _photonTransformViewClassic;
        [SerializeField] private AudioSource _shotSound;
        
        
        private Coroutine _shotCoroutine;
        private Coroutine _disableCoroutine;
        private WaitForSeconds _disableDelay;
        private bool _isCanMove = false;
        [SerializeField]  private bool _isFree = true;
        private bool _isCanDamage = false;
        private Vector3 _currentShotPosition;
        

        public PlayerController MainPlayer
        {
            get => _mainPlayer;
            set => _mainPlayer = value;
        }

        public Pool MainPool
        {
            get => _mainPool;
            set => _mainPool = value;
        }

        public bool IsFree => _isFree;

        public bool IsCanDamage => _isCanDamage;

        private void Start()
        {
            if (!_photonView.IsMine) return;
            _disableDelay = new WaitForSeconds(_disableTime);
        }

        private void StartDisable()
        {
            _photonView.RPC("PlayExplosionEffectRPC", RpcTarget.All);
            if(!_photonView.IsMine) return;
            
            StopDisable();
            _disableCoroutine = StartCoroutine(DisableCoroutine());
        }

        [PunRPC]
        private void PlayExplosionEffectRPC()
        {
            _explosionEffect.Play();
            _isCanDamage = false;
        }

        private IEnumerator DisableCoroutine()
        {
            yield return _disableDelay;
            _photonView.RPC("IsFreeRPC", RpcTarget.All, true);
        }

        private void StopDisable()
        {
            if (_disableCoroutine != null)
            {
                StopCoroutine(_disableCoroutine);
                _disableCoroutine = null;
            }
        }

        public void StartShot()
        {
            if (_photonView.IsMine)
            {
                _shotSound.Play();
                _photonView.RPC("IsFreeRPC", RpcTarget.All, false);
                StopShotCoroutine();
                _isCanMove = true;
                _rigidbody.isKinematic = false;
                _shotCoroutine = StartCoroutine(ShotCoroutine());
                _currentShotPosition = transform.position;
            }
            _photonView.RPC("EnableMeshRPC", RpcTarget.All, true);
        }

        private void StopShot()
        {
            if (_photonView.IsMine)
            {
                _rigidbody.isKinematic = true;
                _isCanMove = false;
                StopShotCoroutine();
            }
            _photonView.RPC("EnableMeshRPC", RpcTarget.All, false);
        }

        private void StopShotCoroutine()
        {
            if (_shotCoroutine != null)
            {
                StopCoroutine(_shotCoroutine);
                _shotCoroutine = null;
            }
        }

        [PunRPC]
        private void EnableMeshRPC(bool state)
        {
            if (state)
            {
                _photonTransformViewClassic.m_PositionModel.SynchronizeEnabled = false;
                transform.localPosition = _currentShotPosition;
            }
            _meshGameObject.SetActive(state);

            if (state)
            {
                _photonTransformViewClassic.m_PositionModel.SynchronizeEnabled = true;
            }
        }

        [PunRPC]
        private void IsFreeRPC(bool state)
        {
            if (state)
            {
                transform.position = new Vector3(100,0,0);
            }
            else
            {
                _isCanDamage = true;
            }

            _isFree = state;
        }

        private IEnumerator ShotCoroutine()
        {
            while (_isCanMove)
            {
                _rigidbody.velocity = transform.right * _speed;
                yield return new WaitForFixedUpdate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if(_isFree || !_photonView.IsMine) return;

            if (other.TryGetComponent(out DestroyableBlock block))
            {
                block.DestroyBlock();
            }

            if (other.TryGetComponent(out BlockBloodController blockblk))
            {
                DisableBullet();
            }
        }

        public void DisableBullet()
        {
            StopShot();
            StartDisable();
        }
    }
}