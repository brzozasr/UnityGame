using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class PlayerData : GameObjectData
    {
        private float _livePoints;
        private int _liveNumber;

        private Player _player;
        
        public PlayerData(GameObject player, Vector3 position, List<string> parents, float livePoints, int liveNumber) :
            base(player, position, parents)
        {
            _player = player.GetComponent<Player>();
            
            _livePoints = livePoints;
            _liveNumber = liveNumber;
        }
        
        public override void UpdateGoData()
        {
            _player.livePoints = _livePoints;
            _player.liveNumber = _liveNumber;
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
            sb.Append($"Live points: {_livePoints.ToString()}; ");
            sb.Append($"Live number: {_liveNumber.ToString()}; ");

            return sb.ToString();
        }
    }
}