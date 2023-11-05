using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Environment
{
    public class BackController : MonoBehaviour
    {
        public static BackController Instance;
        
        [SerializeField] private List<Mesh> _meshes;
        [SerializeField] private List<MeshFilter> _meshFilters;
        [SerializeField] private List<MeshRenderer> _meshRenderers;
        [SerializeField] private List<Material> _blueMaterials;
        [SerializeField] private List<Material> _orangeMaterials;
        [SerializeField] private List<Material> _greenMaterials;

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
            
            int meshNumber = Random.Range(0, _meshes.Count);
            for (int i = 0; i < _meshFilters.Count; i++)
            {
                _meshFilters[i].mesh = _meshes[meshNumber];
            }
        }

        public void SetColor(LevelColor color)
        {
            switch (color)
            {
                case LevelColor.Blue:
                    SetMaterials(_blueMaterials);
                    break;
                case LevelColor.Green:
                    SetMaterials(_greenMaterials);
                    break;
                case LevelColor.Orange:
                    SetMaterials(_orangeMaterials);
                    break;
            }
        }

        private void SetMaterials(List<Material> materials)
        {
            for (int i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].materials = materials.ToArray();
            }
        }
    }
}