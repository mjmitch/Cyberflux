using UnityEngine;

public class BossDoorTrigger : MonoBehaviour
{
    private GameObject door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        door = GameObject.FindWithTag("BossDoor");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            Debug.Log("DOOR");
            door.SetActive(true);
        }

        Destroy(gameObject);
    }
}
