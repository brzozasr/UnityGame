using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DoorController : MonoBehaviour
    {
        public Material GreenLight;
        public Material RedLight;
        public GameObject Door;
        public Transform DoorPlatform;

        private Animation _doorAnimation;
        private bool _openPrevState;

        private void Start()
        {
            _doorAnimation = Door.GetComponent<Animation>();
            Player.OnPlatformEnter += OpenDoor;
        }

        private void OpenDoor(object sender, EventArgs args)
        {
            var sphere = DoorPlatform.GetChild(0);
            var spotLight = DoorPlatform.GetChild(1);

            sphere.GetComponent<Renderer>().material = GreenLight;
		
            spotLight.GetComponent<Light>().color = Color.green;
		
            _doorAnimation.Play("open");
            FindObjectOfType<AudioManager>().PlaySound("DoorOpen");
        }
    }
}