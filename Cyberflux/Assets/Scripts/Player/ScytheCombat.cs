using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ScytheCombat : MonoBehaviour, IDamage
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

    [Header("Slash Projectile Attack")]
    [SerializeField] Transform orientation;
    [SerializeField] GameObject scytheProjectile;
    [SerializeField] int slashProjectileCharges;
    //Low number = slower || High Number = Faster fire rate
    [SerializeField] float slashRechargeRate;
    
    private float slashCount;
    private float nextSlashTime;

    [Header("Input")]
    //Left Mouse Button
    [SerializeField] KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] KeyCode slashKey = KeyCode.Q;

    


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

        if (Time.time >= nextSlashTime) {

            if (Input.GetKey(slashKey))
            {
                SlashAttack();

                nextSlashTime = Time.time + 1f / slashRechargeRate;

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

    void SlashAttack()
    {

        slashCount++;
        
        if (slashProjectileCharges >= 0)
        {
            Instantiate(scytheProjectile, attackPoint.position, orientation.rotation);
        }
    }

    //For Debugging and testing 
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    public void TakeDamage(int dmg)
    {
        throw new System.NotImplementedException();
    }

    public void TakeSlow()
    {
        throw new System.NotImplementedException();
    }

    public void RemoveSlow()
    {
        throw new System.NotImplementedException();
    }

    public int GetHP()
    {
        throw new System.NotImplementedException();
    }
}
