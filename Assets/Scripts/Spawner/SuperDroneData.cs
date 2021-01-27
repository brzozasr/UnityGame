using System;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class SuperDroneData : GameObjectData
    {
        private float _shotTimeRangeFrom;
        private float _shotTimeRangeTo;
        private int _hitPoints;
        private float _maxMoveY;
        private float _moveSpeed;
        
        private SuperDroneController _superDroneController;

        //private SuperDroneDao _superDroneDao; 
        
        public SuperDroneData(GameObject drone, Vector3 position, List<string> parents, float shotTimeRangeFrom,
            float shotTimeRangeTo, int hitPoints, float maxMoveY, float moveSpeed) : base(drone, position, parents)
        {
            _superDroneController = drone.GetComponent<SuperDroneController>();
            
            _shotTimeRangeFrom = shotTimeRangeFrom;
            _shotTimeRangeTo = shotTimeRangeTo;
            _hitPoints = hitPoints;
            _maxMoveY = maxMoveY;
            _moveSpeed = moveSpeed;
        }
        
        public override void UpdateGoData()
        {
            _superDroneController.shootTimeRangeFrom = _shotTimeRangeFrom;
            _superDroneController.shootTimeRangeTo = _shotTimeRangeTo;
            _superDroneController.hitPoints = _hitPoints;
            _superDroneController.maxMoveY = _maxMoveY;
            _superDroneController.moveSpeed = _moveSpeed;
        }
        
        public static (Vector3, float, float, int, float, float) GetSuperDroneParameters(Transform superDrone)
        {
            SuperDroneController droneController = superDrone.gameObject.GetComponent<SuperDroneController>();
            
            Vector3 position = superDrone.position;
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
            sb.Append($"ShotTimeRangeFrom: {_shotTimeRangeFrom.ToString()}; ");
            sb.Append($"ShotTimeRangeTo: {_shotTimeRangeTo.ToString()}; ");
            sb.Append($"HitPoints: {_hitPoints.ToString()}; ");

            return sb.ToString();
        }
    }
}