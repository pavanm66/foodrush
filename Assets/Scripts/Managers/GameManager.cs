using Foodrush.Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameStarted;
    public bool isGameOver;
    public bool isWinGame;
    public PlayerManager player;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
}
