using _Scripts.Enemy;
using _Scripts.Weapons;
using UnityEngine;

namespace _Scripts.Player
{
    public class Skin : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private BloodExplosion _playerBloodExplosion;

        public Weapon CurrentWeapon => _weapon;

        public BloodExplosion PlayerBloodExplosion => _playerBloodExplosion;
    }
}