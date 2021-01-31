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
        internal static bool IsWonGameOver { get; set; }
        internal static bool IsLevelOver { get; set; }
        

        internal static Dictionary<string, int> Inventory = new Dictionary<string, int>();

        internal static void Clear()
        {
            Lives = 0;
            StartLives = 0;
            HpPoints = 0;
            StartHpPoints = 0;
            Score = 0;
            ClearInventory();
            IsWonGameOver = false;
            IsLevelOver = false;
        }
        
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
            Debug.Log($"{hp.ToString()} = {HpPoints.ToString()} + {amountHpPoints.ToString()}");
            Debug.Log($" - StartHpPoints: {StartHpPoints.ToString()}");

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

        internal static void AddItemsToInventory(string itemName, int itemQuantity)
        {
            if (Inventory.ContainsKey(itemName))
            {
                var quantity = Inventory[itemName];
                Inventory[itemName] = quantity + itemQuantity;
            }
            else
            {
                Inventory.Add(itemName, itemQuantity);
            }
        }
        
        internal static void RemoveItemsFromInventory(string itemName, int itemQuantity)
        {
            if (Inventory.ContainsKey(itemName))
            {
                var quantity = Inventory[itemName];
                if (quantity >= itemQuantity)
                {
                    Inventory[itemName] = quantity - itemQuantity;
                }
            }
        }

        internal static void ClearInventory()
        {
            Inventory.Clear();
        }

        internal static bool GetOneItemFromInventory(string itemName)
        {
            if (Inventory.ContainsKey(itemName))
            {
                var quantity = Inventory[itemName];
                if (quantity > 0)
                {
                    quantity -= 1;
                    Inventory[itemName] = quantity;
                    return true;
                }
            }

            return false;
        }

        internal static int GetItemQuantityFromInventory(string itemName)
        {
            if (Inventory.ContainsKey(itemName))
            {
                var quantity = Inventory[itemName];
                if (quantity > 0)
                {
                    return quantity;
                }
            }

            return 0;
        }
    }
}