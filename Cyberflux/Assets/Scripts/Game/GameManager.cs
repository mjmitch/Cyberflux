using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    
    public GameObject player;

    public PlayerController playerScript;

    public bool isPaused;

    // UI Stuff
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public TMP_Text playerAmmoText;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] public GameObject bossHPUI; 
    [SerializeField] public Image bossHPBar;
    [SerializeField] public TMP_Text bossNameText;

    [SerializeField] CanvasGroup OptionPanel;

    int minutes;
    public TMP_Text TimerMinutes;
    int seconds;
    public TMP_Text TimerSeconds;
    float miliseconds;
    public TMP_Text TimerMiliseconds;
 
    enum GameState { Title, Playing, Win, Lose }
    GameState _state = GameState.Title;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

        //ShowTitle();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameStatePause();
        minutes = 0;
        seconds = 0;
        miliseconds = 0;
        UpdateTimerText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) {
            UpdateLevelTimer();
        }
        
    }

    public void GameStatePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GameStateResume()
    {
        isPaused = !isPaused;
        //Time.timeScale = timescaleOriginal;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }
        menuActive = null;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Assuming 1 is the main game scene
    }

    public void Option()
    {
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
    }
    public void Back()
    {
        OptionPanel.alpha= 0;
        OptionPanel.blocksRaycasts = false;
    }
     
    public void QuitGame()
    {
        Application.Quit();
       
    }
    
    public void YouWin()
    {
       
    }

    public void YouLose()
    {

    }

    public void UpdateUI()
    {
       
    }

    public void LoadLevel(int lvl)
    {
        SceneManager.LoadScene(lvl);
    }

    void ShowTitle()
    {
      
    }

    void UpdateLevelTimer()
    {
        if (!isPaused)
        {
            if (miliseconds < 1) {
                miliseconds += 1 * Time.deltaTime;
            }
            else if (seconds < 59)
            {
                miliseconds = 0;
                seconds += 1;
            }
            else if (minutes < 59)
            {
                minutes += 1;
                seconds = 0;
                miliseconds = 0;
            }
            else
            {
                TimerMiliseconds.text = "99";
            }
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        float temp = miliseconds % 1 * 100;
        int tempmiliseconds = (int)temp;
        TimerMiliseconds.text = tempmiliseconds.ToString();

        if (seconds < 10)
            TimerSeconds.text = "0" + seconds.ToString() + ".";
        else
            TimerSeconds.text = seconds.ToString() + ".";

        if (minutes < 10)
            TimerMinutes.text = "0" + minutes.ToString() + ".";
        else
            TimerMinutes.text = minutes.ToString() + ".";
    }
}
