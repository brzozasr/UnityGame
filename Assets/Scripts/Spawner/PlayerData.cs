using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class PlayerData : GameObjectData
    {
        public float LivePoints;
        public int LiveNumber;

        private Player _player;
        
        public PlayerData(GameObject player, Vector3 position, List<string> parents, float livePoints, int liveNumber) :
            base(player, position, parents)
        {
            _player = player.GetComponent<Player>();
            
            LivePoints = livePoints;
            LiveNumber = liveNumber;
        }
        
        public override void UpdateGoData()
        {
            _player.livePoints = LivePoints;
            _player.liveNumber = LiveNumber;
        }
        
        public static (Vector3, float, int) GetPlayerParameters(Transform player)
        {
            Player playerController = player.gameObject.GetComponent<Player>();
            
            Vector3 position = player.position;
            float livePoints = playerController.livePoints;
            int liveNumber = playerController.liveNumber;

            return (position, livePoints, liveNumber);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"Live points: {LivePoints.ToString()}; ");
            sb.Append($"Live number: {LiveNumber.ToString()}; ");

            return sb.ToString();
        }
    }
}