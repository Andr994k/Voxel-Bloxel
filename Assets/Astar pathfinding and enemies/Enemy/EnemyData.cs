using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float health;
    public float[] position;

    public EnemyData(EnemyMechanics enemyMechanics)
    {
        health = enemyMechanics.health;

        position = new float[3];
        position[0] = enemyMechanics.transform.position.x;
        position[1] = enemyMechanics.transform.position.y;
        position[2] = enemyMechanics.transform.position.z;
    }



}
