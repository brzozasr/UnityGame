using System;
using System.Collections;
using UnityEngine;
using Random = System.Random;

namespace DefaultNamespace
{
    public class DroneController : MonoBehaviour
    {
        public GameObject droneBullet;
        public Transform playerTransform;
        public float bulletSpeed;

        private GameObject _bullet;
        private void Awake()
        {
            StartCoroutine(Shooting());
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);

            if (_bullet != null)
            {
                _bullet.transform.position = Vector3.MoveTowards(_bullet.transform.position,
                    new Vector3(playerTransform.position.x, transform.position.y, transform.position.z), 
                    bulletSpeed * Time.deltaTime);
            }
        }

        IEnumerator Shooting()
        {
            Random random = new Random();

            while (true)
            {
                float seconds = random.Next(8, 18);
                yield return new WaitForSeconds(seconds);
                
                _bullet = Instantiate(droneBullet, transform.position, Quaternion.identity);
            }
        }
    }
}