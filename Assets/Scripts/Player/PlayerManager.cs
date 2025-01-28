using System.Collections.Generic;
using UnityEngine;

namespace Foodrush.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public List<GameObject> activeRunnersList = new List<GameObject>();

        private float spacingVariable = 0.8f;
        private Vector3 defaultPosition;


        [SerializeField] float forwardSpeed = 5f;
        [SerializeField] float dragSpeed = 10f;
        [SerializeField] Vector2 xLimits;  // Minimum and maximum x position
        [SerializeField] GameObject foodrunnerPrefab; // Running player food object
        [SerializeField] int foodItemIndex = 0; // index of the food item
        [SerializeField] List<Sprite> foodSprites;

        [SerializeField] List<GameObject> foodrunnersList;
        [SerializeField] bool isPlayerReady;

        public CameraMovement maincamera;

        private void Start()
        {
            defaultPosition = transform.position;
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
                transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

                // Drag to move along the x-axis
                if (Input.GetMouseButton(0) && !GameManager.instance.isWinGame)
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

                if (!GameManager.instance.isWinGame)
                {
                    GameManager.instance.isGameOver = (activeRunnersList.Count == 0);
                    if (GameManager.instance.isGameOver)
                    {
                        Time.timeScale = 0;
                    }
                   
                }
                else
                {
                    GameManager.instance.isCompletedGame = (activeRunnersList.Count == 0);
                }


                float scaleSpeed = 18f;   // Speed of the scale oscillation
                float minScale = 0.6f;   // Minimum Y scale
                float maxScale = 1f;     // Maximum Y scale

                float scaleFactor = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(Time.time * scaleSpeed) + 1) / 2);

                foreach (var runner in foodrunnersList)
                {
                    if (runner.gameObject.activeSelf)
                    {
                        // Apply the scale factor to the Y scale
                        Vector3 currentScale = runner.transform.localScale;
                        currentScale.y = scaleFactor;
                        runner.transform.localScale = currentScale;
                    }
                }

                // Handle runner-specific movement
                //foreach (var runner in foodrunnersList)
                //{
                //    Transform runnerTransform = runner.transform;

                //    // If the runner is on the ramp, skip jumping
                //    if (runnersOnRamp.Contains(runnerTransform)) continue;

                //    // Regular jumping motion
                //    float jumpHeight = 0.3f;
                //    float jumpSpeed = 15f;
                //    Vector3 position = runnerTransform.localPosition;
                //    position.y = Mathf.Sin(Time.time * jumpSpeed) * jumpHeight;
                //    runnerTransform.localPosition = position;
                //}

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

        public void Initialize()
        {
            GameManager.instance.isGameOver = false;
            GameManager.instance.isWinGame = false;
            GameManager.instance.isCompletedGame = false;
            isPlayerReady = false;
            gameObject.transform.position = defaultPosition;
            maincamera.ResetCamera();

            //change index here if required
            if (foodSprites.Count > 0)
            {
                if (foodrunnersList.Count > 0)
                {
                    foreach (var runner in foodrunnersList)
                    {
                        runner.GetComponentInChildren<SpriteRenderer>().sprite = foodSprites[foodItemIndex];
                    }
                }
            }
            foodrunnersList[0].gameObject.SetActive(true);
            activeRunnersList.Add(foodrunnersList[0]);
            foodrunnersList[0].gameObject.transform.localPosition = Vector3.zero;
            //print(Time.timeScale);
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
                        activeRunnersList.Add(runner);
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
                        activeRunnersList.Remove(runner);
                    }
                    break;

                case ObstacleType.Multiply:
                    foreach (var runner in runnerList)
                    {
                        Transform newPosition = CreateTransform();
                        runner.transform.localPosition = newPosition.localPosition;
                        runner.SetActive(true);
                        activeRunnersList.Add(runner);
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
            float hexRadius = activeRunners[0].GetComponentInChildren<SpriteRenderer>().bounds.size.x * spacingVariable; // Adjust for spacing
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
            newRunner.GetComponentInChildren<SpriteRenderer>().sprite = foodSprites[foodItemIndex];
            foodrunnersList.Add(newRunner);
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
                        activeRunnersList.Clear();
                       
                        // Trigger game over events
                        Debug.LogError("Game Over");
                       GameManager.instance.isGameOver = true;
                        
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WinningTrigger"))
            {
                CheckAndDestroy(other.gameObject);
            }
            if (other.CompareTag("Finish"))
            {
                GameManager.instance.isWinGame = true;
            }
        }

        void CheckAndDestroy(GameObject collidedObj)
        {
            if (activeRunnersList.Count > 0)
            {
                if (activeRunnersList.Count >= 12)
                {
                    int looptill = activeRunnersList.Count - 12;
                    for (int i = activeRunnersList.Count - 1; i > looptill; i--)
                    {
                        activeRunnersList[i].SetActive(false);
                        activeRunnersList.RemoveAt(i);

                    }
                    collidedObj.SetActive(false);
                }
                else
                {
                    for (int i = activeRunnersList.Count - 1; i >= 0; i--)
                    {
                        activeRunnersList[i].SetActive(false);
                        activeRunnersList.RemoveAt(i);

                    }
                    GameManager.instance.isCompletedGame = false;
                }

            }
            else
            {
                GameManager.instance.isCompletedGame = false;
            }
        }
    }
}
