using System.Collections.Generic;
using UnityEngine;

namespace Spawner
{
    public abstract class GameObjectData
    {
        public Vector3 Position { get; }
        public List<string> Parents { get; }
        public GameObject Go { get; }
            
        public GameObjectData(GameObject go, Vector3 position, List<string> parents)
        {
            Go = go;
            Parents = parents;
            Position = position;
        }

        public abstract void UpdateGoData();
    }
}