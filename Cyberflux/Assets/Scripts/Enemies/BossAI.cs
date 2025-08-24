using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class BossAI : MonoBehaviour, IDamage
{
    [SerializeField] private string[] bossNames;
    private string bossName;
    [SerializeField] private GameObject bossModel;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform[] attackPositions;
    [SerializeField] private Transform headPosition;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private float[] shootRates;
    private float[] shootTimers;
    [SerializeField] private int bossHP;
    [SerializeField] private int score;
    private int maxHP;
    private int phaseNum;
    [Range(5, 20)] [SerializeField] private float attackDistance;
    [SerializeField] private GameObject[] enemiesToSpawn;
    [SerializeField] private float spawnRate;
    private float spawnTimer;
    [SerializeField] private int auraDamageNum;
    [SerializeField] private float auraDamageRate;
    private float auraDamageTimer;
    private GameObject player;
    
    Transform playerPosition;
    private AudioSource audioPlayer;
    [SerializeField] private AudioClip[] shootingSounds;
    [SerializeField] private AudioClip[] musicPerPhase;
    [Range(1, 2f)] [SerializeField] private float fadeTime;
    [SerializeField] private AudioClip teleportSound;
    [Range(2, 10)] [SerializeField] private float teleportRate;
    [Range(4, 16)] [SerializeField] private float maxTeleportDistance;
    private float teleportTimer;
    private string deathCause;

    private bool playerInTrigger = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerPosition = GameManager.instance.player.transform;
        player = GameManager.instance.player;
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.volume = GameManager.instance.masterVol;
        maxHP = bossHP;
        phaseNum = 1;
        audioPlayer.clip = musicPerPhase[0];
        audioPlayer.Play();
        audioPlayer.loop = true;
        //agent.SetDestination(Vector3.Lerp(transform.position, playerPosition.position, 0.5f));
        teleportTimer = 0;
        shootTimers = new float[2];
        agent.stoppingDistance = 0;
        GameManager.instance.bossHPUI.SetActive(true);
        bossName = bossNames[Random.Range(0, bossNames.Length)];
        GameManager.instance.bossNameText.text = bossName;
        UpdateBossUI();
        spawnTimer = 0;
        deathCause = GetComponentInParent<DeathCause>()?.GetMessage() ?? "Unknown";
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
                player.GetComponent<PlayerController>().TakeDamage(auraDamageNum, deathCause);
            }
        }
        Movement();
        Combat();
        if (phaseNum == 2)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                spawnTimer = 0;
                Vector2 direction = player.transform.position - transform.position;
                direction.Normalize();
                direction *= 10;
                Vector3 spawnPosition = player.transform.position;
                spawnPosition.x += direction.x;
                spawnPosition.z += direction.y;
                GameObject enemy = Instantiate(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Length)], spawnPosition, Quaternion.identity);
                enemy.transform.LookAt(player.transform);
            }
        }
    }

    public void Movement()
    {
        transform.LookAt(new Vector3(playerPosition.position.x, 2, playerPosition.position.z));
        float distanceToPlayer = (player.transform.position - transform.position).magnitude;
        //Debug.Log(distanceToPlayer + " / " + agent.stoppingDistance);
        if (distanceToPlayer <= attackDistance)
        {
            Vector2 direction = transform.position - playerPosition.position;
            direction.Normalize();
            Vector3 newPosition = transform.position;
            newPosition.x += direction.x;
            newPosition.z += direction.y;
            //Debug.Log(transform.position + "/"+ newPosition);
            agent.SetDestination(newPosition);
        }
        else
        {
            agent.SetDestination(playerPosition.position);
        }

        if (phaseNum == 2)
            Teleport();
    }

    public void Combat()
    {
        //Debug.Log(shootTimers.Length);
        for(int i = 0; i < shootTimers.Length; i++)
        {
            //Debug.Log(i);
            shootTimers[i] += Time.deltaTime;
            if (shootTimers[i] > shootRates[i])
            {
               // Debug.Log(i);
                shootTimers[i] = 0;
                Instantiate(bullets[i], attackPositions[i].position, Quaternion.identity);
                audioPlayer.PlayOneShot(shootingSounds[i], GameManager.instance.masterVol * GameManager.instance.sfxVol);
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        //throw new System.NotImplementedException();
        bossHP -= dmg;
        UpdateBossUI();
        
        if (bossHP <= 0)
        {
            score += GameManager.instance.playerScript.GetHP();
            GameManager.instance.score += score;
            ScorePopUp pop = GameManager.instance.popUp;
            pop.SetText("+" + score.ToString());
            Instantiate(pop, transform.position, Quaternion.identity);
            GameManager.instance.YouWin();
        }

        if (bossHP <= (maxHP / 2) && phaseNum == 1)
        {
            phaseNum = 2;
            auraDamageRate *= 0.75f;
            auraDamageNum += 1;
            maxHP *= 2;
            bossHP *= 2;
            for (int i = 0; i < shootRates.Length; i++)
            {
                shootRates[i] *= 0.75f;
                //bullets[i].GetComponent<damage>().damageAmount += 2;
            }
            StartCoroutine(PhaseMusic());
            
        }
    }

    void Teleport()
    {
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= teleportRate)
        {
            teleportTimer = 0;
            //Vector2 unitCircle = Random.insideUnitCircle;
            Vector3 offset = Random.insideUnitCircle.normalized * maxTeleportDistance;
            offset.z = offset.y;
            offset.y = 0;
           // Debug.Log(offset);
            transform.position += offset;
            if(teleportSound != null)
                audioPlayer.PlayOneShot(teleportSound, GameManager.instance.masterVol * GameManager.instance.sfxVol);
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
        return bossHP;
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
        float startVolume = audioPlayer.volume;
        
        while (audioPlayer.volume > 0)
        {
            audioPlayer.volume -= startVolume * Time.deltaTime / fadeTime;
        }

        audioPlayer.clip = musicPerPhase[1];
        audioPlayer.Play();
        while (audioPlayer.volume < startVolume)
        {
            audioPlayer.volume += startVolume * Time.deltaTime / fadeTime;
        }
        yield return null;
    }

    void UpdateBossUI()
    {
        GameManager.instance.bossHPBar.fillAmount = (float) bossHP / maxHP;
    }

}
