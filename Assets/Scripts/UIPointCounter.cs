using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIPointCounter : MonoBehaviour
    {
        public TextMeshProUGUI textLives;
        public TextMeshProUGUI textHp;
        public TextMeshProUGUI textScore;

        private void Update()
        {
            textLives.text = $"Lives: {DataStore.Lives.ToString()}";
            textHp.text = $"HP: {DataStore.HpPoints.ToString()}";
            textScore.text = $"Score: {DataStore.Score.ToString()}";
        }
        
    }
}