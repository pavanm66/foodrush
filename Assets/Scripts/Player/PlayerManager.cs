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


                //test code
                if (Input.GetKeyDown(KeyCode.D))
                {
                    
                }
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
            // figure out the position of spawn of the duplicate.
            var runnerList = CheckMeasures(board.boardType, board.boardValue, out int requiredRunners);
            switch (board.boardType)
            {
                case BoardType.Addition:
                    break;
                case BoardType.Subtraction:
                    break;
                case BoardType.Multiply:
                    break;
                case BoardType.None:
                    break;
            }


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
                        
                    }
                    break;

                case BoardType.Multiply:
                    if (activeRunners.Count * boardValue >= poolRunners.Count)
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
