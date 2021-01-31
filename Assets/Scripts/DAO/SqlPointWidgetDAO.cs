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
    public class SqlPointWidgetDAO : ISqlGameObjectData<GameObjectData>
    {
        private GameObject _prefab;
        private readonly GameObject _pointWidgetS;
        private readonly GameObject _pointWidgetM;
        private readonly GameObject _pointWidgetL;
        private readonly GameObject _pointWidgetXl;
        private Vector3 _vector3;
        private List<string> _parent;

        public SqlPointWidgetDAO(GameObject pointWidgetS, GameObject pointWidgetM, GameObject pointWidgetL, GameObject pointWidgetXl)
        {
            _pointWidgetS = pointWidgetS;
            _pointWidgetM = pointWidgetM;
            _pointWidgetL = pointWidgetL;
            _pointWidgetXl = pointWidgetXl;
        }
        
        public void Save(GameObjectData obj)
        {
            SqlDataConnection.SetCurrentSceneIndex();
            
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
                    conn.Open();
                    if (obj.Go.name == "PointWidgetS" || obj.Go.name == "PointWidgetM" ||
                        obj.Go.name == "PointWidgetL" || obj.Go.name == "PointWidgetXL")
                    {
                        PointWidgetData pointWidgetData = (PointWidgetData) obj;

                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = @"INSERT INTO widget (widget_save_id, widget_scene_id, widget_name, widget_points, 
                                                widget_pos_x, widget_pos_y, widget_pos_z, widget_parent)
                                                VALUES (@WidgetSaveId, @WidgetSceneId, @WidgetName, @WidgetPoints, 
                                                        @WidgetPosX, @WidgetPosY, @WidgetPosZ, @WidgetParent);";

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetSaveId",
                                Value = SqlSaveScoreDAO.SaveID
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetSceneId",
                                Value = SqlDataConnection.CurrentSceneIndex 
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetName",
                                Value = pointWidgetData.Go.name
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetPoints",
                                Value = pointWidgetData.WidgetPoints
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetPosX",
                                Value = pointWidgetData.Position.x
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetPosY",
                                Value = pointWidgetData.Position.y
                            });
                            
                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetPosZ",
                                Value = pointWidgetData.Position.z
                            });

                            cmd.Parameters.Add(new SqliteParameter
                            {
                                ParameterName = "WidgetParent",
                                Value = String.Join("/", pointWidgetData.Parents)
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
                        cmd.CommandText = @"SELECT widget_scene_id, widget_name, widget_points, 
                                            widget_pos_x, widget_pos_y, widget_pos_z, 
                                            widget_parent FROM widget 
                                            WHERE widget_save_id = @SaveID;";

                        cmd.Parameters.Add(new SqliteParameter
                        {
                            ParameterName = "SaveID",
                            Value = saveId
                        });

                        var reader = cmd.ExecuteReader();
                        while (reader.Read()) {
                            var sceneId = reader.GetInt32(0);
                            var name = reader.GetString(1);
                            var points = reader.GetInt32(2);
                            var posX = reader.GetFloat(3);
                            var posY = reader.GetFloat(4);
                            var posZ = reader.GetFloat(5);
                            var parent = reader.GetString(6);
                            
                            // _prefab = Resources.Load<GameObject>($"Assets/Resources/{name}.prefab");
                            //_prefab = AssetDatabase.LoadAssetAtPath($"Assets/Resources/{name}.prefab", typeof(GameObject)) as GameObject;
                            _vector3 = new Vector3(posX, posY, posZ);
                            _parent = parent.Split('/').ToList();

                            if (name == "PointWidgetS")
                            {
                                PointWidgetData pointWidgetS =
                                    new PointWidgetData(_pointWidgetS, _vector3, _parent, points);
                                gameObjectDataList.Add(pointWidgetS);
                            }
                            else if (name == "PointWidgetM")
                            {
                                PointWidgetData pointWidgetM =
                                    new PointWidgetData(_pointWidgetM, _vector3, _parent, points);
                                gameObjectDataList.Add(pointWidgetM);
                            }
                            else if (name == "PointWidgetL")
                            {
                                PointWidgetData pointWidgetL =
                                    new PointWidgetData(_pointWidgetL, _vector3, _parent, points);
                                gameObjectDataList.Add(pointWidgetL);
                            }
                            else if (name == "PointWidgetXL")
                            {
                                PointWidgetData pointWidgetXl =
                                    new PointWidgetData(_pointWidgetXl, _vector3, _parent, points);
                                gameObjectDataList.Add(pointWidgetXl);
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