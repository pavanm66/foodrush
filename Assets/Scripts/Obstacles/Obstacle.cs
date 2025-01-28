using UnityEngine;

namespace Foodrush
{
    public class Obstacle : ObstacleScript
    {
        public ObstacleType obstacleType;
        public int value;
        public bool isTriggered = false;
        public GameObject secondGO;
        
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
                        this.gameObject.SetActive(false);
                        secondGO.GetComponent<BoxCollider>().enabled = false; 
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
