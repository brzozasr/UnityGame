using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class SuperDroneData : GameObjectData
    {
        public float ShotTimeRangeFrom;
        public float ShotTimeRangeTo;
        public int HitPoints;
        public float MaxMoveY;
        public float MoveSpeed;
        
        private SuperDroneController _superDroneController;

        //private SuperDroneDao _superDroneDao; 
        
        public SuperDroneData(GameObject drone, Vector3 position, List<string> parents, float shotTimeRangeFrom,
            float shotTimeRangeTo, int hitPoints, float maxMoveY, float moveSpeed) : base(drone, position, parents)
        {
            _superDroneController = drone.GetComponent<SuperDroneController>();
            
            ShotTimeRangeFrom = shotTimeRangeFrom;
            ShotTimeRangeTo = shotTimeRangeTo;
            HitPoints = hitPoints;
            MaxMoveY = maxMoveY;
            MoveSpeed = moveSpeed;
        }
        
        public override void UpdateGoData()
        {
            _superDroneController.shootTimeRangeFrom = ShotTimeRangeFrom;
            _superDroneController.shootTimeRangeTo = ShotTimeRangeTo;
            _superDroneController.hitPoints = HitPoints;
            _superDroneController.maxMoveY = MaxMoveY;
            _superDroneController.moveSpeed = MoveSpeed;
        }
        
        public static (Vector3, float, float, int, float, float) GetSuperDroneParameters(Transform superDrone)
        {
            SuperDroneController droneController = superDrone.gameObject.GetComponent<SuperDroneController>();
            
            Vector3 position = superDrone.localPosition;
            float shotTimeRangeFrom = droneController.shootTimeRangeFrom;
            float shotTimeRangeTo = droneController.shootTimeRangeTo;
            int hitPoints = droneController.hitPoints;
            float maxMoveY = droneController.maxMoveY;
            float moveSpeed = droneController.moveSpeed;

            return (position, shotTimeRangeFrom, shotTimeRangeTo, hitPoints, maxMoveY, moveSpeed);
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