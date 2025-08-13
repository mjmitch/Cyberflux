using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damagetype { moving, stationary, DOT, homing, explosion }

    bool isDamaging;

    bool isSlowing;

    [Header("Object Set-Up")]
    [SerializeField] damagetype type;
    [SerializeField] Rigidbody rb;
    [SerializeField] bool slowEffect;

    [Header("Object Manipulation")]
    [SerializeField] int damageAmount;
    [SerializeField] float damageRate;
    [SerializeField] int speed;
    [SerializeField] float destroyTime;
      

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Handles removing temporary objects and applying movement:
        if (type == damagetype.moving || type == damagetype.homing || type == damagetype.explosion)
        {
            Destroy(gameObject, destroyTime);

            if (type == damagetype.moving)
            {
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
            dmg.TakeDamage(damageAmount);
            //Handles slows:
            if(slowEffect == true && !isSlowing)
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
        d.TakeDamage(damageAmount);
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
