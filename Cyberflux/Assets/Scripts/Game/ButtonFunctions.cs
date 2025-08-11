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
}
