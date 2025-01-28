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
        triggeredObjects.Add(PlayerObject);
        Debug.Log(triggeredObjects.Count + " are triggered");
        for (int i = 0; i < triggeredObjects.Count; i++)
        {
            triggeredObjects[i].SetActive(false);
            player.activeRunnersList.Remove(triggeredObjects[i]);
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
