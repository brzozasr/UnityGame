using UnityEngine;

namespace DefaultNamespace
{
    public class PointWidgetController : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f, Space.World);
        }
    }
}