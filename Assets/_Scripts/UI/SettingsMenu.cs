using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Image _closePanel;
    [SerializeField] private Transform _defaultButtonsPosition;
    [SerializeField] private float _openTime = 1f;
    
    [Header("Buttons")]
    [SerializeField] private Button _soundButton;
    [SerializeField] private Button _clearSavesButton;
    [SerializeField] private Button _changeLangugeButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private GameObject _ruIcon;
    [SerializeField] private GameObject _engIcon;
    [SerializeField] private Sprite _enabledSoundSprite;
    [SerializeField] private Sprite _disabledSoundSprite;

    [SerializeField] private Image[] _buttonImages;
    [SerializeField] private TMP_Text[] _buttonTexts;
    
    [Space]
    
    [Header("Points")]
    [SerializeField] private List<Transform> _pointsForButtons;

    private bool _fpsEnabled = false;
    private int _attempt;
    private bool _isOpen = false;
    private bool _volumeEnable = true;
    private bool _vibroEnable = true;
    private Sequence _closePanelSequence;

    public void SwitchSettingsMenu()
    {
        if (_isOpen == false)
        {
            for (int i = 0; i < _buttonImages.Length; i++)
            {
                _buttonImages[i].DOFade(1f, _openTime - .4f);
            }

            for (int i = 0; i < _buttonTexts.Length; i++)
            {
                _buttonTexts[i].DOFade(1f, _openTime - .4f);
            }
            
            _soundButton.transform.DOMove(_pointsForButtons[0].transform.position, _openTime);
            _clearSavesButton.transform.DOMove(_pointsForButtons[1].transform.position, _openTime);
            _changeLangugeButton.transform.DOMove(_pointsForButtons[2].transform.position, _openTime);
            _closePanel.gameObject.SetActive(true);

            UIRaycasted(true);
            
            TryKillClosePanelSequence();
            _closePanel.DOFade(.30f, _openTime);
            _closePanel.raycastTarget = true;
        }
        else
        {
            for (int i = 0; i < _buttonImages.Length; i++)
            {
                _buttonImages[i].DOFade(0f, _openTime - .4f);
            }

            for (int i = 0; i < _buttonTexts.Length; i++)
            {
                _buttonTexts[i].DOFade(0f, _openTime - .4f);
            }
            
            _soundButton.transform.DOMove(_defaultButtonsPosition.position, _openTime);
            _clearSavesButton.transform.DOMove(_defaultButtonsPosition.position, _openTime);
            _changeLangugeButton.transform.DOMove(_defaultButtonsPosition.position, _openTime);
            UIRaycasted(false);

            TryKillClosePanelSequence();
            _closePanelSequence.Insert(0, _closePanel.DOFade(0, _openTime));
            _closePanel.raycastTarget = false;
        }

        _isOpen = !_isOpen;
    }

    private void UIRaycasted(bool state)
    {
        _soundButton.enabled = state;
        _clearSavesButton.enabled = state;
        _changeLangugeButton.enabled = state;
        _restartButton.enabled = state;
        _closePanel.raycastTarget = state;
    }

    private void TryKillClosePanelSequence()
    {
        if (_closePanelSequence != null)
        {
            _closePanelSequence.Kill();
            _closePanelSequence = null;
        }

        _closePanelSequence = DOTween.Sequence();
    }

    public void SwitchSound()
    {
        _volumeEnable = !_volumeEnable;

        if (_volumeEnable == false)
        {
            AudioListener.volume = 0f;
            _soundButton.image.sprite = _disabledSoundSprite;
        }
        else
        {
            AudioListener.volume = 1f;
            _soundButton.image.sprite = _enabledSoundSprite;
        }
    }
}