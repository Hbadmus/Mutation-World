using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public GameObject boss;
    public float spawnTime1 = 3;
    public float spawnTime2 = 60;
    public int bossSpawnTime = 120;
    public float xMin = -25;
    public float xMax = 25;
    public float y = 0;
    public float zMin = -25;
    public float zMax = 25;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnEnemy1", spawnTime1, 7);
        InvokeRepeating("SpawnEnemy2", spawnTime2, 7);
        Invoke("SpawnBoss", bossSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

void SpawnEnemy1()
{
    for (int i = 0; i < 3; i++)
    {
        Vector3 enemyPosition;

        // Generate random positions relative to the parent object
        enemyPosition.x = transform.position.x + Random.Range(xMin, xMax);
        enemyPosition.y = 0;
        enemyPosition.z = transform.position.z + Random.Range(zMin, zMax);

        GameObject spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
        spawnedEnemy.transform.parent = transform;
    }
}

void SpawnEnemy2()
{
    for (int i = 0; i < 5; i++)
    {
        Vector3 enemyPosition;

        // Generate random positions relative to the parent object
        enemyPosition.x = transform.position.x + Random.Range(xMin, xMax);
        enemyPosition.y = 0f;
        enemyPosition.z = transform.position.z + Random.Range(zMin, zMax);

        GameObject spawnedEnemy = Instantiate(enemyPrefab2, enemyPosition, Quaternion.identity);
        spawnedEnemy.transform.parent = transform;
    }
}

void SpawnBoss()
{
    Transform bossTransform = transform.Find("BossZombie");
    
    if (bossTransform != null)
    {
        bossTransform.gameObject.SetActive(true);
    }
}


}
