using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class UIMover : MonoBehaviour
    {
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private float _moveDuration;

        private Vector3 _defaultPosition;
        private Sequence _sequence;

        private void Start()
        {
            _defaultPosition = transform.position;
            transform.position = new Vector3(transform.position.x, transform.position.y + 2000, transform.position.z);
        }

        public void Move()
        {
            transform.position = _defaultPosition;
            TryKillSequence();
            _sequence.Insert(0,transform.DOMove(_targetPoint.position, _moveDuration));
        }

        public void MoveBack()
        {
            TryKillSequence();
            _sequence.Insert(0, transform.DOMove(_defaultPosition, _moveDuration));
        }

        private void TryKillSequence()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }

            _sequence = DOTween.Sequence();
        }
    }
}