using UnityEngine;

public class JumpPad : MonoBehaviour
{
   //
    [SerializeField] int launchStrength;
    [SerializeField] int cooldownTime;
    
    float cooldownTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer-= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("PlayerModel"))
        //{
        //    // GameManager.instance.player.GetComponent<Rigidbody>().AddForce(Vector3.up * launchStrength);
        //}
        if (other.transform.root.GetComponent<Rigidbody>() != null && cooldownTimer <= 0)
        {
            
            other.transform.root.GetComponent<Rigidbody>().AddForce((transform.forward) * launchStrength + other.transform.root.GetComponent<Rigidbody>().linearVelocity);
            cooldownTimer = cooldownTime;
        }
    }
//In case the player stays on the jump pad as it is on cooldown
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.root.GetComponent<Rigidbody>() != null && cooldownTimer <= 0)
        {

            other.transform.root.GetComponent<Rigidbody>().AddForce((transform.forward) * launchStrength + other.transform.root.GetComponent<Rigidbody>().linearVelocity);
            cooldownTimer = cooldownTime;
        }
    }

}
