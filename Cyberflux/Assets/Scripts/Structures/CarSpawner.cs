using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] float spawnIntervalMin;
    [SerializeField] float spawnIntervalMax;
    [SerializeField] GameObject car;
    float spawnTime;
    float spawnInterval;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax + 1);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        if (spawnTime > spawnInterval)
        {
            
            Instantiate(car, transform.position, Quaternion.LookRotation(transform.forward,transform.up));
            
            spawnTime = 0;
            spawnInterval = Random.Range(spawnIntervalMin, spawnIntervalMax + 1);
        }
    }
}
