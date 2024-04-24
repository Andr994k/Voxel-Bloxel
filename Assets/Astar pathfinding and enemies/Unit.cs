using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Unity.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    public GameObject target;
    public float speed = 3;
    Vector3[] path;
    int targetIndex;
    Rigidbody rb;

    private void Awake()
    {
        target = GameObject.Find("Target");
    }

    private void Update()
    {
        PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
    }

    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, target.transform.position, OnPathFound);
        rb = GetComponent<Rigidbody>();
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position.x == currentWaypoint.x && transform.position.y == currentWaypoint.y)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            Debug.DrawRay(transform.position + (Vector3.down * 0.5f), transform.forward * 1f, Color.cyan);
            Debug.DrawRay(transform.position, transform.up * -1.5f, Color.magenta);
            if (Physics.Raycast(transform.position + (Vector3.down * 0.5f), transform.forward, 1f) && Physics.Raycast(transform.position, transform.up * -1f, 1.5f))
            {
                transform.position += Vector3.up;
                //transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
                //currentWaypoint = new Vector3(currentWaypoint.x, currentWaypoint.y + 1f, currentWaypoint.z);
            }
            
            currentWaypoint.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(currentWaypoint - transform.position, Vector3.up);
            rb.AddForce(Vector3.down * 0.1f, ForceMode.Force);

            /*if (!Physics.Raycast(transform.position, transform.up * -1f, 1f))
            {
                //transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                currentWaypoint = new Vector3(currentWaypoint.x, currentWaypoint.y - 1f, currentWaypoint.z);
            }*/
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
