using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerBulletController : MonoBehaviour
    {
        public GameObject explosion;
        public float Speed;
        public int TimeToDestroy;

        private void Start()
        {
            Destroy(this.gameObject, TimeToDestroy);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Instantiate(explosion, transform.position, transform.rotation);
                FindObjectOfType<AudioManager>().PlaySound("BulletHitPlayer");
                Destroy(gameObject);
            }
            
            if (!other.gameObject.CompareTag("Player"))
                Destroy(gameObject);
        }
    }
}