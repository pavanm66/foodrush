using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
   
    
   
    public void AddObstacle(int value)
    {
        Debug.Log("Enable these many objects");
    }

}
public enum ObstacleType
{
    Chain,
    Addition,
    Multiply,
    Subtract
}
