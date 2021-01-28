using System;
using System.Collections;
using System.Linq;
using DefaultNamespace.DAO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Button = UnityEngine.UIElements.Button;

namespace DefaultNamespace
{
    public class DialogExit : MonoBehaviour
    {
        public Canvas canvas;

        public static string SaveNameInputField { get; private set; }
        

        private GameObject _messageText;
        private GameObject _yesButton;
        private GameObject _noButton;
        private GameObject _saveButton;
        private GameObject _saveMessageText;
        private GameObject _saveGameButton;
        private GameObject _exitSaveButton;
        private GameObject _saveNameInputField;
        private GameObject _savedMessageText;

        private TMP_InputField _inputField;

        private void Start()
        {
            _messageText = GameObject.Find("/DialogExit/BackgroundImage/MessageText");
            _yesButton = GameObject.Find("/DialogExit/BackgroundImage/ButtonYes");
            _noButton = GameObject.Find("/DialogExit/BackgroundImage/ButtonNo");
            _saveButton = GameObject.Find("/DialogExit/BackgroundImage/ButtonSave");
            _saveMessageText = GameObject.Find("/DialogExit/BackgroundImage/SaveMessageText");
            _saveGameButton = GameObject.Find("/DialogExit/BackgroundImage/ButtonSaveGame");
            _exitSaveButton = GameObject.Find("/DialogExit/BackgroundImage/ButtonExitSave");
            _saveNameInputField = GameObject.Find("/DialogExit/BackgroundImage/SaveNameInputField");
            _savedMessageText = GameObject.Find("/DialogExit/BackgroundImage/SavedMessageText");
            
            _inputField = _saveNameInputField.GetComponent<TMP_InputField>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (SceneManager.GetActiveScene().buildIndex > 0)
                {
                    if (_yesButton.activeInHierarchy)
                    {
                        _saveButton.SetActive(true);
                    }
                }

                if (transform.position.x > -9998)
                {
                    transform.SetParent(canvas.transform, false);
                    PauseGame();
                }
                else
                {
                    transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
                    PauseGame();
                }
            }
        }

        public void SaveGame()
        {
            if (_inputField.text.Length > 0)
            {
                _saveMessageText.SetActive(false);
                _saveGameButton.SetActive(false);
                _exitSaveButton.SetActive(false);
                _saveNameInputField.SetActive(false);
                _savedMessageText.SetActive(true);
                SaveNameInputField = _inputField.text;

                SqlSaveScoreDAO scoreDao = gameObject.AddComponent<SqlSaveScoreDAO>();
                scoreDao.Save();
                
                StartCoroutine(MenuAfterSave());
            }
        }

        IEnumerator MenuAfterSave()
        {
            Time.timeScale = 0.0000001f;
            yield return new WaitForSecondsRealtime(2);
            Time.timeScale = 0;
            _inputField.text = "";
            SaveNameInputField = "";
            _savedMessageText.SetActive(false);
            _messageText.SetActive(true);
            _yesButton.SetActive(true);
            _noButton.SetActive(true);
            _saveButton.SetActive(true);
        }

        public void ShowSaveDialog()
        {
            _messageText.SetActive(false);
            _yesButton.SetActive(false);
            _noButton.SetActive(false);
            _saveButton.SetActive(false);

            _saveMessageText.SetActive(true);
            _saveGameButton.SetActive(true);
            _exitSaveButton.SetActive(true);
            _saveNameInputField.SetActive(true);
        }

        public void HideSaveDialog()
        {
            _messageText.SetActive(true);
            _yesButton.SetActive(true);
            _noButton.SetActive(true);
            _saveButton.SetActive(true);

            _saveMessageText.SetActive(false);
            _saveGameButton.SetActive(false);
            _exitSaveButton.SetActive(false);
            _saveNameInputField.SetActive(false);
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
            transform.position = new Vector3(-9999f, 0, 0);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}