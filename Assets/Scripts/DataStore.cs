using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class DataStore
    {
        public static int Lives { get; private set; }
        internal static int StartLives { get; set; }
        public static int HpPoints { get; private set; }
        internal static int StartHpPoints { get; set; }
        
        internal static int Score { get; set; }

        internal static List<string> VirtualKey = new List<string>();

        /// <summary>
        /// Method for current updating HP points.
        /// </summary>
        /// <param name="livesNo"></param>
        internal static int SetCurrentLives(int livesNo)
        {
            return Lives = livesNo;
        }

        /// <summary>
        /// Method for updating current HP points.
        /// </summary>
        /// <param name="hpPoints"></param>
        internal static int SetCurrentHpPoints(int hpPoints)
        {
            return HpPoints = hpPoints;
        }

        /// <summary>
        /// Method for adding HP points from First Aid Kit.
        /// </summary>
        /// <param name="amountHpPoints"></param>
        internal static int AddHpPoints(int amountHpPoints)
        {
            int hp = HpPoints + amountHpPoints;

            if (hp > StartHpPoints)
            {
                HpPoints = StartHpPoints;
            }
            else
            {
                HpPoints = hp;
            }
            
            return HpPoints;
        }

        internal static int AddPointsToScore(int collectedPoints)
        {
            int score = Score + collectedPoints;
            Score = score;
            return score;
        }
    }
}