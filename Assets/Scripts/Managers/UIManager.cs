using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Foodrush
{

    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject quitPanel;
        [SerializeField] private GameObject winOrLosePanel;
        [SerializeField] private RectTransform dragText;

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
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (startPanel != null) startPanel.SetActive(true);
            if (quitPanel != null) quitPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (winOrLosePanel != null) winOrLosePanel.SetActive(false);
            StartCoroutine(IAnimateText());
        }



        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
            if (GameManager.instance.isGameOver)
            {
                winOrLosePanel.SetActive(true);
            }
            if (GameManager.instance.isGameStarted) 
            { 
                StopCoroutine(IAnimateText()); 
                dragText.gameObject.SetActive(false); 
                startPanel.SetActive(false);    
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
        [SerializeField] bool isClicked;
        [SerializeField] private bool movingRight = true; 

        IEnumerator IAnimateText()
        {
            float speed = 150f;
            print(dragText.transform.localPosition.x + " is x pos");
            while (!isClicked && dragText.gameObject.activeSelf)
            {
                if (dragText.transform.localPosition.x >= 370f)
                {
                    movingRight = false; 
                }
                else if (dragText.transform.localPosition.x <= -370f)
                {
                    movingRight = true;
                }

                Vector3 movement = (movingRight ? Vector3.right : Vector3.left) * Time.deltaTime * speed;

                dragText.transform.localPosition += movement;
                yield return null;
            }
        }


    }
}
