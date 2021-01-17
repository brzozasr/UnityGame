using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class DroneController : MonoBehaviour
    {
        public GameObject droneBullet;
        public GameObject droneExplosion;
        public Canvas canvas;
        public float shootTimeRangeFrom;
        public float shootTimeRangeTo;
        public int hitPoints = 1;
        
        protected Renderer DroneRenderer;
        protected bool IsDroneVisible = false;
        protected Camera MainCamera;
        private AudioManager _droneAudioManager;
        private GameObject _imgBgCounterGO;
        private GameObject _pointCounterGO;
        private TextMeshProUGUI _hitCounter;

        private Bounds _mesh;
        private float _droneWidth;
        private float _droneHeight;

        private void Awake()
        {
            CreateHitLabel();
            _droneAudioManager = FindObjectOfType<AudioManager>();
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            StartCoroutine(Shooting());
            
            _mesh = FindObjectOfType<MeshCollider>().bounds;
            GetDroneHeightAndWidth();
        }

        private void Update()
        {
            GetDroneHeightAndWidth();
            
            Vector3 textPos = MainCamera.WorldToScreenPoint(transform.position);
            _imgBgCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneHeight, textPos.z);
            _pointCounterGO.transform.position = new Vector3(textPos.x, textPos.y + _droneHeight, textPos.z);
            _hitCounter.text = hitPoints.ToString();
            
            transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);

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
                    _droneAudioManager.PlaySound("DroneShot");
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
                _droneAudioManager.PlaySound("DroneExplosion");
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
            image.sizeDelta = new Vector2(30, 15);
            
            _pointCounterGO = new GameObject();
            _pointCounterGO.transform.parent = canvas.transform;
            _pointCounterGO.AddComponent<TextMeshProUGUI>();
            _hitCounter = _pointCounterGO.GetComponent<TextMeshProUGUI>();
            _hitCounter.fontSize = 15;
            _hitCounter.fontWeight = FontWeight.Bold;
            _hitCounter.color = new Color(255, 255, 255);
            _hitCounter.alignment = TextAlignmentOptions.Top;
            _hitCounter.font = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
            
            _hitCounter.text = hitPoints.ToString();
            var counterSize = _hitCounter.GetComponent<RectTransform>();
            counterSize.sizeDelta = new Vector2(30, 15);
            // counterSize.ForceUpdateRectTransforms();

            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                imageBg.enabled = false;
                _hitCounter.enabled = false;
            }
        }

        /// <summary>
        /// Get height and width of the drone depend on the game screen.
        /// </summary>
        private void GetDroneHeightAndWidth()
        {
            Vector3 posStart = MainCamera.WorldToScreenPoint(new Vector3(_mesh.min.x, _mesh.min.y, _mesh.min.z));
            Vector3 posEnd = MainCamera.WorldToScreenPoint(new Vector3(_mesh.max.x, _mesh.max.y, _mesh.min.z));
 
            _droneWidth = (posEnd.x - posStart.x) / 2 + (posEnd.x - posStart.x) * 0.08f;
            _droneHeight = (posEnd.y - posStart.y) / 2 + (posEnd.y - posStart.y) * 0.08f;;
        }
    }
}