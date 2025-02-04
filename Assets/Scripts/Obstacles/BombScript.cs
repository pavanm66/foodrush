using System.Collections.Generic;
using UnityEngine;

namespace Foodrush
{
    public class BombScript : ObstacleScript
    {
        public ObstacleType type;
        //[SerializeField] private GameObject player;          // Reference to the player object
        [SerializeField] private float blastDelay = 0f;    // Delay before the explosion
        [SerializeField] private List<GameObject> collidingObjects = new List<GameObject>(); // To track colliding objects
                                                                                             //[SerializeField] private ParticleSystem explosionEffect; // Optional explosion effect

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object is a player object
            if (other.CompareTag("Runner"))
            {
                // Add the player object to the list of colliding objects
                //if (!collidingObjects.Contains(other.gameObject))
                //{
                //    collidingObjects.Add(other.gameObject);
                //}
                Debug.Log(collidingObjects.Count + " is colliding count");

                GetActiveObjectsCount(other.gameObject);

                // Start the explosion process only if it hasn't started
                if (!IsInvoking(nameof(Explode)))
                {
                    TriggerExplosion();
                }
            }
        }

        private void TriggerExplosion()
        {
          

            // Wait for the blast delay, then explode
            Invoke(nameof(Explode), blastDelay);
        }

        private void Explode()
        {
            Debug.Log(collidingObjects.Count + " is count in explode");

            // Disable only the objects in the collidingObjects list
            foreach (GameObject obj in collidingObjects)
            {
                if (obj != null) // Ensure the object still exists
                {
                    obj.SetActive(false);
                    Debug.Log(obj.name + " is disabled");
                    player.activeRunnersList.Remove(obj);
                }
            }

            // Clear the list of colliding objects
           ClearTriggeredList();

            // Disable or destroy the bomb after the explosion
            gameObject.SetActive(false);
        }

        private bool IsGameOver(GameObject player)
        {
            int activeChildrenCount = 0;

            // Loop through all child objects of the player
            for (int i = 0; i < player.transform.childCount; i++)
            {
                // Check if the child GameObject is active
                if (player.transform.GetChild(i).gameObject.activeSelf)
                {
                    activeChildrenCount++;
                }
            }

            // Game is over if only one or no child is active
            return activeChildrenCount <= 1;
        }
    }
}
