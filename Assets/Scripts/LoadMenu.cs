using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.DAO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class LoadMenu : MonoBehaviour
    {
        private Dictionary<string, string> _items;
        
        private void Start()
        {
            var sqlSaveScore = gameObject.AddComponent<SqlSaveScoreDAO>();
            _items = sqlSaveScore.GetMenuItems();

            var dropdownGameObject = GameObject.Find("/UILoadMenu/SavedDropdown");
            var dropdown = dropdownGameObject.GetComponent<TMP_Dropdown>();
            
            dropdown.options.Clear();
            
            dropdown.options.Add(new TMP_Dropdown.OptionData() {text = "Select a save..."});

            int noStoredLastSaves = 5;

            int j = 0;
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                var item = _items.ElementAt(i);

                var data = item.Key.Split('-');
                var saveId = Int32.Parse(data[0]);

                if (saveId > 1 && j <= noStoredLastSaves - 1)
                {
                    dropdown.options.Add(new TMP_Dropdown.OptionData() {text = item.Value});
                    j++;
                }
                else if (saveId > 1)
                {
                    sqlSaveScore.DeleteSave(saveId);
                }
            }
            
            dropdown.onValueChanged.AddListener(delegate { DropDownItemSelected(dropdown); });
        }

        private void DropDownItemSelected(TMP_Dropdown dropdown)
        {
            int index = dropdown.value;
            string itemText = dropdown.options[index].text;

            if (index > 0)
            {
                var keyString = _items.FirstOrDefault(x => x.Value == itemText).Key;
            
                var data = keyString.Split('-');
                var sceneId = Int32.Parse(data[1]);
                
                SceneManager.LoadScene(sceneId);
            }
        }

        public void ExitLoad()
        {
            transform.position = new Vector3(-29999f, 0, 0);
        }
    }
}