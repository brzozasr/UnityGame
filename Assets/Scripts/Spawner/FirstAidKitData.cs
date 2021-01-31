using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class FirstAidKitData : GameObjectData
    {
        public int HitPointRecovery;

        private FirstAidKitController _firstAidKitController;
        
        public FirstAidKitData(GameObject firstAidKit, Vector3 position, List<string> parents, int hitPointRecovery) :
            base(firstAidKit, position, parents)
        {
            _firstAidKitController = firstAidKit.GetComponent<FirstAidKitController>();
            
            HitPointRecovery = hitPointRecovery;
        }
        
        public override void UpdateGoData()
        {
            _firstAidKitController.hitPointRecovery = HitPointRecovery;
        }
        
        public static (Vector3, int) GetFirstAidKitParameters(Transform firstAidKit)
        {
            FirstAidKitController firstAidKitController = firstAidKit.gameObject.GetComponent<FirstAidKitController>();
            
            Vector3 position = firstAidKit.localPosition;
            int hitPointRecovery = firstAidKitController.hitPointRecovery;

            return (position, hitPointRecovery);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"HitPointRecovery: {HitPointRecovery.ToString()}; ");

            return sb.ToString();
        }
    }
}