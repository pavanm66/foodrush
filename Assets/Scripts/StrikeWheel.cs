using UnityEngine;

namespace Foodrush
{
    public class StrikeWheel : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 100f; // Rotation speed in degrees per second
        [SerializeField] GameObject wheel;

        void Update()
        {
            // Rotate the wheel around its Z-axis
            wheel.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
}
