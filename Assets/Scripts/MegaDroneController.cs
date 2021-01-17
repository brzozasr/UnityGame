using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        private GameObject _imgBgMegaCounterGO;
        private GameObject _pointMegaCounterGO;
        private TextMeshProUGUI _hitMegaCounter;
        
        private Bounds _meshMega;
        private float _droneMegaWidth;
        private float _droneMegaHeight;
        
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
            
            CreateHitLabel();
            _megaDroneAudioManager = FindObjectOfType<AudioManager>();
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            
            StartCoroutine(Shooting());
            
            _meshMega = FindObjectOfType<MeshCollider>().bounds;
            GetDroneHeightAndWidth();
            
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
            
            GetDroneHeightAndWidth();
            Vector3 textPos = MainCamera.WorldToScreenPoint(transform.position);
            _imgBgMegaCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneMegaHeight, textPos.z);
            _pointMegaCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneMegaHeight, textPos.z);
            _hitMegaCounter.text = hitPoints.ToString();
            
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
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBullet"))
            {
                hitPoints--;
            }

            if (hitPoints <= 0)
            {
                Instantiate(droneExplosion, transform.position, transform.rotation);
                _megaDroneAudioManager.PlaySound("DroneExplosion");
                Destroy(gameObject);
                Destroy(_pointMegaCounterGO);
                Destroy(_imgBgMegaCounterGO);
            }
        }
        
        private void CreateHitLabel()
        {
            _imgBgMegaCounterGO = new GameObject();
            _imgBgMegaCounterGO.transform.parent = canvas.transform;
            _imgBgMegaCounterGO.AddComponent<Image>();
            Image imageBg = _imgBgMegaCounterGO.GetComponent<Image>();
            imageBg.color = Color.gray;
            var image = imageBg.GetComponent<RectTransform>();
            image.sizeDelta = new Vector2(30, 15);
            
            _pointMegaCounterGO = new GameObject();
            _pointMegaCounterGO.transform.parent = canvas.transform;
            _pointMegaCounterGO.AddComponent<TextMeshProUGUI>();
            _hitMegaCounter = _pointMegaCounterGO.GetComponent<TextMeshProUGUI>();
            _hitMegaCounter.fontSize = 15;
            _hitMegaCounter.fontWeight = FontWeight.Bold;
            _hitMegaCounter.color = new Color(255, 255, 255);
            _hitMegaCounter.alignment = TextAlignmentOptions.Top;
            _hitMegaCounter.font = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            
            _hitMegaCounter.text = hitPoints.ToString();
            var counterSize = _hitMegaCounter.GetComponent<RectTransform>();
            counterSize.sizeDelta = new Vector2(30, 15);
            // counterSize.ForceUpdateRectTransforms();
            
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                imageBg.enabled = false;
                _hitMegaCounter.enabled = false;
            }
        }
        
        /// <summary>
        /// Get height and width of the drone depend on the game screen.
        /// </summary>
        private void GetDroneHeightAndWidth()
        {
            Vector3 posStart = MainCamera.WorldToScreenPoint(new Vector3(_meshMega.min.x, _meshMega.min.y, _meshMega.min.z));
            Vector3 posEnd = MainCamera.WorldToScreenPoint(new Vector3(_meshMega.max.x, _meshMega.max.y, _meshMega.min.z));
 
            _droneMegaWidth = (posEnd.x - posStart.x) / 2 + (posEnd.x - posStart.x) * 0.55f;
            _droneMegaHeight = (posEnd.y - posStart.y) / 2 + (posEnd.y - posStart.y) * 0.55f;;
        }
    }
}