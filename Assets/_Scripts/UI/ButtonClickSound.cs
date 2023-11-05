using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    [RequireComponent(typeof(AudioSource), typeof(Button))]
    public class ButtonClickSound : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Button _button;

        private void OnValidate()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _button = gameObject.GetComponent<Button>();
        }

        private void Start()
        {
            _button.onClick.AddListener(PlaySound);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            _audioSource.Play();
        }
    }
}