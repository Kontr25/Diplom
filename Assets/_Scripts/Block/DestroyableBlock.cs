using _Scripts.Camera;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Block
{
    public class DestroyableBlock : MonoBehaviour
    {
        [SerializeField] private float _scalingTime;
        [SerializeField] private Transform _blockTransform;
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private BoxCollider _boxCollider;
        [SerializeField] private AudioSource _destroySound;

        public void DestroyBlock()
        {
            _destroySound.Play();
            _boxCollider.enabled = false;
            _explosionEffect.Play();
            CameraEffects.Instance.Shake();
            _blockTransform.DOScale(Vector3.zero, _scalingTime).onComplete = () =>
            {
                _blockTransform.gameObject.SetActive(false);
            };
        }
    }
}