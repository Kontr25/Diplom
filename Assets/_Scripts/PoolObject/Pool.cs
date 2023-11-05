using System;
using System.Collections.Generic;
using _Scripts.Player;
using _Scripts.Weapons;
using Photon.Pun;
using UnityEngine;

namespace _Scripts.PoolObject
{
    public class Pool: MonoBehaviourPun
    {
        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _capacity;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private PlayerController _player;
        [SerializeField] private List<Bullet> _pool;

        public List<Bullet> PoolList
        {
            get => _pool;
            set => _pool = value;
        }

        public void SetPool()
        {
            if (!_photonView.IsMine) enabled = false;
            
            CreatePool(_capacity);
        }

        private void CreatePool(int capacity)
        {
            _pool = new List<Bullet>();
            for (int i = 0; i < capacity; i++)
            {
                CreateObject();
            }
        }

        private void CreateObject()
        {
            GameObject GO = PhotonNetwork.Instantiate(_prefab.name, Vector3.zero, Quaternion.identity);
            Bullet createdObject = GO.GetComponent<Bullet>();
            
            Transform objectTransform = createdObject.transform;
            objectTransform.SetParent(_container);
            objectTransform.localPosition = Vector3.zero;
            objectTransform.localRotation = Quaternion.identity;
            
            _pool.Add(createdObject);
            createdObject.MainPool = this;
            createdObject.MainPlayer = _player;
        }

        public bool HasFreeElement(out Bullet element)
        {
            foreach (var obj in _pool)
            {
                if (obj.IsFree)
                {
                    element = obj;
                    return true;
                }
            }
            element = null;
            return false;
        }

        public Bullet GetFreeElement()
        {
            if (this.HasFreeElement(out var element))
            {
                return element;
            }

            throw new Exception($"There is no free element in pool of type {typeof(Bullet)}");
        }
    }
}