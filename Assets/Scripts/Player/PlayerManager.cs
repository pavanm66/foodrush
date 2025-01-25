using System.Collections.Generic;
using UnityEngine;

namespace Foodrush.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] float forwardSpeed = 5f;
        [SerializeField] float dragSpeed = 10f;
        [SerializeField] Vector2 xLimits;  // Minimum and maximum x position
        [SerializeField] GameObject foodrunnerPrefab; // Running player food object
        [SerializeField] int foodItemIndex = 0; // index of the food item
        [SerializeField] List<Sprite> foodSprites;

        [SerializeField] List<SpriteRenderer> foodrunnersList;
        private float spacingVariable = 0.8f;

        //test code
        [SerializeField] Board board1;
        [SerializeField] Board board2;
        [SerializeField] Board board3;

        [SerializeField] bool isPlayerReady;

        private void Start()
        {
            Initialize();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isPlayerReady = true;

                GameManager.instance.isGameStarted = true;
            }
            if (isPlayerReady)
            {
                float jumpHeight = 0.5f; // Height of the jump
                float jumpSpeed = 18f;    // Speed of the jump

                // Apply a sinusoidal motion to the object's Y position for the jumping effect
                Vector3 position = transform.position;
                position.y = Mathf.Sin(Time.time * jumpSpeed) * jumpHeight;
                transform.position = position;


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
                    // Preserve y and z position of the object
                    targetPos.y = transform.position.y;
                    targetPos.z = transform.position.z;

                    // Smoothly interpolate the player's position for drag effect
                    transform.position = Vector3.Lerp(transform.position, targetPos, dragSpeed * Time.deltaTime);

                }
                //test code

                //if (Input.GetKeyDown(KeyCode.A))
                //{
                //    Populate(board1);
                //}
                //if (Input.GetKeyDown(KeyCode.S))
                //{
                //    Populate(board2);
                //}
                //if (Input.GetKeyDown(KeyCode.D))
                //{
                //    Populate(board3);
                //}
            }
        }

        void Initialize()
        {

            //change index here if required
            if (foodSprites.Count > 0)
            {
                if (foodrunnersList.Count > 0)
                {
                    foreach (var runner in foodrunnersList)
                    {
                        runner.sprite = foodSprites[foodItemIndex];
                    }
                }
            }
            foodrunnersList[0].gameObject.SetActive(true);
            foodrunnersList[0].gameObject.transform.position = Vector3.zero;
            Time.timeScale = 1;
        }

        public void Populate(Obstacle board)
        {
            var runnerList = CheckMeasures(board.obstacleType, board.value, out int requiredRunners);
            switch (board.obstacleType)
            {
                case ObstacleType.Addition:
                    foreach (var runner in runnerList)
                    {
                        Transform newPosition = CreateTransform();
                        runner.transform.localPosition = newPosition.localPosition;
                        runner.SetActive(true);
                    }

                    for (int i = 0; i < requiredRunners; i++)
                    {
                        Transform newPosition = CreateTransform();
                        CreateRunner(newPosition);
                    }
                    break;

                case ObstacleType.Subtract:
                    foreach (var runner in runnerList)
                    {
                        runner.SetActive(false);
                    }
                    break;

                case ObstacleType.Multiply:
                    foreach (var runner in runnerList)
                    {
                        Transform newPosition = CreateTransform();
                        runner.transform.localPosition = newPosition.localPosition;
                        runner.SetActive(true);
                    }

                    for (int i = 0; i < requiredRunners; i++)
                    {
                        Transform newPosition = CreateTransform();
                        CreateRunner(newPosition);
                    }
                    break;

                case ObstacleType.None:
                    break;
            }
        }

        private Transform CreateTransform()
        {
            // Gather all active runners
            List<GameObject> activeRunners = new();
            foreach (var runner in foodrunnersList)
            {
                if (runner.gameObject.activeSelf)
                    activeRunners.Add(runner.gameObject);
            }

            // Define hexagonal offsets (relative positions for six surrounding slots)
            float hexRadius = activeRunners[0].GetComponent<SpriteRenderer>().bounds.size.x * spacingVariable; // Adjust for spacing
            Vector3[] hexOffsets = new Vector3[]
            {
        new Vector3(0, 0, hexRadius),                     // Top
        new Vector3(hexRadius * Mathf.Sqrt(3) / 2, 0, hexRadius / 2),  // Top-right
        new Vector3(hexRadius * Mathf.Sqrt(3) / 2, 0, -hexRadius / 2), // Bottom-right
        new Vector3(0, 0, -hexRadius),                   // Bottom
        new Vector3(-hexRadius * Mathf.Sqrt(3) / 2, 0, -hexRadius / 2), // Bottom-left
        new Vector3(-hexRadius * Mathf.Sqrt(3) / 2, 0, hexRadius / 2)   // Top-left
            };

            // Check for empty slots around all active runners
            foreach (var runner in activeRunners)
            {
                foreach (var offset in hexOffsets)
                {
                    Vector3 potentialPosition = runner.transform.position + offset;

                    // Ensure the position doesn't overlap with existing objects
                    bool isOccupied = activeRunners.Exists(r => Vector3.Distance(r.transform.position, potentialPosition) < hexRadius * 0.9f);
                    if (!isOccupied)
                    {
                        // Clamp the position to stay within the xLimits
                        potentialPosition.x = Mathf.Clamp(potentialPosition.x, xLimits.x, xLimits.y);

                        // Create and return a new Transform at the calculated position
                        Transform newTransform = new GameObject("NewPosition").transform;
                        newTransform.position = potentialPosition;
                        newTransform.SetParent(transform); // Optional: Attach to parent for better organization
                        return newTransform;
                    }
                }
            }

            // If all slots around existing runners are filled, expand to a new layer
            foreach (var runner in activeRunners)
            {
                foreach (var offset in hexOffsets)
                {
                    Vector3 potentialPosition = runner.transform.position + offset * 2; // Expand to the next hexagonal layer

                    // Ensure the position doesn't overlap with existing objects
                    bool isOccupied = activeRunners.Exists(r => Vector3.Distance(r.transform.position, potentialPosition) < hexRadius * 0.9f);
                    if (!isOccupied)
                    {
                        // Clamp the position to stay within the xLimits
                        potentialPosition.x = Mathf.Clamp(potentialPosition.x, xLimits.x, xLimits.y);

                        // Create and return a new Transform at the calculated position
                        Transform newTransform = new GameObject("NewPosition").transform;
                        newTransform.position = potentialPosition;
                        newTransform.SetParent(transform); // Optional: Attach to parent for better organization
                        return newTransform;
                    }
                }
            }

            // Fallback: If no position is found, return the original position (should not happen in practice)
            Transform fallbackTransform = new GameObject("FallbackPosition").transform;
            fallbackTransform.position = transform.position;
            fallbackTransform.SetParent(transform);
            return fallbackTransform;
        }




        private void CreateRunner(Transform newPosition)
        {
            var newRunner = Instantiate(foodrunnerPrefab, newPosition);
            newRunner.GetComponent<SpriteRenderer>().sprite = foodSprites[foodItemIndex];
            foodrunnersList.Add(newRunner.GetComponent<SpriteRenderer>());
        }

        private List<GameObject> CheckMeasures(ObstacleType boardType, int boardValue, out int requiredRunners)
        {
            List<GameObject> activeRunners = new();
            List<GameObject> poolRunners = new();
            List<GameObject> spawningRunners = new();
            requiredRunners = 0;

            foreach (var runner in foodrunnersList)
            {
                if (runner.gameObject.activeSelf) activeRunners.Add(runner.gameObject);
                else poolRunners.Add(runner.gameObject);
            }

            switch (boardType)
            {
                case ObstacleType.Addition:
                    if (poolRunners.Count >= boardValue)
                    {
                        for (int i = 0; i < boardValue; i++)
                        {
                            spawningRunners.Add(poolRunners[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < poolRunners.Count; i++)
                        {
                            spawningRunners.Add(poolRunners[i]);
                        }
                        requiredRunners = boardValue - poolRunners.Count;
                    }
                    break;

                case ObstacleType.Subtract:
                    if (activeRunners.Count > boardValue)
                    {
                        for (int i = 0; i < boardValue; i++)
                        {
                            spawningRunners.Add(activeRunners[i]);
                        }
                    }
                    else
                    {
                        // Trigger game over events
                        Debug.LogError("Game Over");
                    }
                    break;

                case ObstacleType.Multiply:
                    if (poolRunners.Count >= activeRunners.Count * boardValue)
                    {
                        for (int i = 0; i < activeRunners.Count * boardValue; i++)
                        {
                            spawningRunners.Add(poolRunners[i]);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < poolRunners.Count; i++)
                        {
                            spawningRunners.Add(poolRunners[i]);
                        }
                        requiredRunners = (activeRunners.Count * boardValue) - poolRunners.Count;

                    }
                    break;

                case ObstacleType.None:
                    break;
            }

            return spawningRunners;
        }

        void DestroyObjects(int number)
        {
            // check if number is biggerthan the list of objects
            // if yes
            // disable the objects 
        }


    }
}
