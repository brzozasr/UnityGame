using System;
using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameOver : MonoBehaviour
    {
        public Canvas canvas;
        public TextMeshProUGUI livesNo;
        
        private int _lives;
        private bool _isRunOnce = false;
        private AudioManager _audioManager;
        private bool _isWonGameOver = DataStore.IsWonGameOver;
        private GameObject _gameObject;
        private TextMeshProUGUI _winLabel;

        private void Awake()
        {
            _lives = GetCurrentLivesNo(livesNo.text);
            _audioManager = FindObjectOfType<AudioManager>();
            
            _gameObject = GameObject.Find("PassedLevelsText");
            _winLabel = _gameObject.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _lives = GetCurrentLivesNo(livesNo.text);
            _isWonGameOver = DataStore.IsWonGameOver;
            
            if (_lives <= 0 && _isRunOnce == false)
            {
                PlayGameOverSound();
                StartCoroutine(SetGameOver());
                _isRunOnce = true;
            }
            else if (_isWonGameOver && _isRunOnce == false)
            {
                StartCoroutine(SetGameOver());
                _isRunOnce = true;
            }
        }

        private int GetCurrentLivesNo(string livesStr)
        {
            string[] lives = livesStr.Split(':');
            int liveNo = Int32.Parse(lives[1].Trim());
            return liveNo;
        }

        IEnumerator SetGameOver()
        {
            yield return new WaitForSeconds(1.5f);
            
            transform.SetParent(canvas.transform, false);

            if (_isWonGameOver)
            {
                _winLabel.enabled = true;
            }
            else
            {
                _winLabel.enabled = false;
            }
            
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        private void PlayGameOverSound()
        {
            _audioManager.PlaySound("GameOver");
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void PlayAgain()
        {
            DataStore.Clear();
            //transform.parent = null;
            _isRunOnce = false;
            SceneManager.LoadScene(1);
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}