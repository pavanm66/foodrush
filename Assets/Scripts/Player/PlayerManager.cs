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

        //test code
        [SerializeField] Board board1;
        [SerializeField] Board board2;
        [SerializeField] Board board3;

        private void Start()
        {
            Initialize();
        }

        void Update()
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
            //test code

            if (Input.GetKeyDown(KeyCode.A))
            {
                Populate(board1);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Populate(board2);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Populate(board3);
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
        }

        void Populate(Board board)
        {
            var runnerList = CheckMeasures(board.boardType, board.boardValue, out int requiredRunners);
            switch (board.boardType)
            {
                case BoardType.Addition:
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

                case BoardType.Subtraction:
                    foreach (var runner in runnerList)
                    {
                        runner.SetActive(false);
                    }
                    break;

                case BoardType.Multiply:
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

                case BoardType.None:
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

            // Calculate the center position of the active runners
            Vector3 centerPosition = Vector3.zero;
            foreach (var runner in activeRunners)
            {
                centerPosition += runner.transform.position;
            }
            centerPosition /= activeRunners.Count;

            // Generate a random direction around the center
            Vector2 randomDirection = Random.insideUnitCircle.normalized; // Random 2D direction

            // Adjust distance based on sprite size and additional spacing
            float spriteWidth = activeRunners[0].GetComponent<SpriteRenderer>().bounds.size.x; // Use sprite width
            float distance = spriteWidth * 1.0f; // Add extra spacing (1.5x the sprite width)

            // Calculate the offset
            Vector3 offset = new Vector3(randomDirection.x, 0, randomDirection.y) * distance;

            // Calculate the new position
            Vector3 newPosition = centerPosition + offset;

            // Clamp the position to stay within the xLimits (and optional yLimits if applicable)
            newPosition.x = Mathf.Clamp(newPosition.x, xLimits.x, xLimits.y);

            // Create and return a new Transform at the calculated position
            Transform newTransform = new GameObject("NewPosition").transform;
            newTransform.position = newPosition;
            newTransform.SetParent(transform); // Optional: Attach to parent for better organization
            return newTransform;
        }




        private void CreateRunner(Transform newPosition)
        {
            var newRunner = Instantiate(foodrunnerPrefab, newPosition);
            newRunner.GetComponent<SpriteRenderer>().sprite = foodSprites[foodItemIndex];
            foodrunnersList.Add(newRunner.GetComponent<SpriteRenderer>());
        }

        private List<GameObject> CheckMeasures(BoardType boardType, int boardValue, out int requiredRunners)
        {
            List<GameObject> activeRunners = new();
            List<GameObject> poolRunners = new();
            List<GameObject> spawningRunners = new();
            requiredRunners = 0;

            foreach (var runner in foodrunnersList)
            {
                if(runner.gameObject.activeSelf) activeRunners.Add(runner.gameObject);
                else poolRunners.Add(runner.gameObject);
            }

            switch (boardType)
            {
                case BoardType.Addition:
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

                case BoardType.Subtraction:
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

                case BoardType.Multiply:
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

                case BoardType.None:
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
