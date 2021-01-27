using System;
using UnityEngine.SceneManagement;

namespace DefaultNamespace.DAO
{
    public static class SqlDataConnection
    {
        private static string _dataFormat = "dd-MM-yyyy HH:mm:ss.fff";
        
        internal static string DBPath = $"URI=file:data.db";
        internal static int CurrentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        internal static string CurrentDataTime = DateTime.Now.ToString(format: _dataFormat);
    }
}