using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ScytheCombat : MonoBehaviour, IDamage
{

    //Events
    public event Action OnSlash;

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
    [SerializeField] ScytheBar scytheBar; 
    [SerializeField] Transform playerCam;
    [SerializeField] Transform orientation;
    [SerializeField] GameObject scytheProjectile;
    [SerializeField] int slashProjectileCharges;
    public  float slashRechargeTime;
    
    public float currentSlashTime;
    public float nextSlashTime;

    

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
                //Attacks per second Method
                nextAttackTime = Time.time + 1f / attackRate;

            }    
        }

       

        if (Time.time >= nextSlashTime) {

            if (Input.GetKey(slashKey))
            {
                SlashAttack();
                //Seconds until next attack
                nextSlashTime = Time.time + slashRechargeTime;
                //The "?" is just like having an if check to see if the parameter is null || SUPER USEFUL
                OnSlash?.Invoke();
                
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

    public void SlashAttack()
    {
        
        if (slashProjectileCharges >= 0)
        {
            GameObject projectile = Instantiate(scytheProjectile, attackPoint.position, orientation.rotation);
            //Making sure the projectile goes where the player is facing rather than straight
            projectile.transform.forward = playerCam.forward;
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
