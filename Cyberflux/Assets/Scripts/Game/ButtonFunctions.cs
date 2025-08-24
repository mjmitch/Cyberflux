using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
   

    public void StartGame()
    {
        GameManager.instance.StartGame();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    public void GoToCredits()
    {
       
        GameManager.instance.LoadLevel(9);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void SelectItem1()
    {

    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        GameManager.instance.isPaused = false;
        Time.timeScale = 1f;

        GameManager.instance.LoadLevel(1);
    }
}
