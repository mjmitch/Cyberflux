using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class FixedSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] objectSpawn;
    [Range(1, 8)] [SerializeField] int spawnAmount;
    [Range(0, 2f)] [SerializeField] float spawnRate;
    [SerializeField] Transform[] spawnPos;
    private bool fixedSpawner;
    [Header("Set to 0 for a Fixed Spawner where you choose your spawn points manually.\nOtherwise set to a max distance from which enemies can spawn at random")]
    [Range(0, 10)] [SerializeField] private int randomSpawnDistance;
    
    GameObject player;

    float spawnTimer;
    int spawnCount;
    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fixedSpawner = randomSpawnDistance == 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning && objectSpawn != null)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate && spawnCount < spawnAmount)
            { 
                spawn();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    Vector3 GetRandomSpawnPoint()
    {
        Vector3 ranPos = Random.insideUnitSphere * randomSpawnDistance;
        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, randomSpawnDistance, NavMesh.AllAreas);
        return hit.position;
    }

    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        if(fixedSpawner)
            Instantiate(objectSpawn[0], spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);
        else
        {
            int spawnObjectIndex = Random.Range(0, objectSpawn.Length);
            Instantiate(objectSpawn[spawnObjectIndex], GetRandomSpawnPoint(), Quaternion.Euler(player.transform.position - transform.position));
        }
        spawnCount++;
        spawnTimer = 0;
    }

    public bool IsDoneSpawning()
    {
        if (spawnAmount >= spawnCount)
        {
            return true;
        }
        return false;
    }
}