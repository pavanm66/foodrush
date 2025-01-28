using System.Collections;
using UnityEngine;

namespace Foodrush
{
    public class StrikeWheel : ObstacleScript
    {
        [SerializeField] float rotationSpeed = 100f; // Rotation speed in degrees per second
        [SerializeField] GameObject wheel;
        [SerializeField] private bool movingRight = true;
        [SerializeField] private GameObject originPoint;
        [SerializeField] private float speed;
        public StrikeWheelType wheelType;
        public ObstacleType obstacleType;

        void Update()
        {
            // Rotate the wheel around its Z-axis
            wheel.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Runner"))
            {
                GetActiveObjectsCount(other.gameObject);
            }
        }

        private void Start()
        {
            switch (wheelType)
            {
                case StrikeWheelType.still:
                    break;
                case StrikeWheelType.sideWays:
                    SideWaysMovement();
                    break;
                case StrikeWheelType.rotateAround:
                    RotateInArea();
                    break;
                default:
                    break;
            }

        }
        public void RotateInArea()
        {
            StartCoroutine(IRotateInArea());
        }
        IEnumerator IRotateInArea()
        {
            while (!GameManager.instance.isGameOver)
            {
                wheel.transform.RotateAround(originPoint.transform.position, Vector3.up, 0.5f);
                yield return null;
            }
        }

        public void SideWaysMovement()
        {
            StartCoroutine(IMoveSideWays());
        }
        IEnumerator IMoveSideWays()
        {
            while (!GameManager.instance.isGameOver)
            {
                if (wheel.transform.localPosition.x >= 3.3f)
                {
                    movingRight = false;
                }
                else if (wheel.transform.localPosition.x <= -3.3f)
                {
                    movingRight = true;
                }

                Vector3 movement = (movingRight ? Vector3.right : Vector3.left) * Time.deltaTime * speed;

                wheel.transform.localPosition += movement;
                yield return null;
            }

        }
    }
    public enum StrikeWheelType
    {
        still,
        sideWays,
        rotateAround
    }

}