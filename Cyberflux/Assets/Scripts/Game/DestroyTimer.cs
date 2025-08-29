using UnityEngine;

public class DestroyTimer : MonoBehaviour
{

    [SerializeField] int DestroyTime;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > DestroyTime)
        {
            if (gameObject.GetComponentInChildren<PlayerController>())
            {
                GameManager.instance.player.transform.parent = null;
            }
            Destroy(gameObject);
        }
    }
}
