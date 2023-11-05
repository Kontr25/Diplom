using System;
using System.Collections.Generic;
using _Scripts.Player;
using _Scripts.Saves;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SkinWindow : MonoBehaviour
    {
        [SerializeField] private List<SkinCell> _allSkins;
        private PlayerSkinController _playerSkinController;
        [SerializeField] private Button _closeButton;
        [SerializeField] private UIMover _uiMver;

        public PlayerSkinController SkinController
        {
            get => _playerSkinController;
            set => _playerSkinController = value;
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(CloseWindow);
            _uiMver.Move();
            ChangeSkin();
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(CloseWindow);
        }

        public void ChangeSkin()
        {
            for (int i = 0; i < _allSkins.Count; i++)
            {
                if (_allSkins[i].IsClicked)
                {
                    _playerSkinController.SetSkin(i);
                }
                _allSkins[i].CheckCurrentSkin();
            }
        }

        private void CloseWindow()
        {
            _uiMver.MoveBack();
        }
    }
}