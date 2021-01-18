using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PointWidgetController : MonoBehaviour
    {
        [Range(1, 50)]
        public int widgetPoints;
        
        private void Update()
        {
            // transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f, Space.World);
            transform.RotateAround (transform.position, Vector3.up, 90 * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().PlaySound("CollectPoints");
                DataStore.AddPointsToScore(widgetPoints);
                Destroy(gameObject);
            }
        }
    }
}