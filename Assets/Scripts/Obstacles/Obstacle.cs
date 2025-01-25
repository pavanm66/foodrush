using JetBrains.Annotations;
using UnityEngine;
namespace Foodrush
{
    public class Obstacle : ObstacleScript
    {
        public ObstacleType obstacleType;
        public int value;
        [SerializeField] private bool isTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (!isTriggered)
                {
                    isTriggered = true;
                    Debug.Log("triggered the player");
                    player.Populate(this);

                }
            }
        }

    }
}
