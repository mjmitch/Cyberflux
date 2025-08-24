using TMPro;
using UnityEngine;

public class LevelHubScore : MonoBehaviour
{
    [SerializeField] int level;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        LoadLevelScore();
    }
    
    void LoadLevelScore()
    {
        TMP_Text LevelText = GetComponent<TMP_Text>();

        if (level == 1)
        LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 1 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 1 Time HighScore", "00.00.00"));

        if (level == 2)
            LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 2 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 2 Time HighScore", "00.00.00"));

        if (level == 3)
            LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 3 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 3 Time HighScore", "00.00.00"));

        if (level == 4)
            LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 4 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 4 Time HighScore", "00.00.00"));

        if (level == 5)
            LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 5 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 5 Time HighScore", "00.00.00"));

        if (level == 6)
            LevelText.text = "Best Score: " + PlayerPrefs.GetInt("Level 6 HighScore", 0) + "\n" + "Best Time: " + (PlayerPrefs.GetString("Level 6 Time HighScore", "00.00.00"));
    }
   
}
