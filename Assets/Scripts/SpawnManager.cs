using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float spawnRange = 9.0f;
    public int enemyCount;
    public int powerupCount;
    public int waveNumber = 1;
    public GameObject powerupPrefab;
    public GameObject wallsPrefab;
    void Start()
    {
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        SpawnEnemyWave(waveNumber);
        StartCoroutine(PowerupPeriodic());
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;  //Length of find object enemy array is count
        //enemyCount will become 0 when they have all fallen off stage or the beginning wave 1
        //start next wave do things
        if(enemyCount == 0)
        {
            int powerupRange = Random.Range(1, 3);
            if (powerupRange == 1)
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            }
            if (powerupRange > 1)
            {
                Instantiate(wallsPrefab, GenerateSpawnPosition(), wallsPrefab.transform.rotation);
            }
            //spawn next wave
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            //reset random range
            powerupRange = 0;
        }
        
    }
    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
    void SpawnEnemyWave(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
    //Loop to spawn a random power up every 20 seconds called by start
    IEnumerator PowerupPeriodic()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(20);
            int powerupRange = Random.Range(1, 3);
            if (powerupRange == 1)
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            }
            if (powerupRange > 1)
            {
                Instantiate(wallsPrefab, GenerateSpawnPosition(), wallsPrefab.transform.rotation);
            }
        }
    }
}
