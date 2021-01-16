using System;
using System.Collections;
using TMPro;
using UnityEngine;
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
        private GameObject _imgBgCounterGO;
        private GameObject _pointCounterGO;
        private TextMeshProUGUI _hitCounter;

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
            
            Vector3 textPos = MainCamera.WorldToScreenPoint(transform.position);
            _imgBgCounterGO.transform.position = new Vector3(textPos.x, textPos.y + 188, textPos.z);
            _pointCounterGO.transform.position = new Vector3(textPos.x, textPos.y + 188, textPos.z);
            _hitCounter.text = hitPoints.ToString();

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
                Destroy(gameObject);
                Destroy(_pointCounterGO);
                Destroy(_imgBgCounterGO);
            }
        }
        
        private void CreateHitLabel()
        {
            _imgBgCounterGO = new GameObject();
            _imgBgCounterGO.transform.parent = canvas.transform;
            _imgBgCounterGO.AddComponent<Image>();
            Image imageBg = _imgBgCounterGO.GetComponent<Image>();
            imageBg.color = Color.gray;
            var image = imageBg.GetComponent<RectTransform>();
            image.sizeDelta = new Vector2(70, 40);
            
            _pointCounterGO = new GameObject();
            _pointCounterGO.transform.parent = canvas.transform;
            _pointCounterGO.AddComponent<TextMeshProUGUI>();
            _hitCounter = _pointCounterGO.GetComponent<TextMeshProUGUI>();
            _hitCounter.fontSize = 40;
            _hitCounter.fontWeight = FontWeight.Bold;
            _hitCounter.color = new Color(255, 255, 255);
            _hitCounter.alignment = TextAlignmentOptions.Top;
            _hitCounter.font = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            
            _hitCounter.text = hitPoints.ToString();
            var counterSize = _hitCounter.GetComponent<RectTransform>();
            counterSize.sizeDelta = new Vector2(70, 40);
            // counterSize.ForceUpdateRectTransforms();
        }
    }
}