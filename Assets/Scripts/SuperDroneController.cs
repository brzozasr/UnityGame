using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SuperDroneController : DroneController
    {
        public float maxMoveY;
        public float moveSpeed;

        private Transform _currentTransform;
        private float _posSuperX;
        private float _posSuperY;
        private float _posSuperZ;
        private bool _isSuperCall = false;
        private bool _isSuperMoveUp = true;
        private int _frameSuper;
        private int _iteratorSuper = 0;
        
        private AudioManager _superDroneAudioManager;

        private void Awake()
        {
            if (_isSuperCall == false)
            {
                _isSuperCall = true;
                var pos = transform.position;
                _posSuperX = pos.x;
                _posSuperY = pos.y;
                _posSuperZ = pos.z;

                _frameSuper = Random.Range(0, 31);
            }
            
            _superDroneAudioManager = FindObjectOfType<AudioManager>();
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            
            StartCoroutine(Shooting());
        }

        private void Update()
        {
            if (_iteratorSuper >= _frameSuper)
            {
                transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
                var pos = transform.position;

                if (pos.y < _posSuperY + maxMoveY && _isSuperMoveUp)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(pos.x, _posSuperY + maxMoveY, pos.z),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(pos.y - (_posSuperY + maxMoveY)) < 0.1f)
                    {
                        _isSuperMoveUp = false;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_posSuperX, _posSuperY, _posSuperZ),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(_posSuperY - pos.y) < 0.1f)
                    {
                        _isSuperMoveUp = true;
                    }
                }
            }

            // Number of frames delay
            if (_frameSuper >= _iteratorSuper)
            {
                _iteratorSuper++;
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

        private IEnumerator Shooting()
        {
            while (true)
            {
                float seconds = Random.Range(shootTimeRangeFrom, shootTimeRangeTo);
                yield return new WaitForSeconds(seconds);

                if (IsDroneVisible)
                {
                    Instantiate(droneBullet, transform.position, Quaternion.identity);
                    _superDroneAudioManager.PlaySound("DroneShot");
                }
            }
        }
    }
}