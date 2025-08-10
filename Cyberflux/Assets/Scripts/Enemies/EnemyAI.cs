using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;
    [SerializeField] int HP;
    [SerializeField] int fov;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int roamDis;
    [SerializeField] int roamPauseTime;
    
    [SerializeField] int dropChance;
    private GameObject player;
    private Vector3 playerDirection;
    private AudioSource audioPlayer;
    
    float shootTimer;
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
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance <= 0.01f)
            roamTime += Time.deltaTime;
        if (!playerInTrigger || (playerInTrigger && CanSeePlayer()))
            RoamCheck();

        if (isInCombat)
            Combat();
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
        playerDirection = player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDirection, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDirection, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer < fov)
            {
                isInCombat = true;
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void FaceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDirection.x, transform.position.y, playerDirection.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
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

    void Attack()
    {
        
    }

    void CombatMovement()
    {
        
    }

    void Combat()
    {


        Attack();
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;
    }

    public int GetHP()
    {
        return HP;
    }
}
