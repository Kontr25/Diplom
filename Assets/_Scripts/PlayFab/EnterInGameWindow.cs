using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PlayFab
{
    public class EnterInGameWindow: MonoBehaviour
    {
        [SerializeField] private Button _signInButton;
        [SerializeField] private Button _createAccountButton;
        [SerializeField] private Canvas _enterInGameCanvas;
        [SerializeField] private Canvas _createAccounCanvas;
        [SerializeField] private Canvas _signInCanvas;
        [SerializeField] private List<Button> _backButtons;

        private void Start()
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "BD09E";
            }
            _signInButton.onClick.AddListener(OpenSignInWindow);
            _createAccountButton.onClick.AddListener(OpenCreateAccounWindow);
            for (int i = 0; i < _backButtons.Count; i++)
            {
                _backButtons[i].onClick.AddListener(Back);
            }
        }

        private void OnDestroy()
        {
            _signInButton.onClick.RemoveListener(OpenSignInWindow);
            _createAccountButton.onClick.RemoveListener(OpenCreateAccounWindow);
            for (int i = 0; i < _backButtons.Count; i++)
            {
                _backButtons[i].onClick.RemoveListener(Back);
            }
        }

        private void OpenSignInWindow()
        {
            _signInCanvas.enabled = true;
            _createAccounCanvas.enabled = false;
            _enterInGameCanvas.enabled = false;
        }

        private void OpenCreateAccounWindow()
        {
            _createAccounCanvas.enabled = true;
            _enterInGameCanvas.enabled = false;
            _signInCanvas.enabled = false;
            
        }

        private void Back()
        {
            _signInCanvas.enabled = false;
            _createAccounCanvas.enabled = false;
            _enterInGameCanvas.enabled = true;
        }
    }
}