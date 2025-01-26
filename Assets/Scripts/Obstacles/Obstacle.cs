using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
namespace Foodrush
{
    public class Obstacle : ObstacleScript
    {
        public ObstacleType obstacleType;
        public int value;
        [SerializeField] private bool isTriggered;
        //[SerializeField] int count;

        private void OnTriggerEnter(Collider other)
        {
            if (obstacleType != ObstacleType.Chain)
            {
                if (other.CompareTag("Runner"))
                {
                    if (!isTriggered)
                    {
                        PopulateObjects();
                    }
                }
            }
           
        }
      
        void PopulateObjects()
        {
            isTriggered = true;
            Debug.Log("triggered the player");
            player.Populate(this);
        }

    }
}
