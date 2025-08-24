using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] GameObject model;
    [SerializeField] public Animator animator;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform attackPosition;
    [SerializeField] Transform headPosition;
    [SerializeField] GameObject bullet;
    private int bulletDamage;
    [SerializeField] private GameObject weapon;
    [SerializeField] int basedamage;
    [SerializeField] float attackRate;
    [SerializeField] AudioClip attackSound;
    [SerializeField] int HP;
    [SerializeField] int fov;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDis;
    [SerializeField] int roamPauseTime;
    [SerializeField] private bool slowImmune;
    [Range(0f, 1f)] [SerializeField] private float slowModifier;
    private bool isSlowed;
    [Header("Elite Enemy Stuff")]
    [SerializeField] private bool isEliteEnemy;
    //[Range(0.5f, 3f)] [SerializeField] private float moveCoverTime;
    //[SerializeField] private int attackDistance;
    [Range(1, 5)] [SerializeField] private int teleportTime;
    [Range(3, 10)] [SerializeField] private int maxTeleportDistance;
    [SerializeField] private AudioClip teleportSound;
    float teleportTimer;
    private bool initMovement = true;
    //float moveCoverTimer;
    [Header("How much score do you get from killing this enemy?")]
    [SerializeField] public int score;
    [Header("Exploding Enemy Stuff\nLeave blank if not Exploding enemy")]
    [Range(0, 3)] [SerializeField] private int explosionSize;
    [Range(5, 25)] [SerializeField] private int explosionDamage;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private AudioClip explosionSound;
    private bool isExploding = false;
    [Header("Flying Enemy Stuff\nLeave blank if not Flying enemy")]
    [Range(0, 5)] [SerializeField] private int minFlyHeight;
    [Range(2f, 9f)] [SerializeField] private float maxFlyHeight;
    //[Range(4, 15)] [SerializeField] private int attackRange;
    [Range(5, 25)] [SerializeField] private int circleRange;
    private bool isBobbing = false;
    private bool eliteAttackingPlayer = false;
    private bool dead = false;
    
    
    private enum enemyType
    {
        melee,
        ranged,
        exploding,
        swarm,
        flying
    }

    [Header("Enemy Type")]
    [SerializeField] private enemyType type;
    [Header("Sounds")]
    //[SerializeField] int dropChance;
    private GameObject player;
    private Vector3 playerDirection;
    private Vector3 attackPlayerDirection;
    AudioSource audioPlayer;
    
    float attackTimer;
    float angleToPlayer;
    float roamTime;
    float agentStopDisOrig;
    
    Vector3 startPos;
    bool playerInTrigger;
    private bool isInCombat;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int levelNum = SceneManager.GetActiveScene().buildIndex;
        startPos = transform.position;
        agentStopDisOrig = agent.stoppingDistance;
        player = GameObject.FindGameObjectWithTag("Player");
        audioPlayer = GetComponent<AudioSource>();
        if (type == enemyType.exploding && explosionEffect != null)
        {
            explosionDamage += levelNum - 1;
            explosionEffect.GetComponent<damage>().damageAmount = explosionDamage;
            explosionEffect.transform.localScale = new Vector3(explosionSize*2, explosionSize*2, explosionSize*2);
        }
        else if (type == enemyType.flying)
        {
            agent.baseOffset = Random.Range(minFlyHeight, maxFlyHeight);
            roamDis = circleRange;
        }
        agentStopDisOrig = agent.stoppingDistance;

        GameManager.instance.LoadSettings();

        audioPlayer.volume = GameManager.instance.masterVol * GameManager.instance.sfxVol;
        teleportTimer = 0;
        HP *= ((int)(0.05f * (levelNum)) + 1);
        attackRate *= (1 - (levelNum / 100f));
        if (bullet != null)
        {
            bulletDamage = (int)(basedamage * ((0.08f * SceneManager.GetActiveScene().buildIndex) + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerDirection = player.transform.position - headPosition.position;
        attackPlayerDirection = player.transform.position - attackPosition.position;
        
        if (isInCombat)
        {
            if (isEliteEnemy)
                EliteCombatMovement();
            else
                BasicCombatMovement();
            Combat();
        }
        else
        {
            if(agent.remainingDistance <= 0.01f)
                roamTime += Time.deltaTime;
            if (type == enemyType.swarm)
                CanSeePlayer();
            else if (!playerInTrigger || (playerInTrigger && CanSeePlayer()))
                RoamCheck();
        }

        if (type == enemyType.flying && !isBobbing && !dead)
            StartCoroutine(FlyingMovement());
        else if (type == enemyType.flying && dead)
        {
            agent.baseOffset -= Time.deltaTime/2;
            transform.Rotate(Time.deltaTime * 250, Time.deltaTime * 250, Time.deltaTime * 250);
        }

        if (animator != null)
        {
            animator.speed = agent.velocity.normalized.magnitude;
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
            if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("PlayerModel") && angleToPlayer < fov)
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
        audioPlayer.PlayOneShot(attackSound, GameManager.instance.masterVol * GameManager.instance.sfxVol);
    }

    void RangedAttack()
    {
        attackTimer = 0;
        GameObject bullet1 = Instantiate(bullet, attackPosition.position, transform.rotation);
        bullet1.GetComponent<damage>().damageAmount = bulletDamage;
        
            //bullet.GetComponent<damage>().damageAmount *= ((int)1.05f * (SceneManager.GetActiveScene().buildIndex));
        audioPlayer.PlayOneShot(attackSound, GameManager.instance.masterVol * GameManager.instance.sfxVol);
        //bullet1.GetComponent<damage>().rb.linearVelocity = (player.transform.position - transform.position) * bullet1.GetComponent<damage>().speed;
    }

    void FlyingAttack()
    {
        attackTimer = 0;
        RangedAttack();
    }

    void ExplodingAttack()
    {
        if (!isExploding)
        {
            isExploding = true;
            Transform explodePosition = model.transform;
            Instantiate(explosionEffect, explodePosition.position, Quaternion.identity);
            audioPlayer.PlayOneShot(explosionSound, GameManager.instance.masterVol);
        }
        Destroy(gameObject);
    }
    
    void BasicCombatMovement()
    {
        if (dead)
            return;
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
                //agent.speed = 0;
                //agent.acceleration = 0;
                //agent.isStopped = true;
                agent.destination = transform.position;
                break;
        }
    }

    void EliteCombatMovement()
    {
        if (dead)
            return;
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= teleportTime)
        {
            teleportTimer = 0;
            //Vector2 unitCircle = Random.insideUnitCircle;
            Vector3 offset = Random.insideUnitCircle.normalized * maxTeleportDistance;
            offset.z = offset.y;
            offset.y = 0;
            
            transform.position += offset;
            if(teleportSound != null)
                audioPlayer.PlayOneShot(teleportSound, GameManager.instance.masterVol * GameManager.instance.sfxVol);
        }
        BasicCombatMovement();
    }

    void Combat()
    {
        if (dead)
            return;
        attackTimer += Time.deltaTime;
        switch (type)
        {
            case enemyType.melee:
                if ((player.transform.position - transform.position).magnitude <= agent.stoppingDistance + 0.15f && attackTimer >= attackRate)
                    MeleeAttack();
                break;
            case enemyType.ranged:
                if (attackTimer >= attackRate)
                    RangedAttack();
                break;
            case enemyType.exploding:
                if ((player.transform.position - transform.position).magnitude < explosionSize)
                    ExplodingAttack();
                break;
            case enemyType.swarm:
                // Handled with collider
                break;
            case enemyType.flying:
                if (attackTimer >= attackRate)
                    FlyingAttack();
                break;
        }
    }

    IEnumerator FlyingMovement()
    {
        isBobbing = true;
        //transform.Rotate(0, 0, Random.Range(-15, 15));
        agent.baseOffset = Math.Clamp(agent.baseOffset + Random.Range(-0.1f, 0.1f), minFlyHeight, maxFlyHeight);
        yield return new WaitForSeconds(0.1f);
        isBobbing = false;
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
                audioPlayer.PlayOneShot(explosionSound, GameManager.instance.sfxVol * GameManager.instance.masterVol);
            }
            GameManager.instance.score += score;
            ScorePopUp pop = GameManager.instance.popUp;
            pop.SetText("+" + score.ToString());
            Instantiate(pop, transform.position, Quaternion.identity);
            
            Death();
        }
    }

    void Death()
    {
        dead = true;
        switch (type)
        {
            case enemyType.exploding:
                animator.Play("Destroyed");
                break;
            case enemyType.ranged:
                animator.Play("Die");
                break;
            case enemyType.swarm:
                Destroy(gameObject);
                break;
            case enemyType.melee:

                break;
            case enemyType.flying:
                animator.Play("Destroyed");
                break;
        }
        if(type != enemyType.swarm)
            StartCoroutine(RemoveGameObject());
    }

    IEnumerator RemoveGameObject()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
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
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.05f);
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.05f);
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.05f);
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.05f);
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.05f);
        weapon.transform.Rotate(15,0, 5);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.Rotate(-25,0, -5);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.Rotate(-25,0, -5);
        yield return new WaitForSeconds(0.1f);
        weapon.transform.localRotation = Quaternion.Euler(0,0,0);
    }
}