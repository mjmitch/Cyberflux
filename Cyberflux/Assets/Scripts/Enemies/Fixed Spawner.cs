using UnityEngine;

public class FixedSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectSpawn;
    [Range(1, 8)] [SerializeField] int spawnAmount;
    [Range(0, 2f)] [SerializeField] float spawnRate;
    [SerializeField] Transform[] spawnPos;

    float spawnTimer;
    int spawnCount;
    bool startSpawning;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (startSpawning)
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


    void spawn()
    {
        int arrayPos = Random.Range(0, spawnPos.Length);
        Instantiate(objectSpawn, spawnPos[arrayPos].transform.position, spawnPos[arrayPos].transform.rotation);
        spawnCount++;
        spawnTimer = 0;
    }
}