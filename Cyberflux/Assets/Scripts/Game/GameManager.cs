using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using UnityEngine.Audio;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public SceneFader fader; 
    public static GameManager instance;
    
    public GameObject player;
    
    public PlayerController playerScript;

    public bool isPaused;

    public PlayerStats stats;
    // UI Stuff
    public Image playerHPBar;
    public TMP_Text playerHPText;
    public TMP_Text playerAmmoText;
    public Image playerStaminaBar;
    [SerializeField] private TMPro.TMP_Text winSummaryText;
    [SerializeField] private TMPro.TMP_Text loseSummaryText;
    [SerializeField] private TMPro.TMP_Text deathReasonText;

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

    [SerializeField] TutorialPrompt tutorialPrompt;

    
    [SerializeField] public AudioSource UIAudioSource;
    [SerializeField] public AudioMixer audioMixer;

    [SerializeField]
     private List<string> tutorialMessages = new List<string>()
     {
    "Press ⭼ (Middle-click) to unleash Momentum’s surge.",
    "Press LMB (Left-click) to strike with your blade.",
    "Press RMB (Q) to throw the Scythe’s arc",
    "The Black gauge holds the Scythe’s charge.",
    "The Blue gauge fuels your momentum. It gives you acess to sliding, jumping, and dashing."
     };

    private float tutorialTimer = 0f;
    private int currentTutorialIndex = 0;

    [Header("Timers")]
    [HideInInspector] public int minutes;

    public TMP_Text TimerMinutes;

    [HideInInspector] public int seconds;

    public TMP_Text TimerSeconds;

    [HideInInspector] public float miliseconds;

    public TMP_Text TimerMiliseconds;
    public string levelCompleteTime;

    // item stuff
    [SerializeField] public List<Augment> itemPool;
   
    enum GameState { Title, Playing, Win, Lose }
    GameState _state = GameState.Title;

    public int score;
    
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
        if (playerScript != null)
        {
            audioMixer.SetFloat("masterVolume", playerScript.masterVol);
            audioMixer.SetFloat("sfxVolume", playerScript.sfxVol);
            audioMixer.SetFloat("musicVolume", playerScript.musicVol);
            menuItemSelect.SetActive(false);
            menuItemUnlock.SetActive(false);
            
            // Hide Options menu using CanvasGroup only
            OptionPanel.alpha = 0f;
            OptionPanel.interactable = false;
            OptionPanel.blocksRaycasts = false;

            minutes = 0;
            seconds = 0;
            miliseconds = 0;
            UpdateTimerText();
            playerScript.LoadSettings();
        }

       


    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused) {
            UpdateLevelTimer();
        }


        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if ((menuItemSelect && menuItemSelect.activeSelf) ||
        (menuItemUnlock && menuItemUnlock.activeSelf) ||
        (menuWin && menuWin.activeSelf) ||
        (menuLose && menuLose.activeSelf))
            {
                return; // do nothing
            }

            if (!isPaused) GameStatePause();
            else GameStateResume();
        }

        tutorialTimer += Time.deltaTime;

        if (SceneManager.GetActiveScene().name == "Level 1 [The Circuit]")
        {
            tutorialTimer += Time.deltaTime;

            if (tutorialTimer >= 15f) // every 15 seconds
            {
                
                ShowTutorial(tutorialMessages[currentTutorialIndex], 5f);

                currentTutorialIndex++;
                if (currentTutorialIndex >= tutorialMessages.Count)
                    currentTutorialIndex = 0; // loop back

                tutorialTimer = 0f;
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
            OptionPanel.transform.SetAsFirstSibling(); // Moves panel to back of UI
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
        // CHANGE THIS TO LEVEL HUB IF THERE IS SAVE DATA
        playerScript.LoadSettings();
        stats.ResetAllStats();
    }

    public void Option()
    {
        Debug.Log("Option() called");
        UIAudioSource.Play();
        OptionPanel.transform.SetAsLastSibling(); // Moves panel to front of UI
        OptionPanel.alpha = 1;               // fully visible
        OptionPanel.interactable = true;     // UI elements respond
        OptionPanel.blocksRaycasts = true;   // catches clicks
    }
    public void Back()
    {

        Debug.Log("Back() called");  // <-- You should see this in Console
        UIAudioSource.Play();
        OptionPanel.transform.SetAsFirstSibling(); // Moves panel to back of UI
        OptionPanel.alpha = 0;               // invisible
        OptionPanel.interactable = false;    // no interaction
        OptionPanel.blocksRaycasts = false;  // no clicks

    }
     
    public void QuitGame()
    {
        
            Debug.Log("Quit!"); // works in editor

        playerScript.SaveSettings();
        playerScript.playerItems.playeritems.Clear();
        UIAudioSource.Play();
        
        Application.Quit();


      #if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
      #else
        Application.Quit();
      #endif
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

        if (winSummaryText != null)
        {
            string timeResult = TimerMinutes.text + TimerSeconds.text + TimerMiliseconds.text;
            winSummaryText.text = "Clear Time: " + timeResult;


            //This will save data to local file in computer
            PlayerPrefs.SetInt("Level " + SceneManager.GetActiveScene().buildIndex + " Score", score); 
            PlayerPrefs.SetString("Level " + SceneManager.GetActiveScene().buildIndex + " Time", timeResult);
            PlayerPrefs.Save();
           // PlayerPrefs.

        }
    }

    public void YouLose(string cause = "Unknown")
    {
        isPaused = true;
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (menuWin) menuWin.SetActive(false);
        if (menuLose) menuLose.SetActive(true);
        menuActive = menuLose;

        playerScript.SaveSettings();

        if (loseSummaryText != null)
        {
            string timeResult = TimerMinutes.text + TimerSeconds.text + TimerMiliseconds.text;
            loseSummaryText.text = "Time Survived: " + timeResult;
        }

        if (deathReasonText != null)
        {
            if (cause == "Unknown")
            {
                deathReasonText.text = "Cause Of Death: Unknown";
            }
            else
            {
                deathReasonText.text = cause;
            }
        }
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
        else LoadLevel(0);
        playerScript.LoadSettings();// loop back to Title when out of levels
    }

    public void RestartLevel()
    {
        if (playerScript.playerItems.playeritems.Count >= SceneManager.GetActiveScene().buildIndex)
        {
            playerScript.playerItems.playeritems.RemoveAt(SceneManager.GetActiveScene().buildIndex - 1);
        }
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
                if (playerScript != null && playerScript.brokenClock && Random.Range(1,101) <= 10)
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
        if (playerScript == null)
        {
            return;
        }
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

    public void ShowTutorial(string msg, float seconds = 3f)
    {
        if (!tutorialPrompt) return;
        // don�t show over win/lose
        if ((menuWin && menuWin.activeSelf) || (menuLose && menuLose.activeSelf)) return;
        tutorialPrompt.Show(msg, seconds);
    }

    
}
