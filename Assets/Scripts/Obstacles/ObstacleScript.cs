using Foodrush.Player;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public PlayerManager player;
    [SerializeField] List<GameObject> triggeredObjects = new List<GameObject>();
    [SerializeField] int count;
   public void GetActiveObjectsCount(GameObject PlayerObject)
    {
        triggeredObjects.Add(PlayerObject.GetComponentInParent<GameObject>());
        Debug.Log(triggeredObjects.Count + " are triggered");
        for (int i = 0; i < triggeredObjects.Count; i++)
        {
            Debug.Log("inside loop");
            triggeredObjects[i].SetActive(false);
        }
        //triggeredObjects.Clear();    
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
