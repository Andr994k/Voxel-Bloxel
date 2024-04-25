using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DropLogicINV : MonoBehaviour
{
    public GameObject dirtChildINV;
    public GameObject grassChildINV;
    public GameObject sandChildINV;
    public GameObject stoneChildINV;
    public InventoryManager inventoryManagerINV;

    public Collider activeColliderINV;

    public GameObject playerObjectINV;

    public Collider playerColliderINV;

    public BlockType activeBlockINV = BlockType.Nothing;

    public Item itemtypeINV;


    private void Awake()
    {
        inventoryManagerINV = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        playerObjectINV = GameObject.FindGameObjectWithTag("Player");

        playerColliderINV = playerObjectINV.GetComponent<Collider>();

        Character character = playerObjectINV.GetComponent<Character>();

        BlockType block = character.currentDestroyedBlock;

        if (block == BlockType.Stone)
        {
            stoneChildINV.SetActive(true);
            playerColliderINV = playerObjectINV.GetComponent<Collider>();
            activeBlockINV = BlockType.Stone;
            itemtypeINV = Resources.Load<Item>("Items/Stone");
        }
        if (block == BlockType.Sand)
        {
            sandChildINV.SetActive(true);
            activeColliderINV = sandChildINV.GetComponent<Collider>();
            activeBlockINV = BlockType.Stone;
            itemtypeINV = Resources.Load<Item>("Items/Sand");
        }
        if (block == BlockType.Dirt)
        {
            dirtChildINV.SetActive(true);
            activeColliderINV = dirtChildINV.GetComponent<Collider>();
            activeBlockINV = BlockType.Dirt;
            itemtypeINV = Resources.Load<Item>("Items/Dirt");
        }
        if (block == BlockType.Grass_Dirt)
        {
            grassChildINV.SetActive(true);
            activeColliderINV = grassChildINV.GetComponent<Collider>();
            activeBlockINV = BlockType.Grass_Dirt;
            itemtypeINV = Resources.Load<Item>("Items/Grass");
        }
        if (block == BlockType.Dirt)
        {
            dirtChildINV.SetActive(true);
            activeColliderINV = dirtChildINV.GetComponent<Collider>();
        }
        if (block == BlockType.Sand)
        {
            sandChildINV.SetActive(true);
            activeColliderINV = sandChildINV.GetComponent<Collider>();
        }
        if (block == BlockType.Grass_Dirt)
        {
            grassChildINV.SetActive(true);
            activeColliderINV = grassChildINV.GetComponent<Collider>();
        }
    }
    private void Update()
    {
        if (playerColliderINV.bounds.Intersects(activeColliderINV.bounds))
        {

            inventoryManagerINV.AddItem(itemtypeINV);

            Destroy(gameObject);
        }
    }
}
