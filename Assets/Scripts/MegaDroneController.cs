using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class MegaDroneController : SuperDroneController
    {
        public float maxMoveX;
        
        private float _posMegaX;
        private float _posMegaY;
        private float _posMegaZ;
        private bool _isMegaCall = false;
        private int _moveHorizontal;
        private int _frameMega;
        private int _iteratorMega = 0;
        
        private Transform _playerTransform;
        private float _destPosX;
        
        private AudioManager _megaDroneAudioManager;
        
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
                _moveHorizontal = Random.Range(0, 2);
            }
            
            _megaDroneAudioManager = FindObjectOfType<AudioManager>();
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            
            StartCoroutine(Shooting());
            
            _playerTransform = GetPlayerTransform();
                
            if (transform.position.x >= _playerTransform.position.x)
            {
                _destPosX = _posMegaX - maxMoveX;
            }
            else
            {
                _destPosX = _posMegaX + maxMoveX;
            }
        }
        
        private void Update()
        {
            if (_iteratorMega >= _frameMega)
            {
                transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
                var pos = transform.position;

                if (_moveHorizontal == 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(pos.x, _posMegaY + maxMoveY, pos.z),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(pos.y - (_posMegaY + maxMoveY)) < 0.1f)
                    {
                        _moveHorizontal = 2;
                    }
                }
                else if (_moveHorizontal == 1)
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_destPosX ,pos.y, pos.z),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(pos.x - _destPosX) <= 0.1f)
                    {
                        _moveHorizontal = 2;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(_posMegaX, _posMegaY, _posMegaZ),
                        moveSpeed * Time.deltaTime);
                    if (Math.Abs(_posMegaY - pos.y) < 0.1f && Math.Abs(_posMegaX - pos.x) < 0.1f)
                    {
                        _moveHorizontal = Random.Range(0, 2);
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

        private IEnumerator Shooting()
        {
            while (true)
            {
                float seconds = Random.Range(shootTimeRangeFrom, shootTimeRangeTo);
                yield return new WaitForSeconds(seconds);

                if (IsDroneVisible)
                {
                    Instantiate(droneBullet, transform.position, Quaternion.identity);
                    _megaDroneAudioManager.PlaySound("DroneShot");
                }
            }
        }
        
        private Transform GetPlayerTransform()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            
            if (player != null)
            {
                _playerTransform = player.transform;
            }

            return _playerTransform;
        }
    }
}