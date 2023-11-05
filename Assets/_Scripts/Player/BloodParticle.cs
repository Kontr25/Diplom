using _Scripts.Enemy;
using _Scripts.Environment;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Player
{
    public class BloodParticle : MonoBehaviour
    {
        [SerializeField] private float _scalingTime = .2f;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private ParticleSystem _trail;
        private Material _material;
        private bool _isActivate = false;

        private void OnValidate()
        {
            _trail = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            _material = _meshRenderer.material;
            var mainModule = _trail.main;
            mainModule.startColor = _material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BlockBlood block))
            {
                block.Activate(_material);
                transform.DOScale(Vector3.zero, _scalingTime);
            }
        }
    }
}