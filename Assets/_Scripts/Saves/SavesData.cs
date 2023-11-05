using System;
using UnityEngine;

namespace _Scripts.Saves
{
    public class SavesData : MonoBehaviour
    {
        public static SavesData Instance;
        
        private const string _currentSkinKey = "CurrentSkin";

        public int CurrentSkin
        {
            get
            {
                if (PlayerPrefs.HasKey(_currentSkinKey))
                {
                    return PlayerPrefs.GetInt(_currentSkinKey);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                PlayerPrefs.SetInt(_currentSkinKey, value);
            }
        }

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
    }
}