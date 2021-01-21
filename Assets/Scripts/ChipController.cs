using UnityEngine;

namespace DefaultNamespace
{
    public class ChipController : MonoBehaviour
    {
        public string itemName;
        [Range(1, 20)]
        public int itemQuantity;
        private void Update()
        {
            transform.Rotate(0.0f, 90.0f * Time.deltaTime , 0.0f, Space.World);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                FindObjectOfType<AudioManager>().PlaySound("CollectChip");
                DataStore.AddItemsToInventory(itemName, itemQuantity);
                Destroy(gameObject);
            }
        }
    }
}