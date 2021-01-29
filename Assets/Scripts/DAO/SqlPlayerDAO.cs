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
    public class SqlPlayerDAO : ISqlGameObjectData<GameObjectData>
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
                    if (obj.Go.name == "Player")
                    {
                        PlayerData playerData = (PlayerData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO player (player_save_id, player_scene_id, player_lives, 
                                                player_hp, player_pos_x, player_pos_y, player_pos_z, player_parent)
                                                VALUES (@PlayerSaveId, @PlayerSceneId, @PlayerLives, @PlayerHp,
                                                        @PlayerPosX, @PlayerPosY, @PlayerPosZ, @PlayerParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex 
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerLives",
                                Value = DataStore.StartLives
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerHp",
                                Value = DataStore.StartHpPoints
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerPosX",
                                Value = playerData.Position.x
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerPosY",
                                Value = playerData.Position.y
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerPosZ",
                                Value = playerData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "PlayerParent",
                                Value = String.Join("/", playerData.Parents)
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
                        cmd.CommandText = @"SELECT player_scene_id, player_lives, player_hp, 
                                            player_pos_x, player_pos_y, player_pos_z, 
                                            player_parent FROM player 
                                            WHERE player_save_id = @SaveID;";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveID",
                            Value = saveId
                        });

                        var reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            var sceneId = reader.GetInt32(0);
                            var lives = reader.GetInt32(1);
                            var hp = reader.GetInt32(2);
                            var posX = reader.GetFloat(3);
                            var posY = reader.GetFloat(4);
                            var posZ = reader.GetFloat(5);
                            var parent = reader.GetString(6);
                            
                            // _prefab = Resources.Load<GameObject>($"Assets/Resources/{name}.prefab");
                            _prefab = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Player.prefab", typeof(GameObject)) as GameObject;
                            _vector3 = new Vector3(posX, posY, posZ);
                            _parent = parent.Split('/').ToList();

                            PlayerData playerData =
                                new PlayerData(_prefab, _vector3, _parent, hp, lives);
                            gameObjectDataList.Add(playerData);
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