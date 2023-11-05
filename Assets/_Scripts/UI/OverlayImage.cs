using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class OverlayImage : MonoBehaviour
    {
        [SerializeField] private Image _overlayImage;
        [SerializeField] private float _fadeInDuration;

        private bool _isFade;

        public float FadeInDuration => _fadeInDuration;
        private void Awake()
        {
            FadeImage();
        }

        public void FadeImage()
        {
            if (_isFade)
            {
                _isFade = false;
                _overlayImage.DOFade(.8f, _fadeInDuration);
            }
            else
            {
                _isFade = true;
                _overlayImage.DOFade(0f, _fadeInDuration);
            }
        }
    }
}