using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class ChipData : GameObjectData
    {
        private string _itemName;
        private int _itemQuantity;

        private ChipController _chipController;
        
        public ChipData(GameObject chip, Vector3 position, List<string> parents, string itemName, int itemQuantity) :
            base(chip, position, parents)
        {
            _chipController = chip.GetComponent<ChipController>();
            
            _itemName = itemName;
            _itemQuantity = itemQuantity;
        }
        
        public override void UpdateGoData()
        {
            _chipController.itemName = _itemName;
            _chipController.itemQuantity = _itemQuantity;
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
            sb.Append($"ItemName: {_itemName}; ");
            sb.Append($"ItemQuantity: {_itemQuantity.ToString()}; ");

            return sb.ToString();
        }
    }
}