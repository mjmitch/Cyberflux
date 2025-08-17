using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel1 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            GameManager.instance.playerScript.playerItems.playeritems.Clear();
            SceneManager.LoadScene(1);
        }
    }
}
