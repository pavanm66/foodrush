using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject quitPanel;
    [SerializeField] private GameObject winOrLosePanel;

    private bool isGamePaused;
    public bool IsGamePaused
    {
        get
        {
            return isGamePaused;
        }
        set
        {
            isGamePaused = value;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(settingsPanel != null) settingsPanel.SetActive(false);
        if(startPanel != null) startPanel.SetActive(true);
        if(quitPanel != null) quitPanel.SetActive(false);
        if(pausePanel != null) pausePanel.SetActive(false);
        if(winOrLosePanel != null) winOrLosePanel.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           PauseGame();
        }
    }
    public void PauseGame()
    {
        IsGamePaused = !IsGamePaused;
        pausePanel.SetActive(IsGamePaused);
        if (IsGamePaused)
        {
            Time.timeScale = 0;  
        }
        else
        {
            Time.timeScale = 1;
        }
     
    }
    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
