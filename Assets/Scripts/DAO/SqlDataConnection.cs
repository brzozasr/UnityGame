using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.DAO
{
    public static class SqlDataConnection
    {
        internal static string DBPath = $"URI=file:data.db";
        internal static int CurrentSceneIndex { get; private set; }
        internal static string CurrentDataTime { get; set; }

        public static void SetCurrentSceneIndex()
        {
            CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
        
        public static void SetCurrentDataTime()
        {
            string dataFormat = "dd-MM-yyyy HH:mm:ss.fff";
            CurrentDataTime = DateTime.Now.ToString(format: dataFormat);
        }
    }
}