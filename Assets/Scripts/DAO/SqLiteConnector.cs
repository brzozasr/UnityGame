using System;
using System.Data;
using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace DefaultNamespace.DAO
{
    public class SqLiteConnector : MonoBehaviour
    {
        public TextMeshProUGUI test;
        protected string DBPath;
        private void Start()
        {
            // DBPath = $"URI=file:{Application.persistentDataPath}/Database/data.db";
            // DBPath = $"URI=file:/Users/sbrzoza/RiderProjects/UnityTest/Assets/Database/data.db";
            
            DBPath = $"URI=file:data.db";

            Invoke(nameof(GetHighScores), 3.0f);
            SqlDroneDAO sqlDroneDao = new SqlDroneDAO();
            
        }
        
        public void GetHighScores() {
            try
            {
                string score = "";
                using (var conn = new SqliteConnection(DBPath)) {
                    
                    Debug.Log($"before: { conn.State == ConnectionState.Open}");
                    
                    conn.Open();
                    
                    Debug.Log($"after: {conn.State == ConnectionState.Open}");
                
                    using (SqliteCommand cmd = new SqliteCommand (conn)) {
                        cmd.CommandText = "SELECT * FROM save;";
                        
                        var reader = cmd.ExecuteReader();
                        
                        
                        while (reader.Read()) {
                            var id = reader.GetInt32(0);
                            var highScoreName = reader.GetString(1);
                            score = reader.GetString(2);
                            var text = string.Format("{0}: {1} [#{2}]", highScoreName, score, id);
                            Debug.Log(text);
                        }
                        Debug.Log("scores (end)");
                    }
                }

                test.text = score;
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            // IDbConnection dbcon = new SqliteConnection(DBPath);
        // dbcon.Open();
        // IDbCommand dbcmd = dbcon.CreateCommand();
        // const string sql =
        //     "SELECT * FROM save";
        // dbcmd.CommandText = sql;
        // IDataReader reader = dbcmd.ExecuteReader();
        // while(reader.Read())
        // {
        //     var firstName = reader.GetInt32(0);
        //     string lastName = reader.GetString(1);
        //     Console.WriteLine("Name: {0} {1}",
        //         firstName, lastName);
        // }
        // // clean up
        // reader.Dispose();
        // dbcmd.Dispose();
        // dbcon.Close();
        }
        
    }
}