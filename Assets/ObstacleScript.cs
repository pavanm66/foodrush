using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public ObstacleType obstacleType;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum ObstacleType
{
    Chain,
    Addition,
    Multiply,
    Subtract
}
