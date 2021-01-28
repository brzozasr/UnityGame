using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class ChipData : GameObjectData
    {
        public string ItemName;
        public int ItemQuantity;

        private ChipController _chipController;
        
        public ChipData(GameObject chip, Vector3 position, List<string> parents, string itemName, int itemQuantity) :
            base(chip, position, parents)
        {
            _chipController = chip.GetComponent<ChipController>();
            
            ItemName = itemName;
            ItemQuantity = itemQuantity;
        }
        
        public override void UpdateGoData()
        {
            _chipController.itemName = ItemName;
            _chipController.itemQuantity = ItemQuantity;
        }
        
        public static (Vector3, string, int) GetChipParameters(Transform chip)
        {
            ChipController chipController = chip.gameObject.GetComponent<ChipController>();
            
            Vector3 position = chip.position;
            string itemName = chipController.itemName;
            int itemQuantity = chipController.itemQuantity;

            return (position, itemName, itemQuantity);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"ItemName: {ItemName}; ");
            sb.Append($"ItemQuantity: {ItemQuantity.ToString()}; ");

            return sb.ToString();
        }
    }
}