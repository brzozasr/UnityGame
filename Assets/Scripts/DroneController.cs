using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class DroneController : MonoBehaviour
    {
        public GameObject droneBullet;
        public float shootTimeRangeFrom;
        public float shootTimeRangeTo;
        public int hitPoints = 1;
        
        protected Renderer DroneRenderer;
        protected bool IsDroneVisible = false;
        protected Camera MainCamera;

        private void Awake()
        {
            MainCamera = Camera.main;
            DroneRenderer = GetComponent<Renderer>();
            StartCoroutine(Shooting());
        }

        private void Update()
        {
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

        protected IEnumerator Shooting()
        {
            while (true)
            {
                float seconds = Random.Range(shootTimeRangeFrom, shootTimeRangeTo);
                yield return new WaitForSeconds(seconds);

                if (IsDroneVisible)
                {
                    Instantiate(droneBullet, transform.position, Quaternion.identity);
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            // TODO not "Player" but also "PlayerBullet" with ||
            
            if (other.gameObject.CompareTag("Player"))
            {
                hitPoints--;
            }

            if (hitPoints <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}