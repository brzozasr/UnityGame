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
                float destPos;
                
                if (transform.position.x > playerTransform.position.x)
                {
                    destPos = playerTransform.position.x - 150;
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    destPos = playerTransform.position.x + 150;
                }
                else
                {
                    destPos = playerTransform.position.x;
                }
                
                _bullet.transform.position = Vector3.MoveTowards(_bullet.transform.position,
                    new Vector3(destPos, transform.position.y, transform.position.z), 
                    bulletSpeed * Time.deltaTime);

                if (Math.Abs(_bullet.transform.position.x - destPos) < 25)
                {
                    Destroy(_bullet);
                }
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