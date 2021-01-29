using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class MainMenuController : MonoBehaviour
    {
        private GameObject _uiLoadMenu;
        private void Awake()
        {
            FindObjectOfType<AudioManager>().PlaySound("BackgroundMusic");
            _uiLoadMenu = GameObject.Find("UILoadMenu");

        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void LoadSavedGameMenu()
        {
            if (_uiLoadMenu.transform.position.x > -29998)
            {
                _uiLoadMenu.transform.SetParent(transform, false);
            }
            else
            {
                _uiLoadMenu.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            }
        }
    }
}