using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{

    public Vector3 targetLocation = Vector3.zero;

    private void Start()
    {
        targetLocation = new Vector3(Random.Range(transform.position.x - 20f, transform.position.x + 20f),
                                     0,
                                     Random.Range(transform.position.z - 20f, transform.position.z + 20f));
    }
}
