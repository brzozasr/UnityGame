using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class DoorController : MonoBehaviour, IDoorController
    {
        public string compatibleChipName;
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

        public void OpenDoor(object sender, string chip)
        {
            if (compatibleChipName.Equals(chip))
            {
                var sphere = doorPlatform.GetChild(0);
                var spotLight = doorPlatform.GetChild(1);

                sphere.GetComponent<Renderer>().material = greenLight;

                spotLight.GetComponent<Light>().color = Color.green;

                _doorAnimation.Play("open");
                FindObjectOfType<AudioManager>().PlaySound("DoorOpen");

                if (chip.Equals("Chip2"))
                {
                    if (SceneManager.sceneCountInBuildSettings - 1 == SceneManager.GetActiveScene().buildIndex)
                    {
                        DataStore.IsWonGameOver = true;
                    }
                    else
                    {
                        DataStore.IsLevelOver = true;
                    }
                }
            }
        }
    }
}