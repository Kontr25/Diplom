using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Enemy
{
    public class BloodExplosion : MonoBehaviour
    {
        [SerializeField] private List<Rigidbody> _bloodParticles;
        [SerializeField] private float _explosionForceMin;
        [SerializeField] private float _explosionForceMax;
        [SerializeField] private float _explosionRadius;
        private List<Vector3> _defaultPositions = new List<Vector3>();
        private List<Vector3> _defaultScale = new List<Vector3>();
        private List<Quaternion> _defaultRotation = new List<Quaternion>();

        private void Start()
        {
            for (int i = 0; i < _bloodParticles.Count; i++)
            {
                _defaultPositions.Add(_bloodParticles[i].transform.localPosition);
                _defaultScale.Add(_bloodParticles[i].transform.localScale);
                _defaultRotation.Add(_bloodParticles[i].transform.localRotation);
            }
        }

        public void Explosion()
        {
            for (int i = 0; i < _bloodParticles.Count; i++)
            {
                _bloodParticles[i].isKinematic = false;
                _bloodParticles[i].gameObject.SetActive(true);
                float explosionForce = Random.Range(_explosionForceMin, _explosionForceMax);
                _bloodParticles[i].AddExplosionForce(explosionForce, transform.position, _explosionRadius);
                _bloodParticles[i].AddTorque(transform.forward * Random.Range(-1, 2) * 50);
            }
        }

        public void Res()
        {
            for (int i = 0; i < _bloodParticles.Count; i++)
            {
                _bloodParticles[i].isKinematic = true;
                _bloodParticles[i].transform.localPosition = _defaultPositions[i];
                _bloodParticles[i].transform.localScale = _defaultScale[i];
                _bloodParticles[i].transform.localRotation = _defaultRotation[i];
                _bloodParticles[i].gameObject.SetActive(false);
            }
        }
    }
}