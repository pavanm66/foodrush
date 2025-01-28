using Foodrush.Player;
using System.Collections.Generic;
using UnityEngine;

namespace Foodrush
{
    public class ObstacleScript : MonoBehaviour
    {
        public PlayerManager player;
        [SerializeField] List<GameObject> triggeredObjects = new List<GameObject>();
        [SerializeField] List<GameObject> sideBySideColliders = new List<GameObject>();
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
     

        //public virtual void DisableColliders(GameObject obj)
        //{
        //    Debug.Log("here in disable colliders" + obj);
        //    if (obj.GetComponent<Obstacle>())
        //        foreach (GameObject _collider in sideBySideColliders)
        //        {
        //            Debug.Log("here ");
        //            //if (_collider.GetComponent<Obstacle>().obstacleType != ObstacleType.Chain)
        //                _collider.GetComponent<BoxCollider>().enabled = false;
        //        }

        //}
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
