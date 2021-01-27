using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlDroneDAO : ISqlGameObjectData
    {
        private long _lastID;
        
        public void Save(GameObjectData obj)
        {
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
                        _lastID = (long) cmdLastId.ExecuteScalar();
                    }

                    if (obj.Go.name == "Player Variant")
                    {
                        
                    }
                    else if (obj.Go.name == "Drone")
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO drone (save_date, save_name, save_scene_id)
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
                    }
                    else if (obj.Go.name == "SuperDrone")
                    {
                    }
                    else if (obj.Go.name == "MegaDrone")
                    {
                    }
                    else if (obj.Go.name == "Chip")
                    {
                        
                    }
                    else if (obj.Go.name == "FirstAidKitBiohazard" ||
                             obj.Go.name == "FirstAidKitGreen" || 
                             obj.Go.name == "FirstAidKitRed" || 
                             obj.Go.name == "FirstAidKitWhite")
                    {
                        
                    }
                    else if (obj.Go.name == "PointWidgetS" ||
                             obj.Go.name == "PointWidgetM" || 
                             obj.Go.name == "PointWidgetL" || 
                             obj.Go.name == "PointWidgetXL")
                    {
                        
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
                throw;
            }
        }

        public List<GameObjectData> Load()
        {
            return new List<GameObjectData>();
        }
    }
}