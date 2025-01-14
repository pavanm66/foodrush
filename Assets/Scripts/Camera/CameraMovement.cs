using Foodrush.Player;
using UnityEngine;

namespace Foodrush
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform player; // Reference player object
        private Vector3 offset;   // Offset between the camera and the player

        private float followSpeed = 5f; // Speed of camera adjustment

        private void Awake()
        {
            if (!player)
            {
                player = FindFirstObjectByType<PlayerManager>().transform;
                Debug.LogWarning("player is not assigned for the Camera, Auto assigning the player");
            }
        }

        void Start()
        {
            if (player != null)
            {
                // Initialize the offset based on initial positions
                offset = transform.position - player.position;
            }
        }


        void LateUpdate()
        {
            if (!player) return;
            transform.position = player.position + offset;  

        }
    }
}