using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using Spawner;
using UnityEditor;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlChipDAO : ISqlGameObjectData<GameObjectData>
    {
        private GameObject _prefab;
        private Vector3 _vector3;
        private List<string> _parent;
        
        public void Save(GameObjectData obj)
        {
            SqlDataConnection.SetCurrentSceneIndex();
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    if (obj.Go.name == "Chip")
                    {
                        ChipData chipData = (ChipData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO chip (chip_save_id, chip_scene_id, chip_name, 
                                                chip_item_name, chip_item_no, chip_pos_x, chip_pos_y, 
                                                chip_pos_z, chip_parent)
                                                VALUES (@ChipSaveId, @ChipSceneId, @ChipName, @ChipItemName, 
                                                        @ChipItemNo, @ChipPosX, @ChipPosY, @ChipPosZ, @ChipParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex 
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipName",
                                Value = chipData.Go.name
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipItemName",
                                Value = chipData.ItemName
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipItemNo",
                                Value = chipData.ItemQuantity
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipPosX",
                                Value = chipData.Position.x
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipPosY",
                                Value = chipData.Position.y
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipPosZ",
                                Value = chipData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "ChipParent",
                                Value = String.Join("/", chipData.Parents)
                            });

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public List<GameObjectData> Load(int saveId)
        {
            List<GameObjectData> gameObjectDataList = new List<GameObjectData>();
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT chip_scene_id, chip_name, chip_item_name, 
                                            chip_item_no, chip_pos_x, chip_pos_y, chip_pos_z, 
                                            chip_parent FROM chip 
                                            WHERE chip_save_id = @SaveID;";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveID",
                            Value = saveId
                        });

                        var reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            var sceneId = reader.GetInt32(0);
                            var chipName = reader.GetString(1);
                            var itemName = reader.GetString(2);
                            var itemNo = reader.GetInt32(3);
                            var posX = reader.GetFloat(4);
                            var posY = reader.GetFloat(5);
                            var posZ = reader.GetFloat(6);
                            var parent = reader.GetString(7);
                            
                            // _prefab = Resources.Load<GameObject>($"Assets/Resources/{name}.prefab");
                            _prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{chipName}.prefab", typeof(GameObject)) as GameObject;
                            _vector3 = new Vector3(posX, posY, posZ);
                            _parent = parent.Split('/').ToList();

                            ChipData chipData =
                                new ChipData(_prefab, _vector3, _parent, itemName, itemNo);
                            gameObjectDataList.Add(chipData);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }

            return gameObjectDataList;
        }
    }
}