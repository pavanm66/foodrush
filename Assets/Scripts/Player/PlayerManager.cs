using System.Collections;
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

        [SerializeField] List<GameObject> foodrunnersList;
        [SerializeField] bool isPlayerReady;
        [SerializeField] bool isPlayerOnRamp;
        private float spacingVariable = 0.8f;
        private HashSet<Transform> runnersOnRamp = new(); // Track runners on the ramp



        //test code
        [SerializeField] Board board1;
        [SerializeField] Board board2;
        [SerializeField] Board board3;
        [SerializeField] bool isPlayerJumping;


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

        void Initialize()
        {

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
            foodrunnersList[0].gameObject.transform.localPosition = Vector3.zero;
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

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("RampStart"))
        //    {
        //        // Disable jumping for runners entering the ramp
        //        foreach (var runner in foodrunnersList)
        //        {
        //            if (runner.activeSelf)
        //                if (other.bounds.Contains(runner.transform.position))
        //                {
        //                    if(!runnersOnRamp.Contains(runner.transform)) 
        //                        runnersOnRamp.Add(runner.transform);
        //                }
        //        }
        //    }
        //}

        //private void OnTriggerExit(Collider other)
        //{
        //    if (other.CompareTag("RampEnd"))
        //    {
        //        // Re-enable jumping for runners exiting the ramp
        //        foreach (var runner in foodrunnersList)
        //        {
        //            if (runner.activeSelf)
        //            if (other.bounds.Contains(runner.transform.position))
        //            {
        //                if (runnersOnRamp.Contains(runner.transform))
        //                    runnersOnRamp.Remove(runner.transform);
        //                }
        //        }
        //    }
        //}


        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Ramp"))
        //    {
        //        // Check if any runner is touching the ramp
        //        foreach (var runner in foodrunnersList)
        //        {
        //            if (other.bounds.Contains(runner.transform.position))
        //            {
        //                HandleRunnerRampCrossing(runner.transform, other.transform);
        //            }
        //        }
        //    }
        //}


        //private void HandleRunnerRampCrossing(Transform runner, Transform ramp)
        //{
        //    // Skip if the runner is already crossing
        //    if (runnersCrossingRamp.Contains(runner)) return;

        //    StartCoroutine(RampCrossRoutine(runner, ramp));
        //}

        //private IEnumerator RampCrossRoutine(Transform runner, Transform ramp)
        //{
        //    runnersCrossingRamp.Add(runner); // Mark as crossing

        //    // Calculate ramp crossing positions
        //    Vector3 startPos = runner.position;
        //    Vector3 midPos = ramp.position + Vector3.up * rampCrossHeight; // Midpoint at ramp height
        //    Vector3 endPos = ramp.position + Vector3.forward * 2f; // End slightly ahead of ramp

        //    float elapsedTime = 0f;
        //    float duration = rampCrossSpeed;

        //    // Smoothly move runner over the ramp
        //    while (elapsedTime < duration)
        //    {
        //        float t = elapsedTime / duration;

        //        // Move runner through quadratic curve (up and down)
        //        runner.position = Vector3.Lerp(
        //            Vector3.Lerp(startPos, midPos, t),
        //            Vector3.Lerp(midPos, endPos, t),
        //            t
        //        );

        //        elapsedTime += Time.deltaTime;
        //        yield return null;
        //    }

        //    // Ensure final position
        //    runner.position = endPos;

        //    // Remove runner from ramp crossing set
        //    runnersCrossingRamp.Remove(runner);
        //}

    }
}
