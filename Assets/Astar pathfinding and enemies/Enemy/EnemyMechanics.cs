using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMechanics : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    public float health;
    public Vector3 distanceToPlayer;
    public GameObject player;
    public GameObject target;

    private void Awake()
    {
        health = maxHealth;
        player = GameObject.Find("Player(Clone)");
        target = GameObject.Find("Target");
    }

    private void Update()
    {
        distanceToPlayer = player.transform.position - transform.position;

        if (distanceToPlayer.magnitude < 10f)
        {
            target.transform.position = player.transform.position;
        }
    }
}
