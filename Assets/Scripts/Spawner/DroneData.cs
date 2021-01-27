using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class DroneData : GameObjectData
    {
        public float ShotTimeRangeFrom { get; private set; }
        public float ShotTimeRangeTo { get; private set; }
        public int HitPoints { get; private set; }

        private DroneController _droneController;
        

        public DroneData(GameObject drone, Vector3 position, List<string> parents, float shotTimeRangeFrom,
            float shotTimeRangeTo, int hitPoints) : base(drone, position, parents)
        {
            _droneController = drone.GetComponent<DroneController>();
            
            ShotTimeRangeFrom = shotTimeRangeFrom;
            ShotTimeRangeTo = shotTimeRangeTo;
            HitPoints = hitPoints;
        }

        /// <summary>
        /// The method must be executed before spawning an object.
        /// The method updates 
        /// </summary>
        public override void UpdateGoData()
        {
            _droneController.shootTimeRangeFrom = ShotTimeRangeFrom;
            _droneController.shootTimeRangeTo = ShotTimeRangeTo;
            _droneController.hitPoints = HitPoints;
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
            sb.Append($"ShotTimeRangeFrom: {ShotTimeRangeFrom.ToString()}; ");
            sb.Append($"ShotTimeRangeTo: {ShotTimeRangeTo.ToString()}; ");
            sb.Append($"HitPoints: {HitPoints.ToString()}; ");

            return sb.ToString();
        }
    }
}