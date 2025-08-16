using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Door door;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerModel"))
        {
            door.playerInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("PlayerModel"))
        {
            door.playerInTrigger = false;
        }
    }
}
