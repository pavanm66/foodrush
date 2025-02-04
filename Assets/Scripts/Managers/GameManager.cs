using Foodrush;
using Foodrush.Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGameStarted;
    public bool isGameOver;
    public bool isWinGame;
    public bool isCompletedGame;
    public PlayerManager player;
    public UIManager uiManager;

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
    private void Start()
    {
        InitializeGame();
    }
    public void InitializeGame()
    {
        isGameOver = false;
        isWinGame = false;
        isCompletedGame = false;
        isGameStarted = false;
        player.gameObject.SetActive(true); 
        player.Initialize();
       uiManager.Initialise();

    }
}
