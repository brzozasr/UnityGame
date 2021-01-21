using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class NextLevel : MonoBehaviour
    {
        public Canvas canvas;
        
        private bool _isRunOnce = false;
        private bool _isLevelOver = DataStore.IsWonGameOver;

        private void Update()
        {
            _isLevelOver = DataStore.IsLevelOver;
            
            if (_isLevelOver && _isRunOnce == false)
            {
                StartCoroutine(ShowNextLevelImage());
                StartCoroutine(GoToNextLevel());
                _isRunOnce = true;
            }
        }

        IEnumerator ShowNextLevelImage()
        {
            yield return new WaitForSeconds(2.0f);
            transform.SetParent(canvas.transform, false);
            AudioListener.pause = true;
        }

        IEnumerator GoToNextLevel()
        {
            yield return new WaitForSeconds(5.0f);
            transform.parent = null;
            _isRunOnce = false;
            int index = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(index + 1);
            DataStore.IsLevelOver = false;
            Time.timeScale = 1;
            AudioListener.pause = false;
        }
    }
}