using System;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using Spawner;
using UnityEngine;

namespace DefaultNamespace.DAO
{
    public class SqlPointWidgetDAO : ISqlGameObjectData<GameObjectData>
    {
        public void Save(GameObjectData obj)
        {
            try
            {
                using (var conn = new SqliteConnection(SqlDataConnection.DBPath))
                {
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
            throw new System.NotImplementedException();
        }
    }
}