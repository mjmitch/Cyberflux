using UnityEngine;

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
}
