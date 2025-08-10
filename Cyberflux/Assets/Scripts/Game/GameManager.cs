using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        

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

    public void YouLose()
    {
        
    }

    public void YouWin()
    {
        
    }

    public void UpdateUI()
    {
        
    }

    public void LoadLevel(int lvl)
    {
        
    }
}
