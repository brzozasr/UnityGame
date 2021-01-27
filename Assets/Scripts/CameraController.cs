using UnityEngine;

namespace DefaultNamespace
{
    public class CameraController : MonoBehaviour
    {
        public Transform Player;
        public float Speed;
        private Transform _camera;
        
        private void Start()
        {
            FindObjectOfType<AudioManager>().PlaySound("BackgroundMusic");
            _camera = transform.GetChild(0);
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.position, Speed);
        }
    }
}