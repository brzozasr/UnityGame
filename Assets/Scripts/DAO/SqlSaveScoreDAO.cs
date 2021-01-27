using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlSaveScoreDAO : MonoBehaviour
    {
        public static long SaveID { get; private set; }
        
        private long _scoreID;
        
        /// <summary>
        /// Method saves the save data name and score
        /// </summary>
        public void Save()
        {
            var lives = DataStore.Lives;
            var hp = DataStore.HpPoints;
            var points = DataStore.Score;
            var chips = DataStore.Inventory;
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO save (save_date, save_name, save_scene_id)
                                          VALUES (@SaveDate, @SaveName, @SaveSceneId);";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveDate",
                            Value = SqlDataConnection.CurrentDataTime
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveName",
                            Value = "TestName" // TODO set value from text field
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveSceneId",
                            Value = SqlDataConnection.CurrentSceneIndex
                        });

                        cmd.ExecuteNonQuery();
                    }
                    
                    using (var cmdLastId = conn.CreateCommand())
                    {
                        cmdLastId.CommandType = CommandType.Text;
                        cmdLastId.CommandText = "SELECT last_insert_rowid()";
                        SaveID = (long) cmdLastId.ExecuteScalar();
                    }
                    
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"INSERT INTO score (score_save_id, score_lives, score_hp, score_points)
                                          VALUES (@ScoreSaveId, @ScoreLives, @ScoreHp, @ScorePoints);";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "ScoreSaveId",
                            Value = SaveID
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "ScoreLives",
                            Value = lives
                        });

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "ScoreHp",
                            Value = hp
                        });
                        
                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "ScorePoints",
                            Value = points
                        });

                        cmd.ExecuteNonQuery();
                    }
                    
                    using (var cmdScoreId = conn.CreateCommand())
                    {
                        cmdScoreId.CommandType = CommandType.Text;
                        cmdScoreId.CommandText = "SELECT last_insert_rowid()";
                        _scoreID = (long) cmdScoreId.ExecuteScalar();
                    }

                    foreach (var chip in chips)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText =
                                @"INSERT INTO inventory (inventory_score_id, inventory_item_name, inventory_item_no)
                                          VALUES (@InventoryScoreId, @InventoryItemName, @InventoryItemNo);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "InventoryScoreId",
                                Value = _scoreID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "InventoryItemName",
                                Value = chip.Key
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "InventoryItemNo",
                                Value = chip.Value
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

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}