using System.Collections;
using _Scripts.Player;
using _Scripts.PoolObject;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float _recoilForce;
        [SerializeField] private PlayerController _player;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _shotPoint;
        [SerializeField] private Transform _container;
        [SerializeField] private int _pollCapacity;
        [SerializeField] private Transform _recoilPoint;
        [SerializeField] private float _recoilDuration;
        [SerializeField] private float _delayBetweenShots;
        [SerializeField] private Pool _poolBullet;
        [SerializeField] private PhotonView _photonView;

        private Coroutine _shotCoroutine;
        private WaitForSeconds _shotDelay;
        private Sequence _sequence;
        private Vector3 _defaultLocalPosition;

        public float RecoilForce => _recoilForce;

        private void Awake()
        {
            if (!_photonView.IsMine) return;
            SetPool();
            _shotDelay = new WaitForSeconds(_delayBetweenShots);
            _defaultLocalPosition = transform.localPosition;
        }

        private void SetPool()
        {
             _poolBullet.SetPool();
        }

        public void StartShot()
        {
            StopShot();
            _shotCoroutine = StartCoroutine(ShotCoroutine());
        }

        public void StopShot()
        {
            if (_shotCoroutine != null)
            {
                StopCoroutine(_shotCoroutine);
                _shotCoroutine = null;
            }
        }

        private IEnumerator ShotCoroutine()
        {
            while (true)
            {
                Shot();
                yield return _shotDelay;
            }
        }

        public void Shot()
        {
            if (_photonView.IsMine)
            { 
                var bullet = _poolBullet.GetFreeElement();
                 Recoil();
                _player.Recoil();
                
                bullet.transform.SetParent(_container);
                bullet.transform.localPosition = Vector3.zero;
                bullet.transform.localRotation = Quaternion.identity;
                bullet.transform.SetParent(null);
                bullet.StartShot();
            }
        }

        private void Recoil()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }

            _sequence = DOTween.Sequence();
            _sequence.Append(transform.DOLocalMove(_recoilPoint.localPosition, _recoilDuration/2)).SetEase(Ease.Linear);
            _sequence.Append(transform.DOLocalMove(_defaultLocalPosition, _recoilDuration)).SetEase(Ease.Linear);
        }
    }
}
