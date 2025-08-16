using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Audio;


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
    public Image playerStaminaBar;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuItemSelect;
    [SerializeField] GameObject menuItemUnlock;
    [SerializeField] public ItemSelection itemSelectionObject;
    [SerializeField] public ItemUnlock itemUnlockObject;

    [SerializeField] public GameObject bossHPUI; 
    [SerializeField] public Image bossHPBar;
    [SerializeField] public TMP_Text bossNameText;

    [SerializeField] CanvasGroup OptionPanel;
    [SerializeField] public GameObject optionsControls;
    [SerializeField] public GameObject optionsAudio;

    [SerializeField] public AudioSource UIAudioSource;
    [SerializeField] public AudioMixer audioMixer;
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

    private int score;
    
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
           var p = GameObject.FindWithTag("Player");
    if (p != null)
    {
        player = p;
        playerScript = p.GetComponent<PlayerController>();
    }
    else
    {
        player = null;
        playerScript = null;
    }

    score = 0;
    //ShowTitle();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameStatePause();
        audioMixer.SetFloat("masterVolume", playerScript.masterVol);
        audioMixer.SetFloat("sfxVolume", playerScript.sfxVol);
        audioMixer.SetFloat("musicVolume", playerScript.musicVol);
        menuItemSelect.SetActive(false);
        menuItemUnlock.SetActive(false);
        OptionPanel.gameObject.SetActive(false);
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
            if (menuItemSelect.active || menuItemUnlock.active)
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
        }
if (menuItemUnlock)
        {
            menuItemUnlock.SetActive(false);
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
        UIAudioSource.Play();
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
        UIAudioSource.Play();
        OptionPanel.gameObject.SetActive(true);
        OptionPanel.alpha = 1;
        OptionPanel.blocksRaycasts = true;
        optionsControls.SetActive(true);
        optionsAudio.SetActive(false);

    }
    public void Back()
    {
        UIAudioSource.Play();
        OptionPanel.gameObject.SetActive(false);
        OptionPanel.alpha= 0;
        OptionPanel.blocksRaycasts = false;
    }
     
    public void QuitGame()
    {
        UIAudioSource.Play();
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
        UIAudioSource.Play();
        if (menuWin) menuWin.SetActive(false);
        if (menuLose) menuLose.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToTitle()
    {
        UIAudioSource.Play();
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
                if (playerScript.brokenClock && Random.Range(1,101) <= 10)
                {
                    miliseconds -= 1;
                    seconds -= 1;
                    
                    if (seconds < 0 && minutes > 0)
                    {

                        seconds = 59;
                        minutes -= 1;
                    }
                    else if (seconds < 0 && minutes < 0)
                    {
                        seconds -= 1;
                    }
                   
                }
                else
                {
                    miliseconds -= 1;
                    seconds += 1;
                }
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

        if (minutes < 10 && minutes >= 0)
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
    public void UnlockItem()
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
       
        menuItemUnlock.SetActive(true);
    }


    public void UpdateHealthUI(int currentHP, int maxHP)
    {
        if (playerHPBar != null)
            playerHPBar.fillAmount = (float)currentHP / maxHP;

        if (playerHPText != null)
            playerHPText.text = currentHP + " / " + maxHP;
    }

    public void UpdateStaminaUI(float currentStamina, float maxStamina)
    {
        if (playerStaminaBar != null)
            playerStaminaBar.fillAmount = currentStamina / maxStamina;
    }
}
