using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class MainMenuController : MonoBehaviour
    {
        private void Awake()
        {
            FindObjectOfType<AudioManager>().PlaySound("BackgroundMusic");
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}