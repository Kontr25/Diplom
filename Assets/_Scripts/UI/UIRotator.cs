using UnityEngine;

namespace _Scripts.UI
{
    public class UIRotator : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 10f;

        void Update()
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed, Space.Self);
        }
    }
}