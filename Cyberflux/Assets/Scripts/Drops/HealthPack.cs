using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [SerializeField] int HealAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        if (other.CompareTag("PlayerModel") && GameManager.instance.playerScript.Heal(HealAmount))
        {
            Destroy(gameObject);
        }
        
    }
}
