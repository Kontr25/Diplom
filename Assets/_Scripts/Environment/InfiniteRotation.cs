using UnityEngine;

namespace _Scripts.Environment
{
    public class InfiniteRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 100.0f;
        [SerializeField] private Rigidbody _rb;

        private void FixedUpdate()
        {
            _rb.angularVelocity = new Vector3(0, 0, rotationSpeed);
        }
    }
}