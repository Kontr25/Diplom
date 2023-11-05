using DG.Tweening;
using UnityEngine;

namespace _Scripts.Environment
{
    public class BlockBlood : MonoBehaviour
    {
        [SerializeField] private float _scalingTime = .2f;
        [SerializeField] private MeshRenderer _mesh;

        private void OnValidate()
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>(); 
        }

        public void Activate(Material material)
        {
            _mesh.material = material;
            _mesh.enabled = true;
            _mesh.transform.DOScale(Vector3.one * 1.6f, _scalingTime);
        }
    }
}