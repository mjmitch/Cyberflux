using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class GameManager : MonoBehaviour
{
    public SceneFader fader; 
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
    [SerializeField] GameObject menuItemSelect;
    [SerializeField] public ItemSelection itemSelectionObject;

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


    // item stuff
    [SerializeField] public List<Augment> itemPool;
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


        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (menuItemSelect)
            {

            }
            else
            {
                if (!isPaused) GameStatePause();
                else GameStateResume();
            }
        }

    }

    public void GameStatePause()
    {
        if (isPaused) return;
        
        isPaused = true;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (menuPause) menuPause.SetActive(true);
        menuActive = menuPause;

        // If Options is open, make sure it blocks input while paused
        if (OptionPanel)
        {
            OptionPanel.alpha = OptionPanel.alpha; // no visual change, just keep state
            OptionPanel.blocksRaycasts = true;
        }

    }

    public void GameStateResume()
    {
        if (!isPaused) return;
if (menuItemSelect)
        {
            menuItemSelect.SetActive(false);
           // menuActive.SetActive(false);
        }
        // Hide pause and any active menu
        if (menuActive) menuActive.SetActive(false);
        if (menuPause) menuPause.SetActive(false);

        // Hide options panel if it was opened from pause
        if (OptionPanel)
        {
            OptionPanel.alpha = 0f;
            OptionPanel.blocksRaycasts = false;
        }

        isPaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        menuActive = null;
    }

    public void StartGame()
    {
        if (fader)
            fader.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (menuLose) menuLose.SetActive(false);
        if (menuWin) menuWin.SetActive(true);
        menuActive = menuWin;
    }

    public void YouLose()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (menuWin) menuWin.SetActive(false);
        if (menuLose) menuLose.SetActive(true);
        menuActive = menuLose;
    }

    public void ContinueFromWin()   // Next Level
    {
        // hide UI & resume before scene change
        if (menuWin) menuWin.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        int next = SceneManager.GetActiveScene().buildIndex + 1;
        int last = SceneManager.sceneCountInBuildSettings - 1;

        if (next <= last) LoadLevel(next);
        else LoadLevel(0);   // loop back to Title when out of levels
    }

    public void RestartLevel()
    {
        if (menuWin) menuWin.SetActive(false);
        if (menuLose) menuLose.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToTitle()
    {
        if (menuWin) menuWin.SetActive(false);
        if (menuLose) menuLose.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        LoadLevel(0); // Title Screen index
    }


    public void UpdateUI()
    {
       
    }

    public void LoadLevel(int lvl)
    {
        if (fader)
            fader.FadeToScene(lvl);
        else
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
                miliseconds -= 1;
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
    public void SelectItem()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        menuItemSelect.SetActive(true);
    }
}
