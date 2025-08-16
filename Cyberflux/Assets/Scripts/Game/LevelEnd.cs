using UnityEngine;

public class LevelEnd : MonoBehaviour
{

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.YouWin();
        }
    }
}
