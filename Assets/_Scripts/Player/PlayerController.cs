using System.Collections;
using System.Collections.Generic;
using _Scripts.Block;
using _Scripts.Enemy;
using _Scripts.UI;
using _Scripts.Weapons;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _impulceForce;
        [SerializeField] private List<Transform> _disabledTransforms;
        [SerializeField] private float _scalingTime;
        [SerializeField] private AudioSource _deathSound;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerSkinController _skinController;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private float _resDelay;
        [SerializeField] private List<Transform> _respPoints;
        
        private int _mainNumber;
        private int _lastRespPointNumber = 0;
        private PlayerDeathCounter _playerDeathCounter;
        private List<Vector3> _disabledTransformScales = new List<Vector3>();
        private BloodExplosion _bloodExplosion;
        private bool _onGround;
        private bool _isDeath;
        [SerializeField] private int _playerID;
        private Coroutine _resCoroutine;
        private WaitForSeconds _resDelayWait;

        public Weapon CurrentWeapon
        {
            get => _weapon;
            set => _weapon = value;
        }

        public BloodExplosion PlayerBloodExplosion
        {
            get => _bloodExplosion;
            set => _bloodExplosion = value;
        }

        public PlayerSkinController SkinController => _skinController;

        public int PlayerID
        {
            get => _playerID;
            set => _playerID = value;
        }

        public PlayerDeathCounter PlayerDeathCounter
        {
            get => _playerDeathCounter;
            set => _playerDeathCounter = value;
        }

        public List<Transform> RespPoints
        {
            get => _respPoints;
            set => _respPoints = value;
        }

        public int MainNumber
        {
            get => _mainNumber;
            set => _mainNumber = value;
        }

        private void Awake()
        {
            _skinController.SetSkin(0);
            for (int i = 0; i < _disabledTransforms.Count; i++)
            {
                _disabledTransformScales.Add(_disabledTransforms[i].localScale);
            }
            if(!_photonView.IsMine) return;
            _resDelayWait = new WaitForSeconds(_resDelay);
            _rigidbody.isKinematic = false;
        }

        public void MouseDown()
        {
            if(_isDeath) return;
            if(!_photonView.IsMine) return;
            _weapon.StartShot();
        }

        public void MouseUp()
        {
            if(_isDeath) return;
            if(!_photonView.IsMine) return;
            _weapon.StopShot();
        }

        public void Recoil()
        {
            _rigidbody.AddForce(-transform.right * _impulceForce, ForceMode.Impulse);
            if (CheckGrounded())
            {
                float recoilForce = 0;

                if (transform.right.x < .2f && transform.right.x >= 0)
                {
                    recoilForce = _weapon.RecoilForce * .2f;
                }
                else if(transform.right.x > -.2f && transform.right.x <= 0)
                {
                    recoilForce = _weapon.RecoilForce * -.2f;
                }
                else
                {
                    recoilForce = _weapon.RecoilForce * transform.right.x;
                }
                
                _rigidbody.AddExplosionForce(50, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), 2 );
            }
            else
            {
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }
        
        private bool CheckGrounded()
        {
            bool isHit = Physics.CheckSphere(transform.position, _sphereCollider.radius, _groundLayer);

            return isHit;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!_photonView.IsMine) return;
            
            if (other.TryGetComponent(out Trap trap))
            {
                Death();
            }
            
            if (other.TryGetComponent(out Bullet bullet) && bullet.MainPlayer != this && bullet.IsCanDamage)
            {
                bullet.DisableBullet();
                Death();
            }
        }

        private void Res()
        {
            if (_photonView.IsMine)
            {
                for (int i = 0; i < _disabledTransforms.Count; i++)
                {
                    if (_disabledTransforms[i].gameObject.activeInHierarchy)
                    {
                        _disabledTransforms[i].DOScale(_disabledTransformScales[i], _scalingTime);
                    }
                }

                _rigidbody.isKinematic = false;
                _isDeath = false;
            }
        }

        private void StartRes()
        {
            if (_resCoroutine != null)
            {
                StopCoroutine(_resCoroutine);
                _resCoroutine = null;
            }

            _resCoroutine = StartCoroutine(ResCoroutine());
        }

        private IEnumerator ResCoroutine()
        {
            yield return _resDelayWait;
            _photonView.RPC("ResActionRPC", RpcTarget.All);
            int i = 0;
            while (i == _lastRespPointNumber)
            {
                i = Random.Range(0, _respPoints.Count);
            }
            _lastRespPointNumber = i;


            transform.position = _respPoints[i].position;
            yield return _resDelayWait;
            Res();
        }
        
        private void Death()
        {
            if(_isDeath) return;

            if (_photonView.IsMine)
            {
                _playerDeathCounter.Death(_photonView.ViewID);
                _photonView.RPC("DeathActionRPC", RpcTarget.All);
                _isDeath = true;
                _weapon.StopShot();
    
                for (int i = 0; i < _disabledTransforms.Count; i++)
                {
                    if (_disabledTransforms[i].gameObject.activeInHierarchy)
                    {
                        _disabledTransforms[i].DOScale(Vector3.zero, _scalingTime);
                    }
                }
    
                _rigidbody.isKinematic = true;
                StartRes();
            }
        }

        [PunRPC]
        private void DeathActionRPC()
        {
            _deathSound.Play();
            _bloodExplosion.Explosion();
        }
        [PunRPC]
        private void ResActionRPC()
        {
            _bloodExplosion.Res();
        }

        public void FirstPlayer()
        {
            _playerDeathCounter.FirstPlayerID = _photonView.ViewID;
            _playerDeathCounter.FirstPlayer = this;
            _mainNumber = 1;
        }

        public void SecondPlayer()
        {
            _playerDeathCounter.SecondPlayerID = _photonView.ViewID;
            _playerDeathCounter.SecondPlayer = this;
            _mainNumber = 2;
        }

        public void FinishAction()
        {
            if (_photonView.IsMine)
            {
                MouseUp();
                _rigidbody.isKinematic = true;
                if (_playerDeathCounter.FirstPlayerDeathCount > _playerDeathCounter.SecondPlayerDeathCount && _mainNumber == 1)
                {
                    global::FinishAction.Finish.Invoke(global::FinishAction.FinishType.Win);
                }
                else  if (_playerDeathCounter.FirstPlayerDeathCount < _playerDeathCounter.SecondPlayerDeathCount && _mainNumber == 2)
                {
                    global::FinishAction.Finish.Invoke(global::FinishAction.FinishType.Win);
                }
                else if (_playerDeathCounter.FirstPlayerDeathCount == _playerDeathCounter.SecondPlayerDeathCount)
                {
                    global::FinishAction.Finish.Invoke(global::FinishAction.FinishType.Draw);
                }
                else
                {
                    global::FinishAction.Finish.Invoke(global::FinishAction.FinishType.Lose);
                }
            }
        }
    }
}