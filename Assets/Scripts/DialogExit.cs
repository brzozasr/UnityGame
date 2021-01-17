using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class DialogExit : MonoBehaviour
    {
        public Canvas canvas;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (transform.position.x > -1998)
                {
                    transform.SetParent(canvas.transform, false);
                    PauseGame();
                }
                else
                {
                    transform.position = new Vector3 (Screen.width * 0.5f, Screen.height * 0.5f, 0);
                    PauseGame();
                }
                
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            transform.position = new Vector3(-1999f, 0, 0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}