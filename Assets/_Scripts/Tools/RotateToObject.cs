using System;
using _Scripts.Enemy;
using _Scripts.Player;
using UnityEngine;

public class RotateToObject : MonoBehaviour
{
    [SerializeField] private Transform _bloodObject;
    public float rayLength = 1f;
    public int maxRotations = 4;
    private int currentRotationCount = 0;
    private bool _goodRotation = false;

    private void Start()
    {
        Destroy(this);
    }

    public void RotateAndCheck()
    {
        RaycastHit hit;
        if (Physics.Raycast(_bloodObject.position, _bloodObject.up, out hit, rayLength))
        {
            if (hit.collider.TryGetComponent(out PlayerController player))
            {
                return;
            }
            if (currentRotationCount < maxRotations)
            {
                _bloodObject.Rotate(Vector3.forward, 90f);
                currentRotationCount++;
                RotateAndCheck();
            }
            else
            {
                _goodRotation = false;
            }
        }
        else
        {
            _goodRotation = true;
            _bloodObject.gameObject.SetActive(true);
        }

        if (!_goodRotation)
        {
            _bloodObject.gameObject.SetActive(false);
        }
    }
}