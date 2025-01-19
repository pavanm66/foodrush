using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameStarted;
    public bool isGameOver;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
}
