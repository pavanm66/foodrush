using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Foodrush
{

    public class UIManager : MonoBehaviour
    {

        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject quitPanel;
        [SerializeField] private GameObject winOrLosePanel;
        [SerializeField] private GameObject loseButton;
        [SerializeField] private GameObject winButton;
        [SerializeField] private RectTransform dragText;
        [SerializeField] private TextMeshProUGUI winOrLoseText;

        [SerializeField] Image loadingImage;
        [SerializeField] GameObject loadingPanel;

        [SerializeField] List<GameObject> levelsList;
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
      

        public void Initialise()
        {
            Debug.Log(" here is initialise");
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (startPanel != null) startPanel.SetActive(true);
            if (quitPanel != null) quitPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(false);
            if (winOrLosePanel != null) winOrLosePanel.SetActive(false);
            StartCoroutine(IAnimateText());
            if (!levelsList[1].activeSelf)
                levelsList[0].SetActive(true);
            else
            {
                levelsList[0].SetActive(false);
            }
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
                winOrLoseText.text = "You Lose";
                winButton.SetActive(false);
                loseButton.SetActive(true);
                startPanel.SetActive(false);
            }
            if (GameManager.instance.isCompletedGame)
            {
                winOrLosePanel.SetActive(true);
                winOrLoseText.text = "You Win";
                winButton.SetActive(true);
                loseButton.SetActive(false);
                startPanel.SetActive(false);
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
        public void LoadNextLevel()
        {
            StartCoroutine(ILoadNextLevel());
        }
        [SerializeField] float maxTime = 5f;
        [SerializeField] float time = 0f;
        IEnumerator ILoadNextLevel()
        {
            GameManager.instance.player.gameObject.SetActive(false);
            loadingPanel.SetActive(true);
            while (time < maxTime)
            {
                time += Time.deltaTime;
                loadingImage.fillAmount = time / maxTime;

                yield return new WaitForSeconds(Time.deltaTime);
            }
            loadingPanel.SetActive(false);
            //winOrLosePanel.SetActive(false);
            //startPanel.SetActive(true);
            levelsList[1].SetActive(true);
           GameManager.instance.InitializeGame();
          //  GameManager.instance.player.gameObject.SetActive(true);
            yield return new WaitForSeconds(Time.deltaTime);
        }

    }
}
