using System.Collections;
using Cinemachine;
using UnityEngine;

namespace _Scripts.Camera
{
    public class CameraEffects : MonoBehaviour
    {
        public static CameraEffects Instance;

        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private float _intensity;
        [SerializeField] private float _time;
        private float _shakeTimer;
        private Coroutine _shakeCoroutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Shake()
        {
            if (_shakeCoroutine != null)
            {
                StopCoroutine(_shakeCoroutine);
            }

            _shakeCoroutine = StartCoroutine(ShakeCamera());
        }

        private IEnumerator ShakeCamera()
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _intensity;
            _shakeTimer = _time;

            while (_shakeTimer > 0)
            {
                yield return null;
                _shakeTimer -= Time.deltaTime;
            }

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            _shakeCoroutine = null;
        }
    }
}