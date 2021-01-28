using System.Collections.Generic;
using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Spawner
{
    public class PointWidgetData : GameObjectData
    {
        public int WidgetPoints;
        
        private PointWidgetController _pointWidgetController;
        
        public PointWidgetData(GameObject pointWidget, Vector3 position, List<string> parents, int widgetPoints) :
            base(pointWidget, position, parents)
        {
            _pointWidgetController = pointWidget.GetComponent<PointWidgetController>();
            
            WidgetPoints = widgetPoints;
        }
        
        public override void UpdateGoData()
        {
            _pointWidgetController.widgetPoints = WidgetPoints;
        }
        
        public static (Vector3, int) GetPointWidgetParameters(Transform pointWidget)
        {
            PointWidgetController pointWidgetController = pointWidget.gameObject.GetComponent<PointWidgetController>();
            
            Vector3 position = pointWidget.position;
            int widgetPoints = pointWidgetController.widgetPoints;

            return (position, widgetPoints);
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"Object name: {Go.name}; ");
            sb.Append($"WidgetPoints: {WidgetPoints.ToString()}; ");

            return sb.ToString();
        }
    }
}