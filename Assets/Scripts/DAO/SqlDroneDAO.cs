using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlDroneDAO : ISqlGameObjectData<GameObjectData>
    {
        public void Save(GameObjectData obj)
        {
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();

                    if (obj.Go.name == "Drone")
                    {
                        DroneData droneData = (DroneData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO drone (drone_save_id, drone_scene_id, drone_name, 
                                                drone_shot_time_from, drone_shot_time_to, drone_hit_points, 
                                                drone_pos_x, drone_pos_y, drone_pos_z, drone_parent)
                                                VALUES (@DroneSaveId, @DroneSceneId, @DroneName, @DroneShotTimeFrom, 
                                                        @DroneShotTimeTo, @DroneHitPoints, @DronePosX, @DronePosY, 
                                                        @DronePosZ, @DroneParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex 
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneName",
                                Value = droneData.Go.name
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeFrom",
                                Value = droneData.ShotTimeRangeFrom
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeTo",
                                Value = droneData.ShotTimeRangeTo
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneHitPoints",
                                Value = droneData.HitPoints
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosX",
                                Value = droneData.Position.x
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosY",
                                Value = droneData.Position.y
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosZ",
                                Value = droneData.Position.z
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneParent",
                                Value = String.Join("/", droneData.Parents)
                            });

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (obj.Go.name == "SuperDrone")
                    {
                        SuperDroneData superDroneData = (SuperDroneData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO drone (drone_save_id, drone_scene_id, drone_name, 
                                                drone_shot_time_from, drone_shot_time_to, drone_hit_points, 
                                                drone_speed, drone_move_y, drone_pos_x, drone_pos_y, 
                                                drone_pos_z, drone_parent)
                                                VALUES (@DroneSaveId, @DroneSceneId, @DroneName, @DroneShotTimeFrom, 
                                                        @DroneShotTimeTo, @DroneHitPoints, @DroneSpeed, @DroneMoveY,
                                                        @DronePosX, @DronePosY, @DronePosZ, @DroneParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneName",
                                Value = superDroneData.Go.name
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeFrom",
                                Value = superDroneData.ShotTimeRangeFrom
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeTo",
                                Value = superDroneData.ShotTimeRangeTo
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneHitPoints",
                                Value = superDroneData.HitPoints
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSpeed",
                                Value = superDroneData.MoveSpeed
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneMoveY",
                                Value = superDroneData.MaxMoveY
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosX",
                                Value = superDroneData.Position.x
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosY",
                                Value = superDroneData.Position.y
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosZ",
                                Value = superDroneData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneParent",
                                Value = String.Join("/", superDroneData.Parents)
                            });

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (obj.Go.name == "MegaDrone")
                    {
                        MegaDroneData megaDroneData = (MegaDroneData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO drone (drone_save_id, drone_scene_id, drone_name, 
                                                drone_shot_time_from, drone_shot_time_to, drone_hit_points, 
                                                drone_speed, drone_move_y, drone_move_x, drone_pos_x, drone_pos_y, 
                                                drone_pos_z, drone_parent)
                                                VALUES (@DroneSaveId, @DroneSceneId, @DroneName, @DroneShotTimeFrom, 
                                                        @DroneShotTimeTo, @DroneHitPoints, @DroneSpeed, @DroneMoveY,
                                                        @DroneMoveX, @DronePosX, @DronePosY, @DronePosZ, @DroneParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneName",
                                Value = megaDroneData.Go.name
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeFrom",
                                Value = megaDroneData.ShotTimeRangeFrom
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneShotTimeTo",
                                Value = megaDroneData.ShotTimeRangeTo
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneHitPoints",
                                Value = megaDroneData.HitPoints
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneSpeed",
                                Value = megaDroneData.MoveSpeed
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneMoveY",
                                Value = megaDroneData.MaxMoveY
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneMoveX",
                                Value = megaDroneData.MaxMoveX
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosX",
                                Value = megaDroneData.Position.x
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosY",
                                Value = megaDroneData.Position.y
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DronePosZ",
                                Value = megaDroneData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "DroneParent",
                                Value = String.Join("/", megaDroneData.Parents)
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
            return new List<GameObjectData>();
        }
    }
}