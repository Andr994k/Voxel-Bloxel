using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    private Vector3 spawnPoint;
    public float spawnTime = 600;
    public float spawnTimer;
    public float maxEnemyCount;
    public List<GameObject> enemies;

    private void Awake()
    {
        spawnTimer = 0;
    }

    private void Update()
    {
        player = GameObject.Find("Player(Clone)");
        spawnPoint = new Vector3(player.transform.position.x + Random.Range(-20, 20), 50, player.transform.position.z + Random.Range(-20, 20));
        spawnTimer++;

        if (spawnTimer >= spawnTime && enemies.Count < maxEnemyCount)
        {
            spawnTimer = 0;
            enemies.Add(Instantiate(enemyPrefab, spawnPoint, Quaternion.LookRotation(-player.transform.forward, Vector3.up)));
        }
    }




}
