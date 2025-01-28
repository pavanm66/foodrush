using Foodrush.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Foodrush
{
    public class ObstacleScript : MonoBehaviour
    {
        public PlayerManager player;
        [SerializeField] List<GameObject> triggeredObjects = new List<GameObject>();
        [SerializeField] List<BoxCollider> sideBySideColliders = new List<BoxCollider>();
        [SerializeField] int count;

        public void GetActiveObjectsCount(GameObject PlayerObject)
        {
            triggeredObjects.Add(PlayerObject);
            Debug.Log(triggeredObjects.Count + " are triggered");
            for (int i = 0; i < triggeredObjects.Count; i++)
            {
                triggeredObjects[i].SetActive(false);
                player.activeRunnersList.Remove(triggeredObjects[i]);
            }


            //triggeredObjects.Clear();    
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            //foreach (BoxCollider _collider in sideBySideColliders)
            //{
            //    Debug.Log($"Collider state before: {_collider.enabled}");
            //    _collider.enabled = false;
            //    Debug.Log($"Collider state after: {_collider.enabled}");
            //}

        }
    }
    public enum ObstacleType
    {
        Chain,
        Addition,
        Multiply,
        Subtract,
        Bomb,
        None
    }
}
