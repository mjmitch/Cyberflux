using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    
    public GameObject player;

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


    enum GameState { Title, Playing, Win, Lose }
    GameState _state = GameState.Title;

    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");

        ShowTitle();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStatePause()
    {
        
    }

    public void GameStateResume()
    {
        
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


}
