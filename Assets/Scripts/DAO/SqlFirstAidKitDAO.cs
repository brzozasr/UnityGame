using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using Spawner;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace.DAO
{
    public class SqlFirstAidKitDAO : ISqlGameObjectData<GameObjectData>
    {
        private GameObject _prefab;
        private readonly GameObject _firstAidKitBiohazard;
        private readonly GameObject _firstAidKitGreen;
        private readonly GameObject _firstAidKitRed;
        private readonly GameObject _firstAidKitWhite;
        private Vector3 _vector3;
        private List<string> _parent;

        public SqlFirstAidKitDAO(GameObject firstAidKitBiohazard, GameObject firstAidKitGreen, GameObject firstAidKitRed, GameObject firstAidKitWhite)
        {
            _firstAidKitBiohazard = firstAidKitBiohazard;
            _firstAidKitGreen = firstAidKitGreen;
            _firstAidKitRed = firstAidKitRed;
            _firstAidKitWhite = firstAidKitWhite;
        }
        
        public void Save(GameObjectData obj)
        {
            SqlDataConnection.SetCurrentSceneIndex();
            
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
                            cmd.CommandText =
                                @"INSERT INTO firstaid (firstaid_save_id, firstaid_scene_id, firstaid_name, 
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
            List<GameObjectData> gameObjectDataList = new List<GameObjectData>();
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = @"SELECT firstaid_scene_id, firstaid_name, 
                                            firstaid_rec_points, firstaid_pos_x, firstaid_pos_y, 
                                            firstaid_pos_z, firstaid_parent FROM firstaid 
                                            WHERE firstaid_save_id = @SaveID;";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveID",
                            Value = saveId
                        });

                        var reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            var sceneId = reader.GetInt32(0);
                            var name = reader.GetString(1);
                            var recoveryPoints = reader.GetInt32(2);
                            var posX = reader.GetFloat(3);
                            var posY = reader.GetFloat(4);
                            var posZ = reader.GetFloat(5);
                            var parent = reader.GetString(6);
                            
                            // _prefab = Resources.Load<GameObject>($"Assets/Resources/{name}.prefab");
                            //_prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{name}.prefab", typeof(GameObject)) as GameObject;
                            
                            _vector3 = new Vector3(posX, posY, posZ);
                            _parent = parent.Split('/').ToList();

                            if (name == "FirstAidKitBiohazard")
                            {
                                FirstAidKitData firstAidKitBiohazard =
                                    new FirstAidKitData(_firstAidKitBiohazard, _vector3, _parent, recoveryPoints);
                                
                                gameObjectDataList.Add(firstAidKitBiohazard);
                            }
                            else if (name == "FirstAidKitGreen")
                            {
                                FirstAidKitData firstAidKitGreen =
                                    new FirstAidKitData(_firstAidKitGreen, _vector3, _parent, recoveryPoints);
                                
                                gameObjectDataList.Add(firstAidKitGreen);
                            }
                            else if (name == "FirstAidKitRed")
                            {
                                FirstAidKitData firstAidKitRed =
                                    new FirstAidKitData(_firstAidKitRed, _vector3, _parent, recoveryPoints);
                                
                                gameObjectDataList.Add(firstAidKitRed);
                            }
                            else if (name == "FirstAidKitWhite")
                            {
                                FirstAidKitData firstAidKitWhite =
                                    new FirstAidKitData(_firstAidKitWhite, _vector3, _parent, recoveryPoints);
                                
                                gameObjectDataList.Add(firstAidKitWhite);
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