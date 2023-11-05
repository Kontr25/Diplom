using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Player
{
    public class PlayerSkinController : MonoBehaviour
    {
        [SerializeField] private List<Skin> _skins;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private ParticleSystem _changeSkinEffect;

        public void SetSkin(int skinNumber)
        {
            _changeSkinEffect.Stop();
            _changeSkinEffect.Play();
            for (int i = 0; i < _skins.Count; i++)
            {
                _skins[i].gameObject.SetActive(false);
            }

            Skin activeSkin = _skins[skinNumber];
            activeSkin.gameObject.SetActive(true);
            _playerController.CurrentWeapon = activeSkin.CurrentWeapon;
            _playerController.PlayerBloodExplosion = activeSkin.PlayerBloodExplosion;
        }
    }
}