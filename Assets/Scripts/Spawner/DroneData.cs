using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class DroneData : GameObjectData
    {
        private float _shotTimeRangeFrom;
        private float _shotTimeRangeTo;
        private int _hitPoints;

        private DroneController _droneController;
        

        public DroneData(GameObject drone, Vector3 position, List<string> parents, float shotTimeRangeFrom,
            float shotTimeRangeTo, int hitPoints) : base(drone, position, parents)
        {
            _droneController = drone.GetComponent<DroneController>();
            
            _shotTimeRangeFrom = shotTimeRangeFrom;
            _shotTimeRangeTo = shotTimeRangeTo;
            _hitPoints = hitPoints;
        }
        
        /// <summary>
        /// The method must be executed before spawning an object.
        /// The method updates 
        /// </summary>
        public override void UpdateGoData()
        {
            _droneController.shootTimeRangeFrom = _shotTimeRangeFrom;
            _droneController.shootTimeRangeTo = _shotTimeRangeTo;
            _droneController.hitPoints = _hitPoints;
        }
        
        public static (Vector3, float, float, int) GetDroneParameters(Transform drone)
        {
            DroneController droneController = drone.gameObject.GetComponent<DroneController>();
            
            Vector3 position = drone.position;
            float shotTimeRangeFrom = droneController.shootTimeRangeFrom;
            float shotTimeRangeTo = droneController.shootTimeRangeTo;
            int hitPoints = droneController.hitPoints;

            return (position, shotTimeRangeFrom, shotTimeRangeTo, hitPoints);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"ShotTimeRangeFrom: {_shotTimeRangeFrom.ToString()}; ");
            sb.Append($"ShotTimeRangeTo: {_shotTimeRangeTo.ToString()}; ");
            sb.Append($"HitPoints: {_hitPoints.ToString()}; ");

            return sb.ToString();
        }
    }
}