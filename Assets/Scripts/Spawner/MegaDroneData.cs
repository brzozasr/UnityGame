using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class MegaDroneData : GameObjectData
    {
        public float ShotTimeRangeFrom;
        public float ShotTimeRangeTo;
        public int HitPoints;
        public float MaxMoveY;
        public float MoveSpeed;
        public float MaxMoveX;

        private MegaDroneController _megaDroneController;
        
        public MegaDroneData(GameObject drone, Vector3 position, List<string> parents, float shotTimeRangeFrom,
            float shotTimeRangeTo, int hitPoints, float maxMoveY, float moveSpeed, float maxMoveX) : base(drone, position, parents)
        {
            _megaDroneController = drone.GetComponent<MegaDroneController>();
            
            ShotTimeRangeFrom = shotTimeRangeFrom;
            ShotTimeRangeTo = shotTimeRangeTo;
            HitPoints = hitPoints;
            MaxMoveY = maxMoveY;
            MoveSpeed = moveSpeed;
            MaxMoveX = maxMoveX;
        }
        
        public override void UpdateGoData()
        {
            _megaDroneController.shootTimeRangeFrom = ShotTimeRangeFrom;
            _megaDroneController.shootTimeRangeTo = ShotTimeRangeTo;
            _megaDroneController.hitPoints = HitPoints;
            _megaDroneController.maxMoveY = MaxMoveY;
            _megaDroneController.moveSpeed = MoveSpeed;
            _megaDroneController.maxMoveX = MaxMoveX;
        }
        
        public static (Vector3, float, float, int, float, float, float) GetMegaDroneParameters(Transform megaDrone)
        {
            MegaDroneController megaDroneController = megaDrone.gameObject.GetComponent<MegaDroneController>();
            
            Vector3 position = megaDrone.localPosition;
            float shotTimeRangeFrom = megaDroneController.shootTimeRangeFrom;
            float shotTimeRangeTo = megaDroneController.shootTimeRangeTo;
            int hitPoints = megaDroneController.hitPoints;
            float maxMoveY = megaDroneController.maxMoveY;
            float moveSpeed = megaDroneController.moveSpeed;
            float maxMoveX = megaDroneController.maxMoveX;

            return (position, shotTimeRangeFrom, shotTimeRangeTo, hitPoints, maxMoveY, moveSpeed, maxMoveX);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"ShotTimeRangeFrom: {ShotTimeRangeFrom.ToString()}; ");
            sb.Append($"ShotTimeRangeTo: {ShotTimeRangeTo.ToString()}; ");
            sb.Append($"HitPoints: {HitPoints.ToString()}; ");
            sb.Append($"MaxMoveY: {MaxMoveY.ToString()}; ");
            sb.Append($"MoveSpeed: {MoveSpeed.ToString()}; ");
            sb.Append($"MaxMoveX: {MaxMoveX.ToString()}; ");

            return sb.ToString();
        }
    }
}