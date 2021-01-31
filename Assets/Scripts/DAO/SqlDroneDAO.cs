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
    public class SqlDroneDAO : ISqlGameObjectData<GameObjectData>
    {
        private GameObject _prefab;
        private GameObject _drone;
        private GameObject _superDrone;
        private GameObject _megaDrone;
        private Vector3 _vector3;
        private List<string> _parent;

        public SqlDroneDAO(GameObject drone, GameObject superDrone, GameObject megaDrone)
        {
            _drone = drone;
            _superDrone = superDrone;
            _megaDrone = megaDrone;
        }
        

        public void Save(GameObjectData obj)
        {
            SqlDataConnection.SetCurrentSceneIndex();
            
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
            List<GameObjectData> gameObjectDataList = new List<GameObjectData>();
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT drone_scene_id, drone_name, drone_shot_time_from, 
                                            drone_shot_time_to, drone_hit_points, drone_speed, 
                                            drone_move_y, drone_move_x, drone_pos_x, drone_pos_y, 
                                            drone_pos_z, drone_parent FROM drone 
                                            WHERE drone_save_id = @SaveID;";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveID",
                            Value = saveId
                        });

                        var reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            var sceneId = reader.GetInt32(0);
                            var name = reader.GetString(1);
                            var shotTimeFrom = reader.GetFloat(2);
                            var shotTimeTo = reader.GetFloat(3);
                            var hitPoints = reader.GetInt32(4);
                            
                            var posX = reader.GetFloat(8);
                            var posY = reader.GetFloat(9);
                            var posZ = reader.GetFloat(10);
                            
                            var parent = reader.GetString(11);
                            
                            float speed;
                            float moveY;
                            float moveX;

                            if (name == "Drone")
                            {
                                //_prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{name}.prefab", typeof(GameObject)) as GameObject;
                                _vector3 = new Vector3(posX, posY, posZ);
                                _parent = parent.Split('/').ToList();

                                DroneData chipData =
                                    new DroneData(_drone, _vector3, _parent, shotTimeFrom, shotTimeTo, hitPoints);
                                
                                Debug.Log($"Drone name: {chipData.Go.name}");
                                gameObjectDataList.Add(chipData);
                            }
                            else if (name == "SuperDrone")
                            {
                                moveY = reader.GetFloat(6);
                                speed = reader.GetFloat(5);
                                
                                //_prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{name}.prefab", typeof(GameObject)) as GameObject;
                                _vector3 = new Vector3(posX, posY, posZ);
                                _parent = parent.Split('/').ToList();

                                SuperDroneData chipData =
                                    new SuperDroneData(_superDrone, _vector3, _parent, shotTimeFrom, shotTimeTo, hitPoints, moveY, speed);
                                gameObjectDataList.Add(chipData);
                            }
                            else if (name == "MegaDrone")
                            {
                                moveY = reader.GetFloat(6);
                                speed = reader.GetFloat(5);
                                moveX = reader.GetFloat(7);
                                
                                //_prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{name}.prefab", typeof(GameObject)) as GameObject;
                                _vector3 = new Vector3(posX, posY, posZ);
                                _parent = parent.Split('/').ToList();

                                MegaDroneData chipData =
                                    new MegaDroneData(_megaDrone, _vector3, _parent, shotTimeFrom, shotTimeTo, hitPoints, moveY, speed, moveX);
                                gameObjectDataList.Add(chipData);
                            }
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