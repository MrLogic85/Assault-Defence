using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public GameObject enemyPrefab;
    [Range (0, 100)] public float spawnDelay = 10;
    [Range (0, 100)] public float spawnDelayVariation = 10;

    private Enemy enemy;
    private float nextSpawnTime;

	// Use this for initialization
	void Start ()
    {
        nextSpawnTime = Random.Range(-spawnDelayVariation / 2f, spawnDelayVariation / 2f) + spawnDelay + Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (nextSpawnTime < Time.time && (enemy == null || enemy.Hitpoints <= 0))
        {
            GameObject obj = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
            enemy = obj.GetComponent<Enemy>();
            nextSpawnTime = Random.Range(-spawnDelayVariation / 2f, spawnDelayVariation / 2f) + spawnDelay + Time.time;
        }
	}
}
