using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class SuperDroneController : DroneController
    {
        public float maxMoveY;
        public float moveSpeed;

        private Transform _currentTransform;
        private float _posX;
        private float _posY;
        private float _posZ;
        private bool _isCall = false;
        private bool _isMoveUp = true;

        private void Start()
        {
            if (_isCall == false)
            {
                _isCall = true;
                var pos = transform.position;
                _posX = pos.x;
                _posY = pos.y;
                _posZ = pos.z;
            }
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
            var pos = transform.position;
            
            if (pos.y < _posY + maxMoveY && _isMoveUp)
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(pos.x, _posY + maxMoveY, pos.z), 
                    moveSpeed * Time.deltaTime);
                if (Math.Abs(pos.y - (_posY + maxMoveY)) < 0.1f)
                {
                    _isMoveUp = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,
                    new Vector3(_posX, _posY, _posZ), 
                    moveSpeed * Time.deltaTime);
                if (Math.Abs(_posY - pos.y) < 0.1f)
                {
                    _isMoveUp = true;
                }
            }
            
        }
    }
}