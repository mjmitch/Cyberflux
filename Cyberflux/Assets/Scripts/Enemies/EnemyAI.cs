using System;
using System.Collections;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform attackPosition;
    [SerializeField] Transform headPosition;
    [SerializeField] GameObject bullet;
    [SerializeField] private GameObject weapon;
    [SerializeField] float attackRate;
    [SerializeField] int HP;
    [SerializeField] int fov;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDis;
    [SerializeField] int roamPauseTime;
    [SerializeField] private bool slowImmune;
    [Range(0f, 1f)] [SerializeField] private float slowModifier;
    private bool isSlowed;
    [SerializeField] private bool isEliteEnemy;
    [Range(0.5f, 3f)] [SerializeField] private float moveCoverTime;
    float moveCoverTimer;

    [Range(0, 3)] [SerializeField] private int explosionSize;
    [Range(5, 25)] [SerializeField] private int explosionDamage;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    private bool isExploding = false;
    [SerializeField] public int score;
    
    
    private enum enemyType
    {
        melee,
        ranged,
        exploding,
        swarm,
        flying
    }

    [SerializeField] private enemyType type;
    
    [SerializeField] int dropChance;
    private GameObject player;
    private Vector3 playerDirection;
    private AudioSource audioPlayer;
    
    float attackTimer;
    float angleToPlayer;
    float roamTime;
    float agentStopDisOrig;
    
    Vector3 startPos;
    bool playerInTrigger;
    private bool isInCombat;
    private bool hasFoundCover;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        agentStopDisOrig = agent.stoppingDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        audioPlayer = GetComponent<AudioSource>();
        if (type == enemyType.exploding && explosionEffect != null)
        {
            explosionEffect.GetComponent<damage>().damageAmount = explosionDamage;
            explosionEffect.transform.localScale = new Vector3(explosionSize*2, explosionSize*2, explosionSize*2);
        }
        agentStopDisOrig = agent.stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = player.transform.position - headPosition.position;
        if (type != enemyType.flying && !isInCombat)
        {
            if(agent.remainingDistance <= 0.01f)
                roamTime += Time.deltaTime;
            if (!playerInTrigger || (playerInTrigger && CanSeePlayer()))
                RoamCheck();
        }
        if (isInCombat)
        {
            if (isEliteEnemy)
                EliteCombatMovement();
            else
                BasicCombatMovement();
            Combat();
        }
    }

    void RoamCheck()
    {
        if (roamTime >= roamPauseTime && agent.remainingDistance <= 0.01f)
            Roam();
    }

    void Roam()
    {
        roamTime = 0;
        agent.stoppingDistance = 0;
        Vector3 ranPos = Random.insideUnitSphere * roamDis;
        ranPos += startPos;
        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDis, 1);
        agent.SetDestination(hit.position);
    }

    bool CanSeePlayer()
    {
        if (isInCombat)
            return true;
        playerDirection = player.transform.position - headPosition.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        RaycastHit hit;
        //Debug.Log("CAN SEE PLAYER");
        Debug.DrawRay(headPosition.position, playerDirection, Color.red);
        if (Physics.Raycast(headPosition.position, playerDirection, out hit))
        {
            //Debug.Log(hit.collider.name + " / " + angleToPlayer + " / " + fov);
            if (hit.collider.CompareTag("Player") && angleToPlayer < fov)
            {
                //Debug.Log("CAN SEE PLAYER -- COMBAT");
                isInCombat = true;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void FaceTarget()
    {
        transform.LookAt(player.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            agent.stoppingDistance = 0;
        }
    }

    void MeleeAttack()
    {
        attackTimer = 0;
        StartCoroutine(SwordSwing());
    }

    void RangedAttack()
    {
        attackTimer = 0;
        Instantiate(bullet, attackPosition.position, transform.rotation);
    }

    void FlyingAttack()
    {
        
    }

    void ExplodingAttack()
    {
        if (!isExploding)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            audioPlayer.PlayOneShot(explosionSound, GameManager.instance.playerScript.sfxVol);
        }
        isExploding = true;
        Destroy(gameObject);
    }

    /*
     void SwarmAttack()
     
    {
        
    }
    */
    
    void BasicCombatMovement()
    {
        FaceTarget();
        
        switch (type)
        {
            case enemyType.melee:
                agent.stoppingDistance = agentStopDisOrig;
                agent.destination = player.transform.position;
                break;
            case enemyType.ranged:
                agent.stoppingDistance = 5000;
                break;
            case enemyType.exploding:
                agent.stoppingDistance = 0;
                agent.destination = player.transform.position;
                break;
            case enemyType.swarm:
                agent.stoppingDistance = 0;
                agent.destination = player.transform.position;
                break;
            case enemyType.flying:
                FlyingMovement();
                break;
        }
    }

    void EliteCombatMovement()
    {
        switch (type)
        {
            case enemyType.melee:
                agent.stoppingDistance = 5000;
                FindNextClosestCover();
                break;
            case enemyType.ranged:
                agent.stoppingDistance = 5000;
                FindCover();
                break;
            case enemyType.exploding:
                agent.stoppingDistance = 5000;
                FindNextClosestCover();
                break;
            case enemyType.swarm:
                agent.stoppingDistance = 0;
                break;
            case enemyType.flying:
                FlyingMovement();
                break;
        }
    }

    void Combat()
    {
        attackTimer += Time.deltaTime;
        if (isEliteEnemy)
        {
            switch (type)
            {
                case enemyType.melee:
                    break;
                case enemyType.ranged:
                    break;
                case enemyType.exploding:
                    if((player.transform.position - this.transform.position).magnitude < explosionSize)
                        ExplodingAttack();
                    break;
                case enemyType.swarm:
                    break;
                case enemyType.flying:
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case enemyType.melee:
                    //Debug.Log("melee");
                    if ((player.transform.position - this.transform.position).magnitude <= agent.stoppingDistance+0.5f && attackTimer >= attackRate)
                        MeleeAttack();
                    break;
                case enemyType.ranged:
                    if (attackTimer >= attackRate)
                        RangedAttack();
                    break;
                case enemyType.exploding:
                    if((player.transform.position - this.transform.position).magnitude < explosionSize)
                        ExplodingAttack();
                    break;
                case enemyType.swarm:
                    
                    break;
                case enemyType.flying:
                    break;
            }
        }
    }

    void FindCover()
    {
        
    }

    void FindNextClosestCover()
    {
        
    }

    void FlyingMovement()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            if (type == enemyType.exploding && !isExploding)
            {
                explosionEffect.GetComponent<damage>().damageAmount /= 2;
                explosionEffect.transform.localScale /= 2;
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
                audioPlayer.PlayOneShot(explosionSound, GameManager.instance.playerScript.sfxVol);
            }
            
            Destroy(gameObject);
        }
    }

    public int GetHP()
    {
        return HP;
    }

    public void TakeSlow()
    {
        isSlowed = true;
    }

    public void RemoveSlow()
    {
        isSlowed = false;
    }

    public IEnumerator SwordSwing()
    {
        weapon.transform.Rotate(45,0,playerDirection.z);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.Rotate(45,0, playerDirection.z);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.Rotate(-45,0, -playerDirection.z);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.localRotation = Quaternion.Euler(0,0,0);
    }
}