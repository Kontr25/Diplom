using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Block
{
    public class BlockBloodController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _bloodParticle;

        private void Start()
        {
            int i = Random.Range(0, _bloodParticle.Count);
            _bloodParticle[i].SetActive(true);
        }
    }
}