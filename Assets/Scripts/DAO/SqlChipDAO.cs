using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlChipDAO : ISqlGameObjectData<GameObjectData>
    {
        public void Save(GameObjectData obj)
        {
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
                            cmd.CommandText = @"INSERT INTO chip (chip_save_id, chip_scene_id, chip_name, chip_pos_x, 
                                                chip_pos_y, chip_pos_z, chip_parent)
                                                VALUES (@ChipSaveId, @ChipSceneId, @ChipName, 
                                                        @ChipPosX, @ChipPosY, @ChipPosZ, @ChipParent);";

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
            throw new System.NotImplementedException();
        }
    }
}