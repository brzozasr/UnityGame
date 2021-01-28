using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlPlayerDAO : ISqlGameObjectData<GameObjectData>
    {
        public void Save(GameObjectData obj)
        {
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
            throw new System.NotImplementedException();
        }
    }
}