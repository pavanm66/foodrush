using UnityEngine;

namespace Foodrush
{
    public class Obstacle : ObstacleScript
    {
        public ObstacleType obstacleType;
        public int value;
        [SerializeField] private bool isTriggered = false;
        
        //[SerializeField] int count;

        protected override void OnTriggerEnter(Collider other)
        {
            if (obstacleType != ObstacleType.Chain)
            {
                if (other.CompareTag("Runner"))
                {
                    if (!isTriggered)
                    {
                        base.OnTriggerEnter(other);
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
