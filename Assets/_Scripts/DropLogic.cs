using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLogic : MonoBehaviour
{
    public GameObject dirtChild;
    public GameObject grassChild;
    public GameObject sandChild;
    public GameObject stoneChild;

    public Collider activeCollider;

    public GameObject playerObject;

    public Collider playerCollider;


    private void Awake()
    {
        
        playerObject = GameObject.FindGameObjectWithTag("Player");

        playerCollider = playerObject.GetComponent<Collider>();

        Character character = playerObject.GetComponent<Character>();

        BlockType block = character.currentDestroyedBlock;

        if (block == BlockType.Stone)
        {
            stoneChild.SetActive(true);
            activeCollider = stoneChild.GetComponent<Collider>();
        }
    }
    private void Update()
    {
        if (playerCollider.bounds.Intersects(activeCollider.bounds))
        {
            //funktionskald til at tilføje til inventory

            Destroy(gameObject);
        }
    }
}
