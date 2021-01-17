using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        private GameObject _imgBgSuperCounterGO;
        private GameObject _pointSuperCounterGO;
        private TextMeshProUGUI _hitSuperCounter;
        
        private Bounds _meshSuper;
        private float _droneSuperWidth;
        private float _droneSuperHeight;

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
            
            CreateHitLabel();
            _superDroneAudioManager = FindObjectOfType<AudioManager>();
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            
            StartCoroutine(Shooting());
            
            _meshSuper = FindObjectOfType<MeshCollider>().bounds;
            GetDroneHeightAndWidth();
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
            
            GetDroneHeightAndWidth();
            
            Vector3 textPos = MainCamera.WorldToScreenPoint(transform.position);
            _imgBgSuperCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneSuperHeight, textPos.z);
            _pointSuperCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneSuperHeight, textPos.z);
            _hitSuperCounter.text = hitPoints.ToString();

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
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBullet"))
            {
                hitPoints--;
            }

            if (hitPoints <= 0)
            {
                Instantiate(droneExplosion, transform.position, transform.rotation);
                _superDroneAudioManager.PlaySound("DroneExplosion");
                Destroy(gameObject);
                Destroy(_pointSuperCounterGO);
                Destroy(_imgBgSuperCounterGO);
            }
        }
        
        private void CreateHitLabel()
        {
            _imgBgSuperCounterGO = new GameObject();
            _imgBgSuperCounterGO.transform.parent = canvas.transform;
            _imgBgSuperCounterGO.AddComponent<Image>();
            Image imageBg = _imgBgSuperCounterGO.GetComponent<Image>();
            imageBg.color = Color.gray;
            var image = imageBg.GetComponent<RectTransform>();
            image.sizeDelta = new Vector2(30, 15);
            
            _pointSuperCounterGO = new GameObject();
            _pointSuperCounterGO.transform.parent = canvas.transform;
            _pointSuperCounterGO.AddComponent<TextMeshProUGUI>();
            _hitSuperCounter = _pointSuperCounterGO.GetComponent<TextMeshProUGUI>();
            _hitSuperCounter.fontSize = 15;
            _hitSuperCounter.fontWeight = FontWeight.Bold;
            _hitSuperCounter.color = new Color(255, 255, 255);
            _hitSuperCounter.alignment = TextAlignmentOptions.Top;
            _hitSuperCounter.font = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            
            _hitSuperCounter.text = hitPoints.ToString();
            var counterSize = _hitSuperCounter.GetComponent<RectTransform>();
            counterSize.sizeDelta = new Vector2(30, 15);
            // counterSize.ForceUpdateRectTransforms();
            
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                imageBg.enabled = false;
                _hitSuperCounter.enabled = false;
            }
        }
        
        /// <summary>
        /// Get height and width of the drone depend on the game screen.
        /// </summary>
        private void GetDroneHeightAndWidth()
        {
            Vector3 posStart = MainCamera.WorldToScreenPoint(new Vector3(_meshSuper.min.x, _meshSuper.min.y, _meshSuper.min.z));
            Vector3 posEnd = MainCamera.WorldToScreenPoint(new Vector3(_meshSuper.max.x, _meshSuper.max.y, _meshSuper.min.z));
 
            _droneSuperWidth = (posEnd.x - posStart.x) / 2 + (posEnd.x - posStart.x) * 0.45f;
            _droneSuperHeight = (posEnd.y - posStart.y) / 2 + (posEnd.y - posStart.y) * 0.45f;;
        }
    }
}