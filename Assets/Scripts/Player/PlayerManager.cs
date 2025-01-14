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

        private GameObject spawnedRunner;

        private void Start()
        {
            //change index here if required
            spawnedRunner = Instantiate(foodrunnerPrefab, gameObject.transform);
            if (foodSprites.Count > 0)
            {
                spawnedRunner.GetComponent<SpriteRenderer>().sprite = foodSprites[foodItemIndex];
            }

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



                if (Input.GetKeyDown(KeyCode.D))
                {
                    Duplicate();
                }
            }
        }

        void Duplicate()
        {
            // figure out the position of spawn of the duplicate.
            var foodrunner = Instantiate(foodrunnerPrefab, gameObject.transform);


        }
    }
}
