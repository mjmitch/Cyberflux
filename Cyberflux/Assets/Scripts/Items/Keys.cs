using UnityEngine;

public class Keys : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            GameManager.instance.playerScript.keys++;
            GameManager.instance.keyImage.gameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
