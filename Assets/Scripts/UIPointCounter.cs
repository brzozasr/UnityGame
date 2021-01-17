using System;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class UIPointCounter : MonoBehaviour
    {
        public TextMeshProUGUI textHp;
        
        public static UIPointCounter Instance;
        private void Awake()
        {
            Instance = this;
            FirstAidKitController.OnFirstAidCollected += DisplayHpPoints;
        }

        public void DisplayHpPoints(int hpPoints)
        {
            var hp = textHp.text.Split(':');
            int intHp = Int32.Parse(hp[1].Trim());

            int result = intHp + hpPoints;

            // TODO 100
            if (result > 100)
            {
                textHp.text = "HP: 100";
            }
            else
            {
                textHp.text = $"HP: {result}";
            }
        }
    }
}