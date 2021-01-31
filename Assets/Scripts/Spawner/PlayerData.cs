using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class PlayerData : GameObjectData
    {
        public int LivePoints;
        public int LiveNumber;

        private Player _player;
        
        public PlayerData(GameObject player, Vector3 position, List<string> parents, int livePoints, int liveNumber) :
            base(player, position, parents)
        {
            _player = player.GetComponent<Player>();
            
            LivePoints = livePoints;
            LiveNumber = liveNumber;
        }
        
        public override void UpdateGoData()
        {
            _player.actualLivePoints = LivePoints;
            _player.actualLiveNumber = LiveNumber;
        }
        
        public static (Vector3, int, int) GetPlayerParameters(Transform player)
        {
            Player playerController = player.gameObject.GetComponent<Player>();
            
            Vector3 position = player.localPosition;
            int livePoints = playerController.livePoints;
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