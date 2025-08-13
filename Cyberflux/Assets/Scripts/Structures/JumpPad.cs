using System.Collections;
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
        if (cooldownTimer <= 0 && gameObject.layer == 11)
        {
            gameObject.layer = 13;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("PlayerModel") && cooldownTimer <= 0)
        {
            
            
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().AddForce(transform.forward * launchStrength, ForceMode.Force);
            cooldownTimer = cooldownTime;

            StartCoroutine(JumpDelay());
        }





   
    }
//In case the player stays on the jump pad as it is on cooldown
    private void OnTriggerStay(Collider other)
    {

        if (other.isTrigger)
        {
            return;
        }

        if (other.CompareTag("PlayerModel") && cooldownTimer <= 0)
        {
            
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GameObject.FindWithTag("Player").GetComponent<Rigidbody>().AddForce(transform.forward * launchStrength, ForceMode.Force);
            cooldownTimer = cooldownTime;
            StartCoroutine(JumpDelay());
        }






    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.layer = 11;
    }
}
