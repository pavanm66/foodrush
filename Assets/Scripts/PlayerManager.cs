using UnityEngine;

namespace Foodrush.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public float forwardSpeed = 5f;
        public float dragSpeed = 10f;
        public Vector2 xLimits;  // Minimum and maximum x position
        private Vector3 dragStartPos;


        void Update()
        {
            // Automatic forward movement along the z-axis
            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

            // Drag to move along the x-axis
            if (Input.GetMouseButton(0))
            {
                // Get the mouse position in world space
                Vector3 mousePos = Input.mousePosition;
                mousePos.z = Camera.main.WorldToScreenPoint(transform.position).z; // Maintain z-position
                Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

                // Restrict the x-axis movement to within the limits
                targetPos.x = Mathf.Clamp(targetPos.x, xLimits.x, xLimits.y);
                targetPos.z = transform.position.z; // Ensure no unintended z-axis movement

                // Smoothly interpolate the player's position for drag effect
                transform.position = Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime);
            }
        }
    }
}
