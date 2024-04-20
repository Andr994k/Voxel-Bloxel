using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropLogic : MonoBehaviour
{
    public GameObject dirtChild;
    public GameObject grassChild;
    public GameObject dsandChild;
    public GameObject stoneChild;

    private void Awake()
    {
        GameObject playerObject= GameObject.FindGameObjectWithTag("Player");

        Character character = playerObject.GetComponent<Character>();

        BlockType block = character.currentDestroyedBlock;

        if (block == BlockType.Stone)
        {
            stoneChild.SetActive(true);
        }
    }
}
