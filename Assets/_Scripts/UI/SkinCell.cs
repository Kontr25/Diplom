using System;
using _Scripts.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SkinCell : MonoBehaviour
    {
        [SerializeField] private SkinWindow _skinWindow;
        [SerializeField] private int _skinNumber;
        [SerializeField] private Button _changeSkinButton;
        [SerializeField] private GameObject _currentSkinIcon;

        [SerializeField] private bool _isClicked = false;

        public bool IsClicked
        {
            get => _isClicked;
            set => _isClicked = value;
        }

        private void Start()
        {
            _changeSkinButton.onClick.AddListener(ChangeSkin);
            CheckCurrentSkin();
        }

        private void OnDestroy()
        {
            _changeSkinButton.onClick.RemoveListener(ChangeSkin);
        }

        public void CheckCurrentSkin()
        {
            if (SavesData.Instance.CurrentSkin == _skinNumber)
            {
                _changeSkinButton.gameObject.SetActive(false);
                _currentSkinIcon.SetActive(true);
                _isClicked = true;
            }
            else
            {
                _changeSkinButton.gameObject.SetActive(true);
                _currentSkinIcon.SetActive(false);
                _isClicked = false;
            }
        }

        public void ChangeSkin()
        {
            SavesData.Instance.CurrentSkin = _skinNumber;
            CheckCurrentSkin();
            _skinWindow.ChangeSkin();
        }
    }
}