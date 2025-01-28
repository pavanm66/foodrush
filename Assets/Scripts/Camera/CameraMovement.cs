using Foodrush.Player;
using UnityEngine;

namespace Foodrush
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform player; // Reference player object
        [SerializeField] private float cameraTransitionDuration = 1.5f;
        private float cameraTransitionTimer = 0f;
        private Vector3 offset;   // Offset between the camera and the player
        private Vector3 defaultPosition;
        private Quaternion defaultRotation;

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
                defaultPosition = transform.position;
                defaultRotation = transform.rotation;
            }
        }

        void LateUpdate()
        {
            if (!player) return;
            if (GameManager.instance.isWinGame)
            {
                if (GameManager.instance.isWinGame)
                {
                    cameraTransitionTimer += Time.deltaTime;

                    // Calculate progress (clamp between 0 and 1)
                    float progress = Mathf.Clamp01(cameraTransitionTimer / cameraTransitionDuration);

                    // Target position (only change Y to 2, keep X and Z the same)
                    Vector3 targetPosition = new Vector3(gameObject.transform.position.x, player.position.y, gameObject.transform.position.z);

                    // Smoothly interpolate position
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, targetPosition, progress);

                    // Target rotation (only change X rotation to 0, keep other rotations the same)
                    Quaternion targetRotation = Quaternion.Euler(
                        0f,                                  // Set X rotation to 0
                        gameObject.transform.rotation.eulerAngles.y, // Keep Y rotation the same
                        gameObject.transform.rotation.eulerAngles.z  // Keep Z rotation the same
                    );

                    // Smoothly interpolate rotation
                    gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, targetRotation, progress);

                    // Ensure we stop updating after the transition is complete
                    if (progress >= 1f)
                    {
                        //Debug.LogError("camera completed");
                        //transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, player.position.z + gameObject.transform.position.z);
                    }
                }
            }
            else
            {
                transform.position = new Vector3(offset.x, offset.y, player.position.z + offset.z);
            }
        }

        public void UpdateOffset(Vector3 _offset)
        {
            offset = _offset;
        }

        public void ResetCamera()
        {
            transform.position = defaultPosition;
            transform.rotation = defaultRotation;
        }
    }
}