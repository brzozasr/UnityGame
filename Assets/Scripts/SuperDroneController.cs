using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class SuperDroneController : MonoBehaviour
    {
        public GameObject droneBullet;
        public float bulletSpeed;
        public float shootTimeRangeFrom;
        public float shootTimeRangeTo;
        public int hitPoints = 1;
        public static SuperDroneController Instance;

        private void Awake()
        {
            Instance = this;
            StartCoroutine(Shooting());
        }

        private void Update()
        {
            transform.Rotate(Vector3.up, 180.0f * Time.deltaTime);
        }

        IEnumerator Shooting()
        {
            while (true)
            {
                float seconds = Random.Range(shootTimeRangeFrom, shootTimeRangeTo);
                yield return new WaitForSeconds(seconds);
                
                Instantiate(droneBullet, transform.position, Quaternion.identity);
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