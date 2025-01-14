using UnityEngine;

namespace Foodrush
{
    public class StrikeWheel : MonoBehaviour
    {
        public float rotationSpeed = 100f; // Rotation speed in degrees per second
        public GameObject wheel;
        void Update()
        {
            // Rotate the wheel around its Z-axis
            wheel.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
