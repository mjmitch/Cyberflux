using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScytheCombat : MonoBehaviour
{

    [Header("Scythe")]
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRadius;
    [SerializeField] float attackRate;
    [SerializeField] int attackDamage;
    [SerializeField] GameObject scytheModel;

    private float nextAttackTime;
    //Grabbing all of the enemies Hit
     

    [Header("Input")]
    //Left Mouse Button
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;

    


    private void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if(Input.GetKey(attackKey)) 
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }    
        }
    }


    void Attack()
    {
        //Attack Animation || Beta Task

        //Detect Enemies in Range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);
        
        //Conflict Damage
        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<IDamage>().TakeDamage(attackDamage);
        }


    }


    //For Debugging and testing 
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

}
