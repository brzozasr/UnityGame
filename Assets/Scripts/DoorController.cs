using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class DoorController : MonoBehaviour
    {
        public string compatibleChipName;
        public int requiredCompatibleChipNameCount;
        public bool isGameOverChip;
        public Material greenLight;
        public Material redLight;
        public GameObject door;
        public Transform doorPlatform;
        

        private Animation _doorAnimation;

        private void Start()
        {
            _doorAnimation = door.GetComponent<Animation>();
            Player.OnPlatformEnter += OpenDoor;
        }

        public void OpenDoor(object sender, EventArgs args)
        {
            if (DataStore.GetItemQuantityFromInventory(compatibleChipName) >= requiredCompatibleChipNameCount)
            {
                var sphere = doorPlatform.GetChild(0);
                var spotLight = doorPlatform.GetChild(1);

                sphere.GetComponent<Renderer>().material = greenLight;

                spotLight.GetComponent<Light>().color = Color.green;

                _doorAnimation.Play("open");
                FindObjectOfType<AudioManager>().PlaySound("DoorOpen");

                if (isGameOverChip)
                {
                    DataStore.IsWonGameOver = true;
                }
            }
        }
    }
}