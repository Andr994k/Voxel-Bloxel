using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSwitch : MonoBehaviour
{    
    public GameObject stoneBlock;
    public GameObject sandBlock;
    public GameObject grassBlock;
    public GameObject dirtBlock;

    public Character character;


    // Update is called once per frame
    void Update()
    {

        if (character.activeBlock == BlockType.Dirt) 
        {
            stoneBlock.SetActive(false);
            sandBlock.SetActive(false);
            grassBlock.SetActive(false);
            dirtBlock.SetActive(true);
        }
        if (character.activeBlock == BlockType.Stone)
        {
            stoneBlock.SetActive(true);
            sandBlock.SetActive(false);
            grassBlock.SetActive(false);
            dirtBlock.SetActive(false);
        }
        if (character.activeBlock == BlockType.Sand)
        {
            stoneBlock.SetActive(false);
            sandBlock.SetActive(true);
            grassBlock.SetActive(false);
            dirtBlock.SetActive(false);
        }
        if (character.activeBlock == BlockType.Grass_Dirt)
        {
            stoneBlock.SetActive(false);
            sandBlock.SetActive(false);
            grassBlock.SetActive(true);
            dirtBlock.SetActive(false);
        }
    }
}
