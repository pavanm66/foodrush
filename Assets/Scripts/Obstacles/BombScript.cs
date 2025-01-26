using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
   public ObstacleType type;
    [SerializeField] private float blastDelay = 0.5f; // Delay before the explosion
    //[SerializeField] private ParticleSystem explosionEffect; // Optional explosion effect

   [SerializeField] private List<GameObject> collidingObjects = new List<GameObject>(); // To track player objects hitting the bomb

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is a player object
        if (other.CompareTag("Runner"))
        {
            // Add the player object to the list of colliding objects
            if (!collidingObjects.Contains(other.gameObject))
            {
                collidingObjects.Add(other.gameObject);
            }
            Debug.Log(collidingObjects.Count + " is colliding count ");
            // Start the explosion process
            TriggerExplosion();
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    // Remove the object from the colliding objects list if it exits the collider
    //    if (other.CompareTag("Runner") && collidingObjects.Contains(other.gameObject))
    //    {
    //        collidingObjects.Remove(other.gameObject);
    //    }
    //}

    private void TriggerExplosion()
    {
        // Play the explosion effect
        //if (explosionEffect != null)
        //{
        //    Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //}

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
                Debug.Log(obj.name + "  is name ");
            }
        }

        // Clear the list of colliding objects
        //collidingObjects.Clear();

        // Destroy the bomb after the explosion
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
