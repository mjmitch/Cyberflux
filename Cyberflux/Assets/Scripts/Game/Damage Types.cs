using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damagetype { moving, stationary, DOT, homing, explosion }

    bool isDamaging;

    bool isSlowing;

    [Header("Object Set-Up")]
    [SerializeField] damagetype type;
    [SerializeField] public Rigidbody rb;
    [SerializeField] bool slowEffect;
    private GameObject player;

    [Header("Object Manipulation")]
    [SerializeField] public int damageAmount;
    [SerializeField] public float damageRate;
    [SerializeField] public int speed;
    [SerializeField] public float destroyTime;
    bool exploded = false;
    [SerializeField] bool enemyBullet;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Handles removing temporary objects and applying movement:
        if (type == damagetype.moving || type == damagetype.homing || type == damagetype.explosion)
        {
            Destroy(gameObject, destroyTime);

            if (type == damagetype.moving)
            {
                if(enemyBullet)
                    rb.linearVelocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed;
                else
                    rb.linearVelocity = transform.forward * speed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Tracks players position:
        if (type == damagetype.homing)
        {
            rb.linearVelocity = (GameManager.instance.player.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    //Deals damage for non-DOT types:
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type != damagetype.DOT)
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Try to get a DeathCause message
                string cause = GetComponentInParent<DeathCause>()?.GetMessage() ?? "Unknown";
             
                player.TakeDamage(damageAmount, cause);
            }
            else
            {
                dmg.TakeDamage(damageAmount);
            }

            if (slowEffect && !isSlowing)
            {
                StartCoroutine(slowOther(dmg));
            }
        }

        if (type == damagetype.moving || type == damagetype.homing)
        {
            Destroy(gameObject);
        }
    }

    //Deals damage for DOT
    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null && type == damagetype.DOT && !isDamaging)
        {
            StartCoroutine(damageOther(dmg));
            //Handles slows:
            if (slowEffect == true && !isSlowing)
            {
                StartCoroutine(slowOther(dmg));
            }
        }
    }

    //DOT coroutine:
    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;

        // If the thing we are damaging is the player, send cause-of-death string
        PlayerController player = d as PlayerController;
        if (player != null)
        {
            string cause = GetComponentInParent<DeathCause>()?.GetMessage() ?? "Unknown";
            player.TakeDamage(damageAmount, cause);
        }
        else
        {
            // Fallback for anything else that takes damage
            d.TakeDamage(damageAmount);
        }

        yield return new WaitForSeconds(damageRate);
        isDamaging = false;
    }

    //Slow effect coroutine:
    IEnumerator slowOther(IDamage d)
    {
        isSlowing = true;
        d.TakeSlow();
        yield return new WaitForSeconds(damageRate);
        d.RemoveSlow();
        isSlowing = false;
    }

    
}