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
        public TextMeshProUGUI textChip;

        private string[] _chipNames;

        private void Start()
        {
            _chipNames = new string[]
            {
                "Chip",
                "Chip1",
                "Chip2"
            };
        }

        private void Update()
        {
            textLives.text = $"Lives: {DataStore.Lives.ToString()}";
            textHp.text = $"HP: {DataStore.HpPoints.ToString()}";
            textScore.text = $"Score: {DataStore.Score.ToString()}";
            textChip.text = $"x {CountChips().ToString()}";
        }

        private int CountChips()
        {
            int chipCount = 0;
            
            foreach (var chipName in _chipNames)
            {
                chipCount += DataStore.GetItemQuantityFromInventory(chipName);
            }

            return chipCount;
        }
    }
}