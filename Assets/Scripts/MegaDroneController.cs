using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MegaDroneController : SuperDroneController
    {
        private float _posMegaX;
        private float _posMegaY;
        private float _posMegaZ;
        private bool _isMegaCall = false;
        private bool _isMegaMoveUp = true;
        private int _frameMega;
        private int _iteratorMega = 0;
        private void Awake()
        {
            if (_isMegaCall == false)
            {
                _isMegaCall = true;
                var pos = transform.position;
                _posMegaX = pos.x;
                _posMegaY = pos.y;
                _posMegaZ = pos.z;

                _frameMega = Random.Range(0, 31);
            }
            
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            
            StartCoroutine(Shooting());
        }
        
        private void Update()
        {
            if (_iteratorMega >= _frameMega)
            {
                transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
                var pos = transform.position;

                if (pos.y < _posMegaY + maxMoveY && _isMegaMoveUp)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(pos.x, _posMegaY + maxMoveY, pos.z),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(pos.y - (_posMegaY + maxMoveY)) < 0.1f)
                    {
                        _isMegaMoveUp = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_posMegaX, _posMegaY, _posMegaZ),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(_posMegaY - pos.y) < 0.1f)
                    {
                        _isMegaMoveUp = true;
                    }
                }
            }

            // Number of frames delay
            if (_frameMega >= _iteratorMega)
            {
                _iteratorMega++;
            }
            
            if (DroneRenderer.IsVisibleFrom(MainCamera))
            {
                IsDroneVisible = true;
            }
            else
            {
                IsDroneVisible = false;
            }
        }
    }
}