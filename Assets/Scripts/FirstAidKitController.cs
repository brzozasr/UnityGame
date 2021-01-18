using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FirstAidKitController : MonoBehaviour
    {
        [Range(1, 150)]
        public int hitPointRecovery;
        public static event Action<int> OnFirstAidCollected;

        private void Update()
        {
            transform.Rotate(Vector3.up, 100.0f * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().PlaySound("TakingFirstAid");
                int hp = DataStore.AddHpPoints(hitPointRecovery);
                OnFirstAidCollected?.Invoke(hp);
                Destroy(gameObject);
            }
        }
    }
}