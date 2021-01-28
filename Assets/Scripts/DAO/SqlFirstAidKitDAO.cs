using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlFirstAidKitDAO : ISqlGameObjectData<GameObjectData>
    {
        public void Save(GameObjectData obj)
        {
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();
                    if (obj.Go.name == "FirstAidKitBiohazard" ||
                        obj.Go.name == "FirstAidKitGreen" ||
                        obj.Go.name == "FirstAidKitRed" ||
                        obj.Go.name == "FirstAidKitWhite")
                    {
                        FirstAidKitData firstAidKitData = (FirstAidKitData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO firstaid (firstaid_save_id, firstaid_scene_id, firstaid_name, 
                                                firstaid_rec_points, firstaid_pos_x, firstaid_pos_y, firstaid_pos_z, firstaid_parent)
                                                VALUES (@FirstaidSaveId, @FirstaidSceneId, @FirstaidName, @FirstaidRecPoints, 
                                                        @FirstaidPosX, @FirstaidPosY, @FirstaidPosZ, @FirstaidParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex 
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidName",
                                Value = firstAidKitData.Go.name
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidRecPoints",
                                Value = firstAidKitData.HitPointRecovery
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidPosX",
                                Value = firstAidKitData.Position.x
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidPosY",
                                Value = firstAidKitData.Position.y
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidPosZ",
                                Value = firstAidKitData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "FirstaidParent",
                                Value = String.Join("/", firstAidKitData.Parents)
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