using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavController : MonoBehaviour
{
    NavMeshAgent agent;
    public Vector3 targetLocation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Wander());
    }

    public IEnumerator Wander()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            targetLocation = new Vector3(Random.Range(agent.transform.position.x - 5f, agent.transform.position.x + 5f),
                                                 Random.Range(agent.transform.position.y - 2f, agent.transform.position.y + 2f),
                                                 Random.Range(agent.transform.position.z - 5f, agent.transform.position.z + 5f));
            agent.destination = targetLocation;
        }
        
    }


}
