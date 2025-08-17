using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour, IDamage
{
    [SerializeField] private string[] bossNames;
    [SerializeField] private GameObject bossModel;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private Transform headPosition;
    [SerializeField] GameObject[] bullets;
    [SerializeField] private float[] shootRates;
    private float[] shootTimers;
    [SerializeField] private int HP;
    [SerializeField] private int score;
    private int maxHP;
    private int phaseNum;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private float spawnRate;
    [SerializeField] private int auraDamageNum;
    [SerializeField] private float auraDamageRate;
    private float auraDamageTimer;
    private GameObject player;
    
    Transform playerPosition;
    private AudioSource audioPlayer;
    [SerializeField] private AudioClip[] shootingSounds;
    [SerializeField] private AudioClip[] musicPerPhase;

    [Range(2, 10)] [SerializeField] private float teleportRate;
    private float teleportTimer;

    private bool playerInTrigger = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPosition = GameManager.instance.player.transform;
        player = GameManager.instance.player;
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.volume = GameManager.instance.playerScript.masterVol;
        maxHP = HP;
        phaseNum = 1;
        audioPlayer.clip = musicPerPhase[0];
        audioPlayer.Play();
        audioPlayer.loop = true;
        agent.SetDestination(Vector3.Lerp(transform.position, playerPosition.position, 0.5f));
        teleportTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            auraDamageTimer += Time.deltaTime;
            if (auraDamageTimer >= auraDamageRate)
            {
                auraDamageTimer = 0;
                player.GetComponent<PlayerController>().TakeDamage(auraDamageNum);
            }
        }
        
        
    }

    public void Movement()
    {
        
    }

    public void Combat()
    {
        for(int i = 0; i < shootTimers.Length; i++)
        {
            shootTimers[i] += Time.deltaTime;
            if (shootTimers[i] > shootRates[i])
            {
                Instantiate(bullets[i], attackPositions[i].position, Quaternion.identity);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        //throw new System.NotImplementedException();
        HP -= dmg;
        if (HP <= 0)
        {
            score += GameManager.instance.playerScript.GetHP();
            GameManager.instance.YouWin();
        }

        if (HP <= (maxHP / 2) && phaseNum == 1)
        {
            phaseNum = 2;
            auraDamageRate *= 0.75f;
            auraDamageNum += 1;
            maxHP *= 2;
            HP *= 2;
            for (int i = 0; i < shootRates.Length; i++)
            {
                shootRates[i] *= 0.75f;
                bullets[i].GetComponent<damage>().damageAmount += 2;
            }
            StartCoroutine(PhaseMusic());
        }
    }

    public void TakeSlow()
    {
        teleportRate += 2;
        StartCoroutine(AutoRemoveSlow());
    }

    IEnumerator AutoRemoveSlow()
    {
        yield return new WaitForSeconds(2f);
        RemoveSlow();
    }
    
    public void RemoveSlow()
    {
        teleportRate -= 2;        
    }

    public int GetHP()
    {
        //throw new System.NotImplementedException();
        return HP;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTrigger = false;
    }

    IEnumerator PhaseMusic()
    {
        while (audioPlayer.volume >= 0)
        {
            audioPlayer.volume -= 0.25f;
            yield return new WaitForSeconds(0.01f);
        }
        audioPlayer.Stop();
        audioPlayer.clip = musicPerPhase[1];
        audioPlayer.Play();
        while (audioPlayer.volume <
               GameManager.instance.playerScript.masterVol * GameManager.instance.playerScript.musicVol)
        {
            audioPlayer.volume += 0.25f;
            yield return new WaitForSeconds(0.01f);
        }

    }
}
